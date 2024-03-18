using System;
using System.Collections.Generic;

namespace Vecttor02.Models
{
    public class Asteroid
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Diametro { get; set; }
        public double Velocidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Planeta { get; set; }
        public bool IsPotentiallyHazardous { get; set; }
    }
}
