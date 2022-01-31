using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyCompany.NameProject.Domain.Entities.Geocoder;
using MyCompany.NameProject.Domain.Entities.Weather;

namespace MyCompany.NameProject.Application.Common.Interfaces.Geocoder
{
    public interface IGeocoderService
    {
        CoordinatesData GetCoordinates(string city);

        IEnumerable<string> GetAllCities();
    }
}
