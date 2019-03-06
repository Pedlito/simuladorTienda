using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SimuladorTienda.Models;

namespace SimuladorTienda.Controllers
{
    public class GastosController : Controller
    {
        private tiendaEntities db = new tiendaEntities();

        // GET: Gastos
        public async Task<ActionResult> Index()
        {
            return View(await db.tbgastos.ToListAsync());
        }

        // GET: Gastos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbgastos tbgastos = await db.tbgastos.FindAsync(id);
            if (tbgastos == null)
            {
                return HttpNotFound();
            }
            return View(tbgastos);
        }

        // GET: Gastos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gastos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,nombre,monto")] tbgastos tbgastos)
        {
            if (ModelState.IsValid)
            {
                db.tbgastos.Add(tbgastos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tbgastos);
        }

        // GET: Gastos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbgastos tbgastos = await db.tbgastos.FindAsync(id);
            if (tbgastos == null)
            {
                return HttpNotFound();
            }
            return View(tbgastos);
        }

        // POST: Gastos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,nombre,monto")] tbgastos tbgastos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbgastos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbgastos);
        }

        // GET: Gastos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbgastos tbgastos = await db.tbgastos.FindAsync(id);
            if (tbgastos == null)
            {
                return HttpNotFound();
            }
            return View(tbgastos);
        }

        // POST: Gastos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tbgastos tbgastos = await db.tbgastos.FindAsync(id);
            db.tbgastos.Remove(tbgastos);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
