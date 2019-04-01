using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimuladorTienda.Models;

namespace SimuladorTienda.Controllers
{
    public class HomeController : Controller
    {
     
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Variable() {

            return View();
        }

        [HttpPost]
        public ActionResult Variab(Variables variable)
        {
            Rango r = new Rango(variable);
            r.rangoHoras();
            double utilidadp = 0;
            foreach (var comp in r.comprass) {
                utilidadp += comp.utilidadCompra();
            }
            ViewBag.utilidadCompras = utilidadp;
          
            ViewBag.registros=r.comprass;
           double gastosTotales=0;
            foreach (var l in r.listaGastos) {
                gastosTotales += (double)l.monto;
            }

            ViewBag.gastosTatales = gastosTotales;
            double gastosPh = ((gastosTotales / 30) / 12);
            ViewBag.gastosPh = gastosPh;
            double gstor = ((gastosPh) * variable.horasSimular);
            ViewBag.gastoRestar = gstor;
            double UtilidadTotal = utilidadp - gstor;
            ViewBag.UtilidadTotal = UtilidadTotal;

            return View();
        }
    }
    

}