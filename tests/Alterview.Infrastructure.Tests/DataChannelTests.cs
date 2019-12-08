using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using Alterview.Infrastructure.Entities;
using Moq;
using Xunit;

namespace Alterview.Infrastructure.Tests
{
    public class DataChannelTests
    {
        [Fact]
        public void DataChannel_Creation_ExceptionOnNullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => new DataChannel<SportEvent>(null));
        }

        [Fact]
        public void DataChannel_Creation_IdAutoGrows()
        {
            var outpCmd = new Mock<IAsyncCommand<SportEvent>>();

            int startId = new DataChannel<SportEvent>(outpCmd.Object).Id;
            DataChannel<SportEvent> ch = null;
            for (int i = 0; i < 10; i++)
            {
                ch = new DataChannel<SportEvent>(outpCmd.Object);
                Assert.NotNull(ch);
            }

            Assert.NotNull(ch);
            Assert.True(startId + 10 == ch.Id);
        }

        [Fact]
        public async void DataChannel_PushData()
        {
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

            var outputCommandMock = new Mock<IAsyncCommand<SportEvent>>();
            outputCommandMock.Setup(c => c.ExecuteAsync(It.IsAny<SportEvent>())).ReturnsAsync(1);

            var ch = new DataChannel<SportEvent>(outputCommandMock.Object);

            var rnd = new Random(Environment.TickCount);
            int times = rnd.Next(10, 21);

            for (int i = 0; i < times; i++)
            {
                ch.PushData(expectedEvent);
            }

            Assert.NotNull(ch.Worker);

            await Task.Delay(500); // wait for multithreading background work
            outputCommandMock.Verify(c => c.ExecuteAsync(It.IsAny<SportEvent>()), Times.Exactly(times));

            ch.Abort();

            await Task.Delay(500);
        }
    }
}
