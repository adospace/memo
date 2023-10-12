using System.Net.Http.Json;

namespace Memo.BlazorDemo.Pages;

public partial class FetchData
{
    record FetchDataState(bool IsLoading, WeatherForecast[] Forecasts);

    class FetchDataStore : LocalStore<FetchDataState>
    {
        private readonly HttpClient _http;

        public FetchDataStore(HttpClient http)
            : base(() => new(IsLoading: false, Array.Empty<WeatherForecast>()))
        {
            _http = http;
        }
        

        public async Task LoadData()
        {
            Mutate(s => s with { IsLoading = true });

            await Task.Delay(2000);
            
            var forecasts = await _http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json") ?? throw new InvalidOperationException();

            Mutate(s => new FetchDataState(IsLoading: false, Forecasts: forecasts));
        }
    }
}