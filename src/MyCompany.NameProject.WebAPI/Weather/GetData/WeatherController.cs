using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCompany.NameProject.Application.Weather.GetData;
using MyCompany.NameProject.WebAPI.Common;
using System.Threading.Tasks;

namespace MyCompany.NameProject.WebAPI.Weather.GetData
{
    public class WeatherController : ApiController
    {
        private readonly Presenter _presenter;

        public WeatherController(Presenter presenter) => (_presenter) = (presenter);

        /// <summary>
        /// Получение данных погоды.
        /// </summary>
        /// <param name="code">Код пользователя, полученный от ЕСИА</param>
        /// <param name="state">Уникальный идентификатор UUID, отправленный ранее для авторизации пользователя в ЕСИА</param>
        /// <returns>Модель с данными Пользователя из ЕСИА</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("data")]
        public async Task<IActionResult> GetDataAsync([FromQuery] string city)
        {
            var output = await Mediator.Send(new GetDataWeatherQuery { City = city });
            _presenter.Populate(output);
            return _presenter.ViewModel;
        }
    }
}
