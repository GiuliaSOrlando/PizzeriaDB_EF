using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PizzeriaDB_EF.Models;

namespace PizzeriaDB_EF.Controllers
{
    public class DettagliOrdineController : Controller
    {
        private ModelDBContext DB = new ModelDBContext();
        public ActionResult Index()
        {
            return View(DB.DettagliOrdine.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "IdOrdine,IdProdotto,Quantita,PrezzoTotale,IndirizzoSpedizione,NoteAggiuntive")] DettagliOrdine dettagliOrdine)
        {
            if (ModelState.IsValid)
            {
                DB.DettagliOrdine.Add(dettagliOrdine);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dettagliOrdine);
        }

        public ActionResult Edit(int id)
        {
            DettagliOrdine dettagliOrdine = DB.DettagliOrdine.Find(id);
            if (dettagliOrdine == null)
            {
                return HttpNotFound();
            }
            return View(dettagliOrdine);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "IdOrdine,IdProdotto,Quantita,PrezzoTotale,IndirizzoSpedizione,NoteAggiuntive")] DettagliOrdine dettagliOrdine)
        {
            if (ModelState.IsValid)
            {
                DB.Entry(dettagliOrdine).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dettagliOrdine);
        }

        public ActionResult Delete(int id)
        {
            DettagliOrdine dettagliOrdine = DB.DettagliOrdine.Find(id);
            if (dettagliOrdine == null)
            {
                return HttpNotFound();
            }
            return View(dettagliOrdine);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ActualDelete(int id)
        {
            DettagliOrdine dettagliOrdine = DB.DettagliOrdine.Find(id);
            DB.DettagliOrdine.Remove(dettagliOrdine);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
