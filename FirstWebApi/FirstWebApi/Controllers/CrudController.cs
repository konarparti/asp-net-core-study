using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApi.Controllers
{
    [ApiController]
    [Route("api/crud")]
    public class CrudController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<CrudController> _logger;
        private readonly Repository _repository;

        public CrudController(ILogger<CrudController> logger, Repository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost, Route("create")]
        public IActionResult Create([FromBody] WeatherForecast weatherForecast)
        {
            _repository.Weather.Add(weatherForecast);
            return Ok();
        }

        [HttpPut, Route("update")]
        public IActionResult Update([FromBody] WeatherForecast weatherForecast)
        {
            var value = _repository.Weather.FindLast(x => x.Date == weatherForecast.Date);
            if (value is null)
            {
                _repository.Weather.Add(weatherForecast);
            }
            else
            {
                value.TemperatureC = weatherForecast.TemperatureC;
            }
            return Ok();
        }

        [HttpDelete, Route("delete")]
        public IActionResult Delete([FromBody] Dates dates)
        {
            if (dates.DateFrom >= dates.DateTo)
            {
                return Problem("Вторая дата должна быть больше первой");
            }
            var value = _repository.Weather.FindLast(w => w.Date >= dates.DateFrom && w.Date <= dates.DateTo);
            if (value is null)
            {
                return Problem("Не надйдено для удаления");
            }
            _repository.Weather.Remove(value);
            return Ok();
        }

        [HttpGet, Route("get")]
        public IEnumerable<WeatherForecast> Get([FromBody] Dates dates)
        {
            if (dates.DateFrom >= dates.DateTo)
            {
                return null;
            }
            return _repository.Weather.FindAll(w => w.Date >= dates.DateFrom && w.Date <= dates.DateTo).ToArray();
        }

        [HttpGet, Route("getall")]
        public IEnumerable<WeatherForecast> GetAll()
        {
            return _repository.Weather.ToArray();
        }

    }
}
