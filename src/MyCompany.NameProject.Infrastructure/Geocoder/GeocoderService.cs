using MyCompany.NameProject.Application.Common.Exceptions;
using MyCompany.NameProject.Application.Common.Interfaces.Geocoder;
using MyCompany.NameProject.Domain.Entities.Geocoder;
using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.Infrastructure.Geocoder.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyCompany.NameProject.Infrastructure.Geocoder
{
    public class GeocoderService : IGeocoderService
    {
        private readonly IMapper<Coordinates, CoordinatesData> _dataMapper;

        public GeocoderService(
            IMapper<Coordinates, CoordinatesData> dataMapper)
        {
            _dataMapper = dataMapper;
        }

        public IEnumerable<string> GetAllCities()
        {
            var cities = new List<string>
            {
                "Moscow",
                "St.Peretburg",
                "Krasnodar",
                "Orengurg",
                "Kaliningrad"
            };

            return cities;
        }

        public CoordinatesData GetCoordinates(string city)
        {
            //TODO Сдесь по хорошему должен быть полноценный сервис геокодера определяющий координаты города по наименованию, но пока так
            var parts = new List<KeyValuePair<string, Coordinates>>
            {
                new KeyValuePair<string, Coordinates>("Moscow", new Coordinates{ Lat = "55.755819", Lon = "37.617644"}),
                new KeyValuePair<string, Coordinates>("St.Peretburg", new Coordinates{ Lat = "59.939099", Lon = "30.315877"}),
                new KeyValuePair<string, Coordinates>("Krasnodar", new Coordinates{ Lat = "45.035470", Lon = "38.975313"}),
                new KeyValuePair<string, Coordinates>("Orengurg", new Coordinates{ Lat = "51.768205", Lon = "55.096964"}),
                new KeyValuePair<string, Coordinates>("Kaliningrad", new Coordinates{ Lat = "54.710162", Lon = "20.510137"})
            };

            var coordinates = parts.FirstOrDefault(c => c.Key == city).Value;

            if (coordinates == null)
                throw new NotFoundException($"City with name \"{city}\" not found");

            return _dataMapper.Map(coordinates);
        }
    }
}
