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
        public async void SportsController_GetAll_NotFound_WithNoDatabaseConnection()
        {
            var logMock = new Mock<ILogger<SportsController>>();
            var eventsRepo = new EventsRepository("");
            var sportsRepo = new SportsRepository("");

            var spControl = new SportsController(logMock.Object, eventsRepo, sportsRepo);
            var result = await spControl.Get();
            Assert.NotNull(result);

            Assert.IsType<ActionResult<IEnumerable<SportInfo>>>(result);
            Assert.NotNull(result.Result);
            Assert.IsType<NotFoundResult>(result.Result);
            Assert.True(result.Value == null);
            Assert.True((result.Result as NotFoundResult).StatusCode == 404);
        }

        [Fact]
        public async void SportsController_GetSportsWithEventsCount_JsonResult()
        {
            var expected = new List<SportInfo> 
            {
                new SportInfo() { SportId = 1, Name = "First", EventsCount = 12 },
                new SportInfo() { SportId = 2, Name = "Second", EventsCount = 23 },
                new SportInfo() { SportId = 3, Name = "Third", EventsCount = 34 },
            };

            var logMock = new Mock<ILogger<SportsController>>();
            var eventsRepoMock = new Mock<IEventsRepository>();
            var sportsRepoMock = new Mock<ISportsRepository>();
            sportsRepoMock.Setup(
                r => r.GetSportsWithEventsCount())
                .ReturnsAsync(expected);

            var spControl = new SportsController(logMock.Object, eventsRepoMock.Object, sportsRepoMock.Object);
            var result = await spControl.Get();
            Assert.NotNull(result);
            Assert.True(result.Result == null);
            Assert.IsType<ActionResult<IEnumerable<SportInfo>>>(result);
            Assert.IsType<List<SportInfo>>(result.Value);
            Assert.NotNull(result.Value);

            var actual = result.Value;
            var pairs = expected.Zip(actual, (exp, act) => new { Expected = exp, Actual = act });

            foreach (var p in pairs)
            {
                Assert.Equal(p.Expected, p.Actual);
            }
        }

        [Fact]
        public async void SportsController_GetEventsBySportAndDate_JsonResult()
        {
            var expected = new List<SportEvent>
            {
                new SportEvent()
                {
                    EventId = 1,
                    SportId = 2,
                    EventName = "Fake Event",
                    EventDate = new DateTime(1870, 4, 22),
                    Team1Price = 1.23M,
                    Team2Price = 4.56M,
                    DrawPrice = 7.89M
                },
                new SportEvent()
                {
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

            var spControl = new SportsController(logMock.Object, eventsRepoMock.Object, sportsRepoMock.Object);
            var result = await spControl.Get(1, DateTime.Now);
            Assert.NotNull(result);
            Assert.True(result.Result == null);
            Assert.IsType<ActionResult<IEnumerable<SportEvent>>>(result);
            Assert.IsType<List<SportEvent>>(result.Value);
            Assert.NotNull(result.Value);

            var actual = result.Value;
            var pairs = expected.Zip(actual, (exp, act) => new { Expected = exp, Actual = act });

            foreach (var p in pairs)
            {
                Assert.Equal(p.Expected, p.Actual);
            }
        }
    }
}
