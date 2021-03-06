﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Musala.Controllers;
using Musala.DAL;
using Musala.Models;
using Musala.Resources;
using Musala.Resources.Mapper;
using System;
using Xunit;

namespace MusalaUnitTest
{
    public class PeripheralTests : DBControllerTest
    {
        public PeripheralTests()
            : base(
                new DbContextOptionsBuilder<MusalaContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options)
        { }

        PeripheralsController controller;

        [Fact]
        public async void TestFindAll()
        {
            using var context= new MusalaContext(ContextOptions);
            controller = new PeripheralsController(context, new MusalaMapper());
            var result = await controller.GetAll(null) as IActionResult;
            Assert.IsAssignableFrom<SuccessPayloadResult>(result);
        }
    }
}

