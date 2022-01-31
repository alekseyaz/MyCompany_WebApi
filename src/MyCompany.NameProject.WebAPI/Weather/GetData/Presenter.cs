using Microsoft.AspNetCore.Mvc;
using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.Domain.Entities.Weather;
using MyCompany.NameProject.WebAPI.Weather.GetData.Models;

namespace MyCompany.NameProject.WebAPI.Weather.GetData
{
    public class Presenter
    {
        private readonly IMapper<WeatherData, WeatherDataResponseModel> _mapper;

        public IActionResult ViewModel { get; internal set; }

        public Presenter(IMapper<WeatherData, WeatherDataResponseModel> mapper)
        {
            _mapper = mapper;
        }

        public void Populate(WeatherData output)
        {
            ViewModel = new OkObjectResult(_mapper.Map(output));
        }
    }
}
