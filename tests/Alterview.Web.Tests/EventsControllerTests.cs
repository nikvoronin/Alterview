using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using Alterview.Infrastructure.Data;
using Alterview.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace Alterview.Web.Tests
{
    public class EventsControllerTests
    {
        [Fact]
        public async void EventsController_GetById_ResponseWithBadRequest_WhenNoDbConnection()
        {
            var logMock = new Mock<ILogger<EventsController>>();
            var evRepo = new EventsRepository("");

            EventsController evControl = new EventsController(logMock.Object, evRepo);
            var result = await evControl.Get(1);
            Assert.NotNull(result);

            Assert.IsType<BadRequestResult>(result);
            var br = (BadRequestResult)result;
            Assert.True(br.StatusCode == 400);
        }

        [Fact]
        public async void EventsController_GetById_JsonResult()
        {
            var logMock = new Mock<ILogger<EventsController>>();
            var evRepoMock = new Mock<IEventsRepository>();

            SportEvent expectedEvent =
                new SportEvent()
                {
                    EventId = 1,
                    SportId = 2,
                    EventName = "Fake Event",
                    EventDate = new DateTime(1870, 4, 22),
                    Team1Price = 1.23M,
                    Team2Price = 4.56M,
                    DrawPrice = 7.89M
                };

            evRepoMock.Setup(
                r => r.GetEventById(It.IsAny<int>()))
                .ReturnsAsync(expectedEvent);

            EventsController evControl = new EventsController(logMock.Object, evRepoMock.Object);
            var result = await evControl.Get(1);
            Assert.NotNull(result);
            Assert.IsType<JsonResult>(result);

            var jsonResult = (JsonResult)result;
            Assert.IsType<SportEvent>(jsonResult.Value);
            SportEvent actualEvent = (SportEvent)jsonResult.Value;

            Assert.Equal(expectedEvent, actualEvent);
        }
    }
}
