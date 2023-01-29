using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackGammonModels;

namespace BackGammonServicesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public readonly JWTAuthenticationManager _JWTAuthenticationManager;

        public WeatherForecastController(JWTAuthenticationManager JWTAuthenticationManager)
        {
            this._JWTAuthenticationManager = JWTAuthenticationManager;
        }

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [AllowAnonymous]
        [HttpPost("Authrize")]
        public IActionResult AuthUser([FromBody] User user)
        {
            var token = _JWTAuthenticationManager.Authenticate(user.UserName, user.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}