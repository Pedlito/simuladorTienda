using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimuladorTienda.Models
{
    public class Variables
    {
        public int tasaLlegadaMinima { get; set; }
        public int tasaLlegadaMaxima { get; set; }
        public int cantComprasMinima { get; set; }
        public int cantComprasMaxima { get; set; }
        public int horasSimular { get; set; }
    }
}