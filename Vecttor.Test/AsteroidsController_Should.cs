using Microsoft.AspNetCore.Mvc;
using Vecttor02.Controllers;
using Vecttor02.Models;
using Vecttor02.Service;

namespace Vecttor.Tests
{
    public class AsteroidsControllerTests
    {
        [Fact]
        public async Task ValidarGetAsteroids()
        {
            // Arrange
            var nasaApiServiceMock = new NasaApiServiceMock();
            var controller = new AsteroidsController(nasaApiServiceMock);

            // Act
            var result = await controller.GetAsteroids(3);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Respuesta 200
            var asteroids = Assert.IsAssignableFrom<List<Asteroid>>(okResult.Value); // Contiene objetos válidos
            Assert.Equal(3, asteroids.Count); // Verificar que se devuelven 3 asteroides
        }

        [Fact]
        public async Task ValidarErrorGetAsteroids()
        {
            // Arrange
            var nasaApiServiceMock = new NasaApiServiceMock();
            var controller = new AsteroidsController(nasaApiServiceMock);

            // Act
            var result = await controller.GetAsteroids(10); // Pasamos un número de días inválido

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Error 400
            Assert.Equal("El parámetro 'days' debe estar entre 1 y 7.", badRequestResult.Value); //Respuesta esperada en error
        }
    }
}