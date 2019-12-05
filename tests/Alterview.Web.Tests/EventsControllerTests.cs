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
        public async void EventsController_GetById_NotFound_WhenNoDbConnection()
        {
            var logMock = new Mock<ILogger<EventsController>>();
            var evRepo = new EventsRepository("");

            var evControl = new EventsController(logMock.Object, evRepo);
            var result = await evControl.Get(1);
            Assert.NotNull(result);

            Assert.IsType<ActionResult<SportEvent>>(result);
            Assert.NotNull(result.Result);
            Assert.IsType<NotFoundResult>(result.Result);
            Assert.True(result.Value == null);
            Assert.True((result.Result as NotFoundResult).StatusCode == 404);
        }

        [Fact]
        public async void EventsController_GetById_PositiveResult()
        {
            var logMock = new Mock<ILogger<EventsController>>();
            var evRepoMock = new Mock<IEventsRepository>();

            var expectedEvent =
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

            var evControl = new EventsController(logMock.Object, evRepoMock.Object);
            var result = await evControl.Get(1);
            
            Assert.NotNull(result);
            Assert.True(result.Result == null);
            Assert.IsType<ActionResult<SportEvent>>(result);
            Assert.IsType<SportEvent>(result.Value);
            Assert.NotNull(result.Value);

            Assert.Equal(expectedEvent, result.Value);
        }
    }
}
