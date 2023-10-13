using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PizzeriaDB_EF.Models;

namespace PizzeriaDB_EF.Controllers
{
    
    public class ProdottiController : Controller
    {
        private ModelDBContext DB = new ModelDBContext();

        public ActionResult Index()
        {
            ViewBag.IngredientiExtra = DB.IngredientiExtra.ToList();
            return View(DB.Prodotti.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create([Bind(Include = "Nome,Foto,Prezzo,TempoConsegna,Ingredienti")] Prodotti prodotti, HttpPostedFileBase foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    string fileName = Path.GetFileName(foto.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/img/uploads"), fileName);

                    foto.SaveAs(filePath);

                    prodotti.Foto = fileName;

                    DB.Prodotti.Add(prodotti);
                    DB.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(prodotti);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Prodotti prodotti = DB.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "IdProdotto, Nome, Foto, Prezzo, TempoConsegna, Ingredienti")] Prodotti prodotti, HttpPostedFileBase foto)
        {
            if (ModelState.IsValid)
            {
                Prodotti prodottoSalvato = DB.Prodotti.Find(prodotti.IdProdotto);

                if (prodottoSalvato != null)
                {
                    prodottoSalvato.Nome = prodotti.Nome;
                    prodottoSalvato.Prezzo = prodotti.Prezzo;
                    prodottoSalvato.TempoConsegna = prodotti.TempoConsegna;
                    prodottoSalvato.Ingredienti = prodotti.Ingredienti;

                    if (foto != null)
                    {
                        string fileName = Path.GetFileName(foto.FileName);
                        string filePath = Path.Combine(Server.MapPath("~/Content/img/uploads"), fileName);
                        foto.SaveAs(filePath);

                        prodottoSalvato.Foto = fileName;
                    }

                    DB.Entry(prodottoSalvato).State = EntityState.Modified;
                    DB.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(prodotti);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Prodotti prodotti = DB.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult ActualDelete(int id)
        {
            Prodotti prodotti = DB.Prodotti.Find(id);
            DB.Prodotti.Remove(prodotti);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
