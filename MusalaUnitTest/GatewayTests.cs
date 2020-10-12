
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Musala.Controllers;
using Musala.Models;
using Musala.Models.DTO;
using Musala.Resources;
using Musala.Resources.Filters;
using Musala.Resources.Mapper;
using System;
using System.Collections.Generic;
using Xunit;

namespace MusalaUnitTest
{
    public class GatewaysTests : DBControllerTest
    {
        public GatewaysTests()
            : base(
                new DbContextOptionsBuilder<MusalaTestContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options)
        {
        }
        GatewaysController controller;


        [Fact]
        public async void TestFindAll()
        {
            using var context = new MusalaTestContext(ContextOptions);
            controller = new GatewaysController(context, new MusalaMapper());
            var result = await controller.GetAll(query: null) as IActionResult;

            Assert.IsAssignableFrom<SuccessPayloadResult>(result);

            SuccessPayloadResult successPayloadResult = result as SuccessPayloadResult;

            Assert.IsAssignableFrom<List<GatewayDto>>(successPayloadResult.GetPayloadForTest().Data);
            List<GatewayDto> gatewayDtos = successPayloadResult.GetPayloadForTest().Data as List<GatewayDto>;

            int gatewaysAmount = await context.Gateway.CountAsync();
            Assert.Equal(gatewaysAmount, gatewayDtos.Count);
        }

        [Fact]
        public async void TestFindAllWithFilters()
        {
            QueryObject query = new QueryObject()
            { 
                Skip=1,
                Take=2,
                Sort="name Asc",
                Filter= "substringof('Gateway',name) eq true and substringof('34.23',ipv4) eq true"
            };
            
            using var context = new MusalaTestContext(ContextOptions);
            controller = new GatewaysController(context, new MusalaMapper());
            
            var result = await controller.GetAll(query:query ) as IActionResult;
            Assert.IsAssignableFrom<SuccessPayloadResult>(result);

            SuccessPayloadResult successPayloadResult = result as SuccessPayloadResult;

            Assert.IsAssignableFrom<List<GatewayDto>>(successPayloadResult.GetPayloadForTest().Data);
            List<GatewayDto> gatewayDtos = successPayloadResult.GetPayloadForTest().Data as List<GatewayDto>;

            Assert.Equal(2, gatewayDtos.Count);
        }

        /// <summary>
        /// Test for ensure that find and element
        /// </summary>
        [Fact]
        public async void TestFindOne()
        {
            using var context = new MusalaTestContext(ContextOptions);
            controller = new GatewaysController(context, new MusalaMapper());

            var firstGateway = await context.Gateway.FirstAsync();
            //find the element in the list
            var result = controller.GetOne(firstGateway.Id.ToString()) as IActionResult;
            Assert.IsAssignableFrom<SuccessPayloadResult>(result);

            SuccessPayloadResult successPayloadResult = result as SuccessPayloadResult;

            Assert.IsAssignableFrom<GatewayDto>(successPayloadResult.GetPayloadForTest().Data);
            GatewayDto gatewayDto = successPayloadResult.GetPayloadForTest().Data as GatewayDto;

            Assert.Equal(firstGateway.Name, gatewayDto.Name);
        }
    }
}
