using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimuladorTienda.Models
{
    public class DatosCola
    {
        public int tasaLlegada { get; set; }
        public int tasaServicio { get; set; }
        public int codPrueba { get; set; }
        public float utilizacion { get; set; }
        public float lq { get; set; }
        public float ls { get; set; }
        public float wq { get; set; }
        public float ws { get; set; }
        public List<Probabilidad> probabilidades { get; set; }
    }
}