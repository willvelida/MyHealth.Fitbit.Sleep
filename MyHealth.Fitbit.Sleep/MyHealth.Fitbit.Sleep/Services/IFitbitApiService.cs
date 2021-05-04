using MyHealth.Fitbit.Sleep.Models;
using System.Threading.Tasks;

namespace MyHealth.Fitbit.Sleep.Services
{
    public interface IFitbitApiService
    {
        Task<SleepResponseObject> GetSleepResponseObject(string date);
    }
}
