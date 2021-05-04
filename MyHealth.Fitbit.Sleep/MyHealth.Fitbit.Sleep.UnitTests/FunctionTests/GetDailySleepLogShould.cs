using AutoMapper;
using FluentAssertions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MyHealth.Common;
using MyHealth.Fitbit.Sleep.Functions;
using MyHealth.Fitbit.Sleep.Models;
using MyHealth.Fitbit.Sleep.Services;
using System;
using System.Threading.Tasks;
using Xunit;
using mdl = MyHealth.Common.Models;

namespace MyHealth.Fitbit.Sleep.UnitTests.FunctionTests
{
    public class GetDailySleepLogShould
    {
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IFitbitApiService> _mockFitbitApiService;
        private Mock<IMapper> _mockMapper;
        private Mock<IServiceBusHelpers> _mockServiceBusHelpers;
        private Mock<ILogger> _mockLogger;
        private TimerInfo _testTimerInfo;

        private GetDailySleepLog _func;

        public GetDailySleepLogShould()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockFitbitApiService = new Mock<IFitbitApiService>();
            _mockMapper = new Mock<IMapper>();
            _mockServiceBusHelpers = new Mock<IServiceBusHelpers>();
            _mockLogger = new Mock<ILogger>();
            _testTimerInfo = default(TimerInfo);

            _func = new GetDailySleepLog(
                _mockConfiguration.Object,
                _mockFitbitApiService.Object,
                _mockMapper.Object,
                _mockServiceBusHelpers.Object);
        }

        [Fact]
        public async Task RetrieveSleepLogResponseAndSendMappedObjectToSleepTopic()
        {
            // Arrange
            _mockFitbitApiService.Setup(x => x.GetSleepResponseObject(It.IsAny<string>())).ReturnsAsync(It.IsAny<SleepResponseObject>());
            _mockMapper.Setup(x => x.Map(It.IsAny<SleepResponseObject>(), It.IsAny<mdl.Sleep>())).Verifiable();
            _mockServiceBusHelpers.Setup(x => x.SendMessageToTopic(It.IsAny<string>(), It.IsAny<mdl.Sleep>())).Returns(Task.CompletedTask);

            // Act
            Func<Task> getDailySleepAction = async () => await _func.Run(_testTimerInfo, _mockLogger.Object);

            // Assert
            await getDailySleepAction.Should().NotThrowAsync<Exception>();
            _mockServiceBusHelpers.Verify(x => x.SendMessageToTopic(It.IsAny<string>(), It.IsAny<mdl.Sleep>()), Times.Once);
            _mockServiceBusHelpers.Verify(x => x.SendMessageToQueue(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }

        [Fact]
        public async Task ThrowAndCatchExceptionWhenFitApiServiceThrowsException()
        {
            // Arrange
            _mockFitbitApiService.Setup(x => x.GetSleepResponseObject(It.IsAny<string>())).ThrowsAsync(new Exception());

            // Act
            Func<Task> getDailySleepAction = async () => await _func.Run(_testTimerInfo, _mockLogger.Object);

            // Assert
            await getDailySleepAction.Should().ThrowAsync<Exception>();
            _mockServiceBusHelpers.Verify(x => x.SendMessageToTopic(It.IsAny<string>(), It.IsAny<mdl.Sleep>()), Times.Never);
            _mockServiceBusHelpers.Verify(x => x.SendMessageToQueue(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task ThrowAndCatchExceptionWhenMapperThrowsException()
        {
            // Arrange
            _mockFitbitApiService.Setup(x => x.GetSleepResponseObject(It.IsAny<string>())).ReturnsAsync(It.IsAny<SleepResponseObject>());
            _mockMapper.Setup(x => x.Map(It.IsAny<SleepResponseObject>(), It.IsAny<mdl.Sleep>())).Throws(new Exception());

            // Act
            Func<Task> getDailySleepAction = async () => await _func.Run(_testTimerInfo, _mockLogger.Object);

            // Assert
            await getDailySleepAction.Should().ThrowAsync<Exception>();
            _mockServiceBusHelpers.Verify(x => x.SendMessageToTopic(It.IsAny<string>(), It.IsAny<mdl.Sleep>()), Times.Never);
            _mockServiceBusHelpers.Verify(x => x.SendMessageToQueue(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task ThrowAndCatchExceptionWhenSendMessageToTopicThrowsException()
        {
            // Arrange
            _mockFitbitApiService.Setup(x => x.GetSleepResponseObject(It.IsAny<string>())).ReturnsAsync(It.IsAny<SleepResponseObject>());
            _mockMapper.Setup(x => x.Map(It.IsAny<SleepResponseObject>(), It.IsAny<mdl.Sleep>())).Verifiable();
            _mockServiceBusHelpers.Setup(x => x.SendMessageToTopic(It.IsAny<string>(), It.IsAny<mdl.Sleep>())).ThrowsAsync(new Exception());

            // Act
            Func<Task> getDailySleepAction = async () => await _func.Run(_testTimerInfo, _mockLogger.Object);

            // Assert
            await getDailySleepAction.Should().ThrowAsync<Exception>();
            _mockServiceBusHelpers.Verify(x => x.SendMessageToQueue(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }
    }
}
