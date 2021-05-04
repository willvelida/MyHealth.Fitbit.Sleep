using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyHealth.Common;
using MyHealth.Fitbit.Sleep.Services;
using System;
using System.Threading.Tasks;
using mdl = MyHealth.Common.Models;

namespace MyHealth.Fitbit.Sleep.Functions
{
    public class GetDailySleepLog
    {
        private readonly IConfiguration _configuration;
        private readonly IFitbitApiService _fitbitApiService;
        private readonly IMapper _mapper;
        private readonly IServiceBusHelpers _serviceBusHelpers;

        public GetDailySleepLog(
            IConfiguration configuration,
            IFitbitApiService fitbitApiService,
            IMapper mapper,
            IServiceBusHelpers serviceBusHelpers)
        {
            _configuration = configuration;
            _fitbitApiService = fitbitApiService;
            _mapper = mapper;
            _serviceBusHelpers = serviceBusHelpers;
        }

        [FunctionName(nameof(GetDailySleepLog))]
        public async Task Run([TimerTrigger("0 0 5 * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"{nameof(GetDailySleepLog)} executed at: {DateTime.Now}");
                var dateParameter = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                log.LogInformation($"Attempting to retrieve Sleep Log for {dateParameter}");
                var sleepResponse = await _fitbitApiService.GetSleepResponseObject(dateParameter);

                log.LogInformation("Mapping API response to Sleep object");
                var sleep = new mdl.Sleep();
                _mapper.Map(sleepResponse, sleep);

                log.LogInformation("Sending mapped Sleep log to service bus");
                await _serviceBusHelpers.SendMessageToTopic(_configuration["SleepTopic"], sleep);
            }
            catch (Exception ex)
            {
                log.LogError($"Exception thrown in {nameof(GetDailySleepLog)}: {ex.Message}");
                await _serviceBusHelpers.SendMessageToQueue(_configuration["ExceptionQueue"], ex);
                throw ex;
            }
        }
    }
}
