using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimuladorTienda.Models
{
    public class DetalleVenta
    {
        public int codVenta { get; set; }
        public tbproducto producto { get; set; }
        public int cantidad { get; set; }

        public decimal getUtilidad()
        {
            return (decimal)((producto.precio - producto.costo) * cantidad);
        }
    }
}