using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using Alterview.Infrastructure.Data;
using Alterview.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Alterview.Web.Tests
{
    public class SportsControllerTests
    {
        [Fact]
        public async void SportsController_GetAll_BadRequestWithNoDatabaseConnection()
        {
            var logMock = new Mock<ILogger<SportsController>>();
            var eventsRepo = new EventsRepository("");
            var sportsRepo = new SportsRepository("");

            SportsController spControl = new SportsController(logMock.Object, eventsRepo, sportsRepo);
            var result = await spControl.GetAll();
            Assert.NotNull(result);

            Assert.IsType<BadRequestResult>(result);
            var br = (BadRequestResult)result;
            Assert.True(br.StatusCode == 400);
        }

        [Fact]
        public async void SportsController_GetAll_JsonResult()
        {
            SportInfo[] expected = new SportInfo[] {
                new SportInfo(){ SportId = 1, Name = "First", EventsCount = 12 },
                new SportInfo(){ SportId = 2, Name = "Second", EventsCount = 23 },
                new SportInfo(){ SportId = 3, Name = "Third", EventsCount = 34 },
            };

            var logMock = new Mock<ILogger<SportsController>>();
            var eventsRepoMock = new Mock<IEventsRepository>();
            var sportsRepoMock = new Mock<ISportsRepository>();
            sportsRepoMock.Setup(
                r => r.GetSportsWithEventsCount())
                .ReturnsAsync(expected);

            SportsController spControl = new SportsController(logMock.Object, eventsRepoMock.Object, sportsRepoMock.Object);
            var result = await spControl.GetAll();
            Assert.NotNull(result);

            Assert.IsType<JsonResult>(result);
            var jsonResult = (JsonResult)result;

            var actual = (IEnumerable<SportInfo>)jsonResult.Value;
            var pairs = expected.Zip(actual, (exp, act) => new { Expected = exp, Actual = act });

            foreach (var p in pairs)
                Assert.Equal(p.Expected, p.Actual);
        }

        [Fact]
        public async void SportsController_GetEventsBySportAndDate_JsonResult()
        {
            SportEvent[] expected = new SportEvent[] {
                new SportEvent() {
                    EventId = 1,
                    SportId = 2,
                    EventName = "Fake Event",
                    EventDate = new DateTime(1870, 4, 22),
                    Team1Price = 1.23M,
                    Team2Price = 4.56M,
                    DrawPrice = 7.89M
                },
                new SportEvent() {
                    EventId = 2,
                    SportId = 3,
                    EventName = "FFake EEvent",
                    EventDate = new DateTime(1879, 11, 7),
                    Team1Price = 3.21M,
                    Team2Price = 6.54M,
                    DrawPrice = 9.87M
                },
        };

            var logMock = new Mock<ILogger<SportsController>>();
            var eventsRepoMock = new Mock<IEventsRepository>();
            var sportsRepoMock = new Mock<ISportsRepository>();
            eventsRepoMock.Setup(
                r => r.GetEventsBySportAndDate(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(expected);

            SportsController spControl = new SportsController(logMock.Object, eventsRepoMock.Object, sportsRepoMock.Object);
            var result = await spControl.Get(1, DateTime.Now);
            Assert.NotNull(result);

            Assert.IsType<JsonResult>(result);
            var jsonResult = (JsonResult)result;

            var actual = (IEnumerable<SportEvent>)jsonResult.Value;
            var pairs = expected.Zip(actual, (exp, act) => new { Expected = exp, Actual = act });

            foreach (var p in pairs)
                Assert.Equal(p.Expected, p.Actual);
        }
    }
}
