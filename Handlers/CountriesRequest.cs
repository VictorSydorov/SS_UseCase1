
using MediatR;
using Models;


public class CountriesRequest : IRequest<IEnumerable<Country>>
{
    public CountriesRequest()
    {
    }
}