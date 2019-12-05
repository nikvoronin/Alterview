using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using Alterview.Infrastructure.Data;
using Moq;
using System;
using Xunit;

namespace Alterview.Infrastructure.Tests
{
    public class EventsRepositoryTests
    {
        [Fact]
        public void EventsRepository_EmptyConnectionString_ExceptionWhenQuery()
        {
            var repo = new EventsRepository("");

            Assert.ThrowsAsync<AggregateException>(async () => await repo.GetEventById(1));
            Assert.ThrowsAsync<AggregateException>(async () => await repo.GetEventsBySportAndDate(1, DateTime.Now));

            var eventMock = new Mock<SportEvent>();
            Assert.ThrowsAsync<AggregateException>(async () => await repo.UpdateEvent(eventMock.Object));
        }

        [Fact]
        public async void EventsRepository_UpdateEvent_NullArguments()
        {
            var evRepoMock = new Mock<IEventsRepository>();
            evRepoMock.Setup(e => e.UpdateEvent(It.IsNotNull<SportEvent>())).ReturnsAsync(1);
            evRepoMock.Setup(e => e.UpdateEvent(It.Is<SportEvent>(v => v == null))).ThrowsAsync(new ArgumentNullException("ev"));

            Assert.True(1 == await evRepoMock.Object.UpdateEvent(new SportEvent()));
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await evRepoMock.Object.UpdateEvent(null));
        }
    }
}
