using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimuladorTienda.Models
{
   
    public class Variables
    {
        public int tasaLlegada { get; set; }
        public int tasaServicio { get; set; }
        public int cant_min_comp { get; set; }
        public int cat_max_comp { get; set; }
        public int Cant_horas { get; set; }
    }
 
    public class DetalleCompra
    {
        public tbproducto producto { get; set; }
        public int cantidad { get; set; }
        public double utilidadDet() {
           return  (double)((producto.precio - producto.costo) * cantidad);
        }
    }

    public class Compra
    {
      int cantid =0;
       

      public List<DetalleCompra> detallesDeCompra;
        //constructor
        public Compra(int cant)
        {
            this.cantid = cant;       
            detallesDeCompra = new List<DetalleCompra>();
        }
        //base de datos
        private tiendaEntities db = new tiendaEntities();

        public void elegir(int op)
        {
            switch (op)
            {
                case 1:
                    detallesDeCompra.Add(new DetalleCompra() { producto = seleccionarBebida(), cantidad =cantid});
                    break;
                case 2:
                   detallesDeCompra.Add(new DetalleCompra() { producto = seleccionarComida(), cantidad = cantid});
                    break;
                case 3:
                    detallesDeCompra.Add(new DetalleCompra() { producto = seleccionarBebida(), cantidad = cantid});
                    detallesDeCompra.Add(new DetalleCompra() { producto = seleccionarComida(), cantidad = cantid});
                    break;
            }


        }

        public tbproducto seleccionarBebida()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            List<tbproducto> productos = (from p in db.tbproducto where p.idcate == 1 select p).ToList();
        
            int b = rnd.Next(0, productos.Count);
            var producto = productos.ElementAt(b);
            return producto;
        }

        public tbproducto seleccionarComida()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            List<tbproducto> productos = (from p in db.tbproducto where p.idcate == 2 select p).ToList();
            int c = rnd.Next(0, productos.Count);
            var producto = productos.ElementAt(c);
            return producto;
        }

        public double utilidadCompra() {
            double util = 0.0;
            foreach (var det in detallesDeCompra) {
                util +=det.utilidadDet();
            }
            return util;
        }

    }

    public class Rango
    {
        private Variables variable;
        private tiendaEntities db = new tiendaEntities();
        public List<tbgastos> listaGastos;
        public List<Compra> comprass;

        public Rango(Variables var)
        {
            variable = var;
            comprass = new List<Compra>();
            listaGastos = new List<tbgastos>();
        }

        public void rangoHoras()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
           // int o = rnd.Next(1, 4); // creates a number between 1 and 3


            int numPpersonas = variable.Cant_horas * variable.tasaLlegada;
            //  double utilidadTotal = 0.0;
     
                 listaGastos= db.tbgastos.ToList();
            //listaGastos g = new listaGastos();
          //  List<Gasto> gastos = g.getGastos();

            decimal totalGastos = 0.0m;
            foreach (tbgastos g in listaGastos)
            {
                totalGastos += g.monto;
            }

           

            for (int i = 1; i <= numPpersonas; i++)
            {
                Compra comp = new Compra(rnd.Next(variable.cant_min_comp,variable.cat_max_comp+1));
                comp.elegir(rnd.Next(1, 4));
                comprass.Add(comp);

               // foreach (DetalleCompra item in comp.detallesDeCompra)
               // {
                    //comprass.Add(item);
                   // utilidadTotal += item.producto.getUtilidad()*item.cantidad;
                    //Console.WriteLine("Nombre = " + item.producto.nombre + " | Utilidad" + item.producto.getUtilidad());
               // }

                //Console.WriteLine("----------");
            }

           // Console.WriteLine("Gastos totales" + totalGastos);
          //  double gastosFraccionado = ((totalGastos / 30) / 12) * variable.Cant_horas;

           // Console.WriteLine("Utilidad total = " + utilidadTotal + " gastos = " + gastosFraccionado);
           // Console.WriteLine("Utilidad neta: -> " + (utilidadTotal - gastosFraccionado));
            //tasa de llegada = cantidad de presonas por hora
            //tasa de servicio
            //cantidad maxima y minima de productos que puede comprar una persona
           // return comprass;
        }
    }
}