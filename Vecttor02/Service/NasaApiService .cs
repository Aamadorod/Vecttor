using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Vecttor02.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Vecttor02.Service
{
    public interface INasaApiService
    {        
        Task<List<Asteroid>> GetAsteroids(int days);
    }
    public class NasaApiService : INasaApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://api.nasa.gov/neo/rest/v1/feed";
        private const string API_KEY = "zdUP8ElJv1cehFM0rsZVSQN7uBVxlDnu4diHlLSb";

        public NasaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Asteroid>> GetAsteroids(int days)
        {
            // Obtener la fecha de inicio y fin y montar la url
            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(days);
            string url = $"{ApiBaseUrl}?start_date={startDate:yyyy-MM-dd}&end_date={endDate:yyyy-MM-dd}&api_key={API_KEY}";

            // Realizar el GET a la API de la NASA
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Comprobar si se realizó correctamente
            if (response.IsSuccessStatusCode)
            {
                // Leer y parsear el contenido JSON de la respuesta
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject responseData = JObject.Parse(jsonResponse);

                // Extraer los datos de los asteroides
                return ProcessAsteroids(responseData, startDate, endDate);
            }
            else
            {
                // Si hubo error en el GET lanzar una excepción
                response.EnsureSuccessStatusCode();
                return null;
            }
        }

        private List<Asteroid> ProcessAsteroids(JObject responseData, DateTime startDate, DateTime endDate)
        {
            List<Asteroid> asteroidList = new List<Asteroid>();

            // Recorrer las fechas
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Obtener los datos de los asteroides para la fecha actual
                JArray asteroidsData = responseData["near_earth_objects"][date.ToString("yyyy-MM-dd")] as JArray;

                // Si hay datos en esa fecha procesamos
                if (asteroidsData != null)
                {
                    // Recorremos los asteroides
                    foreach (JObject asteroid in asteroidsData)
                    {
                        bool isPotentiallyHazardous = asteroid.Value<bool>("is_potentially_hazardous_asteroid");
                        if (isPotentiallyHazardous)
                        {
                            // Crear un objeto Asteroid con los datos de ese elemento
                            Asteroid asteroidObject = new Asteroid
                            {
                                Nombre = asteroid.Value<string>("name"),
                                Diametro = CalculateAverageDiameter(asteroid),
                                Velocidad = asteroid["close_approach_data"][0]["relative_velocity"]["kilometers_per_hour"].Value<double>(),
                                Fecha = asteroid["close_approach_data"][0]["close_approach_date"].Value<DateTime>(),
                                Planeta = asteroid["close_approach_data"][0]["orbiting_body"].Value<string>(),
                                IsPotentiallyHazardous = asteroid["is_potentially_hazardous_asteroid"].Value<bool>()
                            };

                            // Añadirlo a la lista
                            asteroidList.Add(asteroidObject);

                        }
                    }
                }
            }

            // Ordenar los asteroides y coger los 3 primeros
            asteroidList = asteroidList.OrderByDescending(a => a.Diametro).ToList();
            asteroidList = asteroidList.Take(3).ToList();

            return asteroidList;
        }


        private double CalculateAverageDiameter(JObject asteroid)
        {
            double minDiameter = asteroid["estimated_diameter"]["kilometers"]["estimated_diameter_min"].Value<double>();
            double maxDiameter = asteroid["estimated_diameter"]["kilometers"]["estimated_diameter_max"].Value<double>();

            // Calcular el tamaño medio (promedio)
            return (minDiameter + maxDiameter) / 2.0;
        }
    }

    public class NasaApiServiceMock : INasaApiService
    {
        public Task<List<Asteroid>> GetAsteroids(int days)
        {
            // Simular la obtención de datos de asteroides (datos ficticios para las pruebas)
            var asteroids = new List<Asteroid>
            {
                new Asteroid { Nombre = "Asteroid 1", Diametro = 14.5, Velocidad = 1000, Fecha = DateTime.Now.AddDays(1), Planeta = "Earth" },
                new Asteroid { Nombre = "Asteroid 2", Diametro = 12.7, Velocidad = 800, Fecha = DateTime.Now.AddDays(2), Planeta = "Earth" },
                new Asteroid { Nombre = "Asteroid 3", Diametro = 12.3, Velocidad = 1200, Fecha = DateTime.Now.AddDays(3), Planeta = "Earth" }
            };

            return Task.FromResult(asteroids);
        }
    }
}
