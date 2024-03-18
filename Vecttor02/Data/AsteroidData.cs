using Vecttor02.Models;

namespace Vecttor02.Data
{
    public class AsteroidData
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Diametro { get; set; }
        public double Velocidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Planeta { get; set; }
        public bool IsPotentiallyHazardous { get; set; }
        public int TopAsteroidId { get; set; }
        public TopAsteroidData TopAsteroid { get; set; }
    }
}
