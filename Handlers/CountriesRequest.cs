
using MediatR;
using Models;


public class CountriesRequest : IRequest<IEnumerable<Country>>
{    
     public string NameFilter { get; set; } = string.Empty;
     public int PopulationFilter { get; set; } = 0;
    
}