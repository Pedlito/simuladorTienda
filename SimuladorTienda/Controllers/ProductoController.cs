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
    public class ProductoController : Controller
    {
        private tiendaEntities db = new tiendaEntities();

        // GET: Producto
        public async Task<ActionResult> Index()
        {
            var tbproducto = db.tbproducto.Include(t => t.tbcategoria);
            return View(await tbproducto.ToListAsync());
        }

        // GET: Producto/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbproducto tbproducto = await db.tbproducto.FindAsync(id);
            if (tbproducto == null)
            {
                return HttpNotFound();
            }
            return View(tbproducto);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            ViewBag.idcate = new SelectList(db.tbcategoria, "idcat", "nombre");
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "idp,nombre,costo,precio,preferencia,idcate")] tbproducto tbproducto)
        {
            

            if (ModelState.IsValid)
            {
                db.tbproducto.Add(tbproducto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.idcate = new SelectList(db.tbcategoria, "idcat", "nombre", tbproducto.idcate);
            return View(tbproducto);
        }

        // GET: Producto/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbproducto tbproducto = await db.tbproducto.FindAsync(id);
            if (tbproducto == null)
            {
                return HttpNotFound();
            }
            ViewBag.idcate = new SelectList(db.tbcategoria, "idcat", "nombre", tbproducto.idcate);
            return View(tbproducto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "idp,nombre,costo,precio,preferencia,idcate")] tbproducto tbproducto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbproducto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.idcate = new SelectList(db.tbcategoria, "idcat", "nombre", tbproducto.idcate);
            return View(tbproducto);
        }

        // GET: Producto/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbproducto tbproducto = await db.tbproducto.FindAsync(id);
            if (tbproducto == null)
            {
                return HttpNotFound();
            }
            return View(tbproducto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tbproducto tbproducto = await db.tbproducto.FindAsync(id);
            db.tbproducto.Remove(tbproducto);
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
