
using MediatR;
using Models;

public class CountryDataRequest : IRequest<IEnumerable<Country>>
{
    public static string Ascend = "ascend";
    public static string Descend = "descend";

    public string NameFilter { get; set; } 
    public int PopulationFilter { get; set; }
    public string SortByName { get; set; }
    public bool ShouldBeSorted => SortByName.Equals(Ascend) || SortByName.Equals(Descend);
    public int NumberOfCountries { get; set; }

    
    public CountryDataRequest(string? nameFilter, int? populationFilter, string? sortByName, int? numberOfCountries)
    {
        NameFilter = nameFilter ?? string.Empty;
        PopulationFilter = populationFilter is null? 0: populationFilter.Value*1000000;
        SortByName = sortByName is null ? 
                     string.Empty :
                     sortByName.StartsWith("a", StringComparison.InvariantCultureIgnoreCase) ? Ascend : Descend;
        NumberOfCountries = numberOfCountries ?? 0;
    }
}

