using Microsoft.AspNetCore.Mvc;
using System;
using Vecttor02.Service;

namespace Vecttor02.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AsteroidsController : ControllerBase
    {
        private readonly INasaApiService _nasaApiService;

        public AsteroidsController(INasaApiService nasaApiService)
        {
            _nasaApiService = nasaApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsteroids(int days)
        {
            // Comprobar si el parámetro 'days' está dentro del rango válido
            if (days < 1 || days > 7)
            {
                return BadRequest("El parámetro 'days' debe estar entre 1 y 7.");
            }

            try
            {
                // Obtener los datos de los asteroides utilizando el servicio NasaApiService
                var asteroids = await _nasaApiService.GetAsteroids(days);

                return Ok(asteroids);
            }
            catch (Exception ex)
            {
                // Controlar cualquier excepción
                return StatusCode(500, $"Error al obtener datos de asteroides: {ex.Message}");
            }
        }
    }
}
