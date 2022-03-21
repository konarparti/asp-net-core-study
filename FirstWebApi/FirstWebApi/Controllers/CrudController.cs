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

        [HttpGet, Route("getall")]
        public IEnumerable<WeatherForecast> GetAll()
        {
            return _repository.Weather.ToArray();
        }

    }
}
