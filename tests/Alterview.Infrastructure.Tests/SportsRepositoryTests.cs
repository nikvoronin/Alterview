using Alterview.Infrastructure.Data;
using System;
using Xunit;

namespace Alterview.Infrastructure.Tests
{
    public class SportsRepositoryTests
    {
        [Fact]
        public void SportsRepository_EmptyConnectionString_ExceptionWhenQuery()
        {
            var repo = new SportsRepository("");
            Assert.ThrowsAsync<AggregateException>(async () => await repo.GetSportsWithEventsCount());
        }
    }
}
