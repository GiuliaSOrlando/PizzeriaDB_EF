using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PizzeriaDB_EF.Models;

namespace PizzeriaDB_EF.Controllers
{
    public class UtentiController : Controller
    {
        private ModelDBContext DB = new ModelDBContext();

        public ActionResult Index()
        {
            return View(DB.Utenti.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Username,Email,Password,Ruolo")] Utenti utenti)
        {
            if (ModelState.IsValid)
            {
                utenti.Ruolo = "User";
                DB.Utenti.Add(utenti);
                DB.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View(utenti);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Username,Password")] Utenti utenti)
        {
            var user = DB.Utenti.FirstOrDefault(u => u.Username == utenti.Username && u.Password == utenti.Password);
            if(user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Username, false);
                return RedirectToAction("Index", "Home");
            }
            return View("Create");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit(int id)
        {
            Utenti utenti = DB.Utenti.Find(id);
            if (utenti == null)
            {
                return HttpNotFound();
            }
            return View(utenti);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Username,Email,Password,Ruolo")] Utenti utenti)
        {
            if (ModelState.IsValid)
            {
                DB.Entry(utenti).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(utenti);
        }

        public ActionResult Delete(int id)
        {
            Utenti utenti = DB.Utenti.Find(id);
            if (utenti == null)
            {
                return HttpNotFound();
            }
            return View(utenti);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ActualDelete(int id)
        {
            Utenti utenti = DB.Utenti.Find(id);
            DB.Utenti.Remove(utenti);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
