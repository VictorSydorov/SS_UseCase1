
using MediatR;
using Models;


public class CountriesRequest : IRequest<IEnumerable<Country>>
{    
     public string NameFilter { get; set; } = string.Empty;
     public int PopulationFilter { get; set; } = 0;
     public string SortByName { get; set; } = string.Empty;

    public bool ShouldBeSorted => SortByName.Equals(Ascend) || SortByName.Equals(Descend);

    public static string Ascend = "ascend";
    public static string Descend = "descend";
}

