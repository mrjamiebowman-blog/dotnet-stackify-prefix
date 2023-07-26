using MrJB.OTel.Prefix.Data.Models;

namespace MrJB.OTel.Prefix.Data.Services;

public interface IDataService
{
    public Task<WeatherForecast[]> GetDataAsync();
}
