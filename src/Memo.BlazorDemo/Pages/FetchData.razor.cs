using System.Net.Http.Json;

namespace Memo.BlazorDemo.Pages;

public partial class FetchData
{
    record FetchDataState(bool IsLoading, WeatherForecast[] Forecasts);

    class FetchDataStore : LocalStore<FetchDataState>
    {
        private readonly HttpClient _http;

        public FetchDataStore(HttpClient http)
        {
            _http = http;
        }
        
        protected override FetchDataState InitialState() 
            => new(IsLoading: false, Array.Empty<WeatherForecast>());

        public async Task LoadData()
        {
            Mutate(State with { IsLoading = true });

            await Task.Delay(2000);
            
            var forecasts = await _http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json") ?? throw new InvalidOperationException();

            Mutate(new FetchDataState(IsLoading: false, Forecasts: forecasts));
        }
    }
}