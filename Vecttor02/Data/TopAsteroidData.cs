using Vecttor02.Models;

namespace Vecttor02.Data
{
    public class TopAsteroidData
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<AsteroidData> Asteroids { get; set; }
    }
}
