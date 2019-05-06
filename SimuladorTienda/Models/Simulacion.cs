using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimuladorTienda.Models
{
    public class Simulacion
    {
        tiendaEntities db = new tiendaEntities();
        Random rnd = new Random(DateTime.Now.Millisecond);

        public Venta SimularVenta(int codPrueba, int codVenta, int numHora, int minProductos, int maxProductos)
        {
            Venta venta = new Venta();
            venta.codPrueba = codPrueba;
            venta.codVenta = codVenta;
            venta.numHora = numHora;
            List<DetalleVenta> detalle = new List<DetalleVenta>();
            List<tbproducto> productos = db.tbproducto.ToList();
            int productosAComprar = getRandom(minProductos, maxProductos + 1); // random para calcular cuantos productos comprara
            for (int i = 0; i < productosAComprar; i++)
            {
                if (productos.Count > 0)
                {
                    // agregar detalle
                    int indexProducto = getRandom(0, productos.Count()); // random del producto a comprar, obtiene index, no codigo
                    int cantidadProductos = (getRandom(0, 100) < 90) ? 1 : 2; // unidades del producto a comprar
                    DetalleVenta det = new DetalleVenta
                    {
                        codVenta = codVenta,
                        producto = productos[indexProducto],
                        cantidad = cantidadProductos
                    };
                    detalle.Add(det);
                    productos.RemoveAll(t => t.idp == det.producto.idp); // elimina el producto para que no se vuelva a seleccionar
                }
            }
            venta.detalle = detalle;
            return venta;
        }

        public int getRandom(int min, int max)
        {
            rnd.Next();
            return rnd.Next(min, max);
        }

        public decimal getGastoHora()
        {
            decimal gastoTotal = db.tbgastos.Sum(t => t.monto);
            return (gastoTotal / 360);
        }
    }
}