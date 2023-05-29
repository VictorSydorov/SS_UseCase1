using MediatR;
using Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SS_UseCase1.Handlers
{
    public class CountriesRequestHandler : IRequestHandler<CountriesRequest, IEnumerable<Country>>
    {
        private readonly HttpClient httpClient;

        public CountriesRequestHandler(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.BaseAddress = new Uri("https://restcountries.com/v3.1/all");
        }

        public async Task<IEnumerable<Country>> Handle(CountriesRequest request, CancellationToken cancellationToken)
        {
            var result  = await httpClient.GetAsync("https://restcountries.com/v3.1/all");
            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                var countries = JsonSerializer.Deserialize<IEnumerable<Country>>(json, CreateOptions())!;

                return countries;
            }
            else {
                throw new Exception("Error getting data.");            
            }           
        }

        private JsonSerializerOptions CreateOptions()
        {
            var options = new JsonSerializerOptions();
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString;
            return options;
        }
    }
}
