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
        // GET: Simulacion
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Simulacion(Variables variables)
        {
            ventas = new List<Venta>();
            int codVenta = 0;
            for (int codPrueba = 1; codPrueba <= 3; codPrueba++)
            {
                Simulacion sm = new Simulacion();
                double utilidad = 0;
                for (int i = 0; i < variables.horasSimular; i++)
                {
                    int tasa = sm.getRandom(variables.tasaLlegadaMinima, variables.tasaLlegadaMaxima);
                    for (int j = 0; j < tasa; j++)
                    {
                        Venta venta = sm.simularVenta(codPrueba, codVenta, (i + 1), variables.cantComprasMinima, variables.cantComprasMaxima);
                        ventas.Add(venta);
                        codVenta++;
                        foreach (DetalleVenta item in venta.detalle)
                        {
                            utilidad += item.getUtilidad();
                        }
                    }
                }
            }
            return RedirectToAction("Resultados");
        }

        public ActionResult Resultados()
        {
            List<HoraVenta> horasVentas = new List<HoraVenta>();
            for (int codPrueba = 1; codPrueba <= 3; codPrueba++)
            {
                List<Venta> ventasPrueba = ventas.Where(t => t.codPrueba == codPrueba).ToList();
                List<int> horas = (from t in ventasPrueba
                                   group t by t.numHora into h
                                   select h.Key).ToList();
                foreach (int hora in horas)
                {
                    HoraVenta ventaHora = new HoraVenta();
                    ventaHora.codPrueba = codPrueba;
                    ventaHora.numhora = hora;
                    ventaHora.numVentas = ventasPrueba.Where(t => t.numHora == hora).Count();
                    double utilidad = 0;
                    foreach (Venta venta in ventasPrueba.Where(t => t.numHora == hora))
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
    }
}