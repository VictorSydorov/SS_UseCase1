
using MediatR;
using Models;


public class CountriesRequest : IRequest<IEnumerable<Country>>
{    
     public string NameFilter { get; } = string.Empty;
    
}