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
    }
}
