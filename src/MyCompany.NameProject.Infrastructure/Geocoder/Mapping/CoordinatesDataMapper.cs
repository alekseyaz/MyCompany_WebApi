using MyCompany.NameProject.Domain.Entities.Geocoder;
using MyCompany.NameProject.Domain.Entities.Weather;
using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.Infrastructure.Geocoder.Models;
using MyCompany.NameProject.Infrastructure.RestEndpoints.Weather.Models;

namespace MyCompany.NameProject.Infrastructure.Geocoder.Mapping
{
    internal class CoordinatesDataMapper : IMapper<Coordinates, CoordinatesData>
    {
        public CoordinatesData Map(Coordinates source)
        {
            if (source == null)
                return null;

            return new CoordinatesData
            {
                Lat = source.Lat,
                Lon = source.Lon
            };
        }
    }
}
