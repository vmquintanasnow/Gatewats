using Microsoft.EntityFrameworkCore;
using Musala.DAL;
using Musala.Domain.Entity;
using System;
using System.Collections.Generic;

namespace MusalaUnitTest
{
    public class DBControllerTest
    {
        protected DBControllerTest(DbContextOptions<MusalaContext> contextOptions)
        {
            ContextOptions = contextOptions;

            Seed();
        }

        protected DbContextOptions<MusalaContext> ContextOptions { get; }

        private void Seed()
        {
            using (var context = new MusalaContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var gateway1 = new Gateway()
                {
                    Ipv4 = "10.98.87.1",
                    Name = "Gateway 1",
                };

                var gateway2 = new Gateway()
                {
                    Ipv4 = "10.98.87.1",
                    Name = "Gateway 2",
                };

                List<Gateway> gateways = new List<Gateway>()
                {
                    gateway1,
                    gateway2,                    
                    new Gateway
                    {
                        Ipv4 = "34.23.168.1",
                        Name = "Gateway 3"
                    }
                    ,
                    new Gateway
                    {
                        Ipv4 = "34.23.4.1",
                        Name = "Gateway 4"
                    },
                    new Gateway
                    {
                        Ipv4 = "34.23.5.1",
                        Name = "Gateway 5"
                    },
                    new Gateway
                    {
                        Ipv4 = "34.23.6.1",
                        Name = "Gateway 6"
                    }
                };

                List<Device> peripherals = new List<Device>();

                for (int i = 0; i < 10; i++)
                {
                    peripherals.Add(new Device()
                    {
                        Vendor = $"Device {i+1}",
                        Status = true,
                        DateCreation = DateTime.Now,
                        Gateway = gateway1,
                    });

                    if (i < 9)
                    {
                        peripherals.Add(new Device()
                        {
                            Vendor = $"Device {i + 11}",
                            Status = true,
                            DateCreation = DateTime.Now,
                            Gateway = gateway2,
                        });
                    }
                }

                context.Gateway.AddRange(gateways);
                context.Peripheral.AddRange(peripherals);
                context.SaveChanges();
            }
        }
    }
}
