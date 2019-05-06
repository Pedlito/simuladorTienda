using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimuladorTienda.Models;

namespace SimuladorTienda.Controllers
{
    public class SimulacionController : Controller
    {
        private static List<Venta> ventas = new List<Venta>();
        private static decimal gastoHora = 0;
        private static int numHoras = 0;
        private static int servicios = 0;
        // GET: Simulacion
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Simulacion(Variables variables)
        {
            // Se realiza la simulación de las ventas
            numHoras = variables.horasSimular;
            Simulacion sm = new Simulacion();
            ventas = new List<Venta>();
            int codVenta = 0;
            for (int codPrueba = 1; codPrueba <= 3; codPrueba++)
            {
                decimal utilidad = 0;
                for (int i = 0; i < variables.horasSimular; i++)
                {
                    int tasa = sm.getRandom(variables.tasaLlegadaMinima, variables.tasaLlegadaMaxima + 1);
                    for (int j = 0; j < tasa; j++)
                    {
                        Venta venta = sm.SimularVenta(codPrueba, codVenta, (i + 1), variables.cantComprasMinima, variables.cantComprasMaxima);
                        ventas.Add(venta);
                        codVenta++;
                        foreach (DetalleVenta item in venta.detalle)
                        {
                            utilidad += item.getUtilidad();
                        }
                    }
                }

            }

            servicios = variables.tasaLlegadaMaxima;
            // Se realiza el calculo de los gastos por hora
            gastoHora = sm.getGastoHora();
            return RedirectToAction("Resultados");
        }

        public ActionResult Resultados()
        {
            List<HoraVenta> horasVentas = new List<HoraVenta>();
            for (int codPrueba = 1; codPrueba <= 3; codPrueba++)
            {
                for (int hora = 1; hora <= numHoras; hora++)
                {
                    HoraVenta ventaHora = new HoraVenta();
                    ventaHora.codPrueba = codPrueba;
                    ventaHora.numhora = hora;
                    ventaHora.numVentas = ventas.Where(t => t.codPrueba == codPrueba && t.numHora == hora).Count();
                    decimal utilidad = 0;
                    foreach (Venta venta in ventas.Where(t => t.codPrueba == codPrueba && t.numHora == hora))
                    {
                        foreach (DetalleVenta det in venta.detalle)
                        {
                            utilidad += det.getUtilidad() * det.cantidad;
                        }
                    }
                    ventaHora.utilidad = utilidad;
                    horasVentas.Add(ventaHora);
                }
            }
            ViewBag.gastoTotal = decimal.Round(gastoHora * numHoras, 2);

            //DatosCola

            List<DatosCola> datosColas = new List<DatosCola>();
            for (int i = 1; i < 4; i++)
            {
                DatosCola cola = new DatosCola();
                cola.codPrueba = i;
                cola.tasaLlegada = (int)Math.Round(horasVentas.Where(t => t.codPrueba == i).Average(t => t.numVentas));
                cola.tasaServicio = servicios;
                cola.utilizacion = (float)cola.tasaLlegada / (float)cola.tasaServicio;
                cola.lq = calcLq(cola.tasaLlegada, cola.tasaServicio);
                cola.ls = calcLs(cola.tasaLlegada, cola.tasaServicio, cola.lq);
                cola.wq = calcWq(cola.tasaLlegada, cola.lq);
                cola.ws = calcWs(cola.tasaServicio, cola.wq);
                cola.probabilidades = new List<Probabilidad>();
                for (int x = 0; x <= cola.tasaServicio; x++)
                {
                    Probabilidad p = new Probabilidad { x = x, p = calcProbabilidad(x, cola.utilizacion) };
                    cola.probabilidades.Add(p);
                }
                datosColas.Add(cola);
            }

            // Paso listas para graficas
            ViewBag.datosColas = datosColas;
            
            // Creo listas para mandarlas
           
            return View(horasVentas);
        }

        public ActionResult ResultadosHora(int codPrueba, int numHora)
        {
            ViewBag.codPrueba = codPrueba;
            ViewBag.numHora = numHora;
            return View(ventas.Where(t => t.codPrueba == codPrueba && t.numHora == numHora));
        }

        public ActionResult Hora(int numHora)
        {
            return View(ventas.Where(t => t.numHora == numHora));
        }

        public PartialViewResult Detalle(int codVenta)
        {
            Venta venta = ventas.Where(t => t.codVenta == codVenta).SingleOrDefault();
            return PartialView("_Detalle", venta);
        }

        private float calcProbabilidad(int x, float utilizacion)
        {
            float retorno = 0, potencia = 0, resta = 0;
            potencia = (float)Math.Pow(utilizacion, x);
            resta = 1 - utilizacion;
            retorno = potencia * resta;
            return (float)retorno;
        }

        private float calcLq(int llegada, int servicio)
        {
            float retorno = 0, numerador = 0, denominador = 0;
            numerador = (float)Math.Pow((float)llegada, 2);
            denominador = servicios * ((float)servicio - (float)llegada);
            retorno = numerador / denominador;
            return retorno;
        }

        private float calcLs(int llegada, int servicio, float lq)
        {
            return lq + ((float)llegada / (float)servicio);
        }

        private float calcWq(int llegada, float lq)
        {
            return lq / (float)llegada;
        }

        private float calcWs(int servicio, float wq)
        {
            return wq + (1 / (float)servicio);
        }
    }
}