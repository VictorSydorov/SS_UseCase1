using MediatR;
using Models;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SS_UseCase1.Handlers
{
    public class CountriesRequestHandler : IRequestHandler<CountryDataRequest, IEnumerable<Country>>
    {
        private readonly HttpClient httpClient;

        public CountriesRequestHandler(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.BaseAddress = new Uri("https://restcountries.com/v3.1/all");
        }

        public async Task<IEnumerable<Country>> Handle(CountryDataRequest request, CancellationToken cancellationToken)
        {
            var result = await httpClient.GetAsync("https://restcountries.com/v3.1/all");
            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                var countries = JsonSerializer.Deserialize<IEnumerable<Country>>(json, CreateOptions())!;

                return DataProcessor.ProcessCountries(countries, request);
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


        private class DataProcessor {
            private readonly Func<CountryDataRequest, bool> canProcess;
            private readonly Func<IEnumerable<Country>, CountryDataRequest, IEnumerable<Country>> process;

            private static readonly DataProcessor[] _processors;

            static DataProcessor()
            {
                _processors = new DataProcessor[] {

                    new DataProcessor(r=> !string.IsNullOrEmpty(r.NameFilter),  
                        (countries, request) => countries.Where(c=>c.Name.Common.Contains(request.NameFilter, StringComparison.InvariantCultureIgnoreCase))),

                    new DataProcessor(r => r.PopulationFilter > 0, 
                        (countries, request) => countries.Where(c=>c.Population < request.PopulationFilter)),
                    
                    new DataProcessor(r => r.ShouldBeSorted,
                        (countries, request) => 
                            request.SortByName.Equals(CountryDataRequest.Ascend)?
                            countries.OrderBy(c => c.Name.Common) : 
                            countries.OrderByDescending(c => c.Name.Common)),

                    new DataProcessor(r => r.NumberOfCountries > 0,
                        (countries, request) =>countries.Take(request.NumberOfCountries))
                };
            }

            private DataProcessor(Func<CountryDataRequest, bool> canProcess, Func<IEnumerable<Country>, CountryDataRequest, IEnumerable<Country>> process)
            {
                this.canProcess = canProcess;
                this.process = process;
            }

            public IEnumerable<Country> Process(IEnumerable<Country> countries, CountryDataRequest countriesRequest) {
                if (canProcess(countriesRequest))
                {
                    return process(countries, countriesRequest);
                }
                else return countries;
            }

            public static IEnumerable<Country> ProcessCountries(IEnumerable<Country> countries, CountryDataRequest countriesRequest) {
                foreach (var proc in _processors)
                {
                    countries = proc.Process(countries, countriesRequest);
                }
                return countries;
            }
        }
    }
}
