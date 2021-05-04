using Microsoft.Extensions.Configuration;
using MyHealth.Common;
using MyHealth.Fitbit.Sleep.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyHealth.Fitbit.Sleep.Services
{
    public class FitbitApiService : IFitbitApiService
    {
        private readonly IConfiguration _configuration;
        private readonly IKeyVaultHelper _keyVaultHelper;
        private HttpClient _httpClient;

        public FitbitApiService(
            IConfiguration configuration,
            IKeyVaultHelper keyVaultHelper,
            HttpClient httpClient)
        {
            _configuration = configuration;
            _keyVaultHelper = keyVaultHelper;
            _httpClient = httpClient;
        }

        public async Task<SleepResponseObject> GetSleepResponseObject(string date)
        {
            try
            {
                var fitbitAccessToken = await _keyVaultHelper.RetrieveSecretFromKeyVaultAsync(_configuration["AccessTokenName"]);
                _httpClient.DefaultRequestHeaders.Clear();
                Uri getDailySleepLogUri = new Uri($"https://api.fitbit.com/1/user/-/sleep/date/{date}.json");
                var request = new HttpRequestMessage(HttpMethod.Get, getDailySleepLogUri);
                request.Content = new StringContent("");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", fitbitAccessToken.Value);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                var sleepResponse = JsonConvert.DeserializeObject<SleepResponseObject>(responseString);

                return sleepResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
