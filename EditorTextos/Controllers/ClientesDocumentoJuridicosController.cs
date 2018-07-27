using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EditorTextos.Models;
using Newtonsoft.Json;

namespace EditorTextos.Controllers
{
    public class ClientesDocumentoJuridicosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClientesDocumentoJuridicos
        public ActionResult Index()
        {
            var clientesDocumentoJuridicos = db.ClientesDocumentoJuridicos.Include(c => c.Clientes).Include(c => c.Empresas).Include(c => c.PlantillaDocumentos);
            return View(clientesDocumentoJuridicos.ToList());
        }

        // GET: ClientesDocumentoJuridicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesDocumentoJuridicos clientesDocumentoJuridicos = db.ClientesDocumentoJuridicos.Find(id);
            if (clientesDocumentoJuridicos == null)
            {
                return HttpNotFound();
            }
            return View(clientesDocumentoJuridicos);
        }

        // GET: ClientesDocumentoJuridicos/Create
        public ActionResult Create()
        {
            ViewBag.ClientesId = new SelectList(db.Clientes, "Id", "PrimerNombre");
            ViewBag.EmpresasId = new SelectList(db.Empresas, "Id", "Empresa");
            ViewBag.PlantillaDocumentosId = new SelectList(db.PlantillaDocumentos, "Id", "Plantilla");

            var _dbclientes = db.Clientes
                .Include(d => d.Direcciones)
                .Include(c => c.CorreoElectronicos.Select(t => t.TipoCorreos))
                .Include(d => d.Documentos.Select(t => t.TipoDocumentos))
                .Where(x => x.Id == 1 || x.Id == 2).ToList();


            JsonSerializerSettings _settingsJson = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            };

            ViewBag._Json = JsonConvert.SerializeObject(db.Clientes.Find(1),_settingsJson);
            ViewBag._Json2 = JsonConvert.SerializeObject(_dbclientes, _settingsJson);

            return View();
        }

        // POST: ClientesDocumentoJuridicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DocumentoTexto,Resumen,ClientesId,PlantillaDocumentosId,EmpresasId,FechaCreacion,FechaActualizacion")] ClientesDocumentoJuridicos clientesDocumentoJuridicos)
        {
            if (ModelState.IsValid)
            {
                db.ClientesDocumentoJuridicos.Add(clientesDocumentoJuridicos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientesId = new SelectList(db.Clientes, "Id", "PrimerNombre", clientesDocumentoJuridicos.ClientesId);
            ViewBag.EmpresasId = new SelectList(db.Empresas, "Id", "Empresa", clientesDocumentoJuridicos.EmpresasId);
            ViewBag.PlantillaDocumentosId = new SelectList(db.PlantillaDocumentos, "Id", "Plantilla", clientesDocumentoJuridicos.PlantillaDocumentosId);
            return View(clientesDocumentoJuridicos);
        }

        // GET: ClientesDocumentoJuridicos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesDocumentoJuridicos clientesDocumentoJuridicos = db.ClientesDocumentoJuridicos.Find(id);
            if (clientesDocumentoJuridicos == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientesId = new SelectList(db.Clientes, "Id", "PrimerNombre", clientesDocumentoJuridicos.ClientesId);
            ViewBag.EmpresasId = new SelectList(db.Empresas, "Id", "Empresa", clientesDocumentoJuridicos.EmpresasId);
            ViewBag.PlantillaDocumentosId = new SelectList(db.PlantillaDocumentos, "Id", "Plantilla", clientesDocumentoJuridicos.PlantillaDocumentosId);
            return View(clientesDocumentoJuridicos);
        }

        // POST: ClientesDocumentoJuridicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DocumentoTexto,Resumen,ClientesId,PlantillaDocumentosId,EmpresasId,FechaCreacion,FechaActualizacion")] ClientesDocumentoJuridicos clientesDocumentoJuridicos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientesDocumentoJuridicos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientesId = new SelectList(db.Clientes, "Id", "PrimerNombre", clientesDocumentoJuridicos.ClientesId);
            ViewBag.EmpresasId = new SelectList(db.Empresas, "Id", "Empresa", clientesDocumentoJuridicos.EmpresasId);
            ViewBag.PlantillaDocumentosId = new SelectList(db.PlantillaDocumentos, "Id", "Plantilla", clientesDocumentoJuridicos.PlantillaDocumentosId);
            return View(clientesDocumentoJuridicos);
        }

        // GET: ClientesDocumentoJuridicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesDocumentoJuridicos clientesDocumentoJuridicos = db.ClientesDocumentoJuridicos.Find(id);
            if (clientesDocumentoJuridicos == null)
            {
                return HttpNotFound();
            }
            return View(clientesDocumentoJuridicos);
        }

        // POST: ClientesDocumentoJuridicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesDocumentoJuridicos clientesDocumentoJuridicos = db.ClientesDocumentoJuridicos.Find(id);
            db.ClientesDocumentoJuridicos.Remove(clientesDocumentoJuridicos);
            db.SaveChanges();
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
