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
    public class IngredientiExtraController : Controller
    {
        private ModelDBContext DB = new ModelDBContext();

        public ActionResult Index()
        {
            return View(DB.IngredientiExtra.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "IdIngredientiExtra,Nome,Prezzo")] IngredientiExtra ingredientiExtra)
        {
            if (ModelState.IsValid)
            {
                DB.IngredientiExtra.Add(ingredientiExtra);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ingredientiExtra);
        }

        public ActionResult Edit(int id)
        {
            IngredientiExtra ingredientiExtra = DB.IngredientiExtra.Find(id);
            if (ingredientiExtra == null)
            {
                return HttpNotFound();
            }
            return View(ingredientiExtra);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "IdIngredientiExtra,Nome,Prezzo")] IngredientiExtra ingredientiExtra)
        {
            if (ModelState.IsValid)
            {
                DB.Entry(ingredientiExtra).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ingredientiExtra);
        }

        public ActionResult Delete(int id)
        {
            IngredientiExtra ingredientiExtra = DB.IngredientiExtra.Find(id);
            if (ingredientiExtra == null)
            {
                return HttpNotFound();
            }
            return View(ingredientiExtra);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ActualDelete(int id)
        {
            IngredientiExtra ingredientiExtra = DB.IngredientiExtra.Find(id);
            DB.IngredientiExtra.Remove(ingredientiExtra);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
