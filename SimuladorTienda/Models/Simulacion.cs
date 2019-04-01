using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimuladorTienda.Models
{
    public class Simulacion
    {
        tiendaEntities db = new tiendaEntities();
        public Venta simularVenta(int codPrueba, int codVenta, int numHora, int minProductos, int maxProductos)
        {
            Venta venta = new Venta();
            venta.codPrueba = codPrueba;
            venta.codVenta = codVenta;
            venta.numHora = numHora;
            List<DetalleVenta> detalle = new List<DetalleVenta>();
            List<tbproducto> productos = db.tbproducto.ToList();
            int productosAComprar = getRandom(minProductos, maxProductos); // random para calcular cuantos productos comprara
            for (int i = 0; i < productosAComprar; i++)
            {
                if (productos.Count > 0)
                {
                    // agregar detalle
                    int indexProducto = getRandom(0, productos.Count()); // random del producto a comprar
                    tbproducto producto = productos[indexProducto]; // obtiene un producto al azar de la lista de productos
                    DetalleVenta det = new DetalleVenta
                    {
                        codVenta = codVenta,
                        producto = producto,
                        cantidad = getRandom(minProductos, (maxProductos + 1))
                    };
                    detalle.Add(det);
                    productos.RemoveAll(t => t.idp == producto.idp); // elimina el producto para que no se vuelva a seleccionar
                }
            }
            venta.detalle = detalle;
            return venta;
        }

        public int getRandom(int min, int max)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            return rnd.Next(min, max);
        }
    }
}