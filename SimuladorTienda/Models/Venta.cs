using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimuladorTienda.Models
{
    public class Venta
    {
        public int codPrueba { get; set; }
        public int codVenta { get; set; }
        public int numHora { get; set; }
        public List<DetalleVenta> detalle { get; set; }

        public double getUtilidad()
        {
            double utilidad = 0;
            foreach (DetalleVenta det in detalle)
            {
                utilidad += det.getUtilidad() * det.cantidad;
            }
            return utilidad;
        }
    }
}