using PizzeriaDB_EF.Models;
using PizzeriaDB_EF.Models.PizzeriaDB_EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzeriaDB_EF.Controllers
{
    [Authorize]
    public class OrdiniController : Controller
    {
        private ModelDBContext DB = new ModelDBContext();

        private List<SelectListItem> IngredientiExtraViewBag
        {
            get
            {
                List<IngredientiExtra> ingredientiExtraDisponibili = DB.IngredientiExtra.ToList();
                List<SelectListItem> ingredientSelections = new List<SelectListItem>();
                foreach (IngredientiExtra i in ingredientiExtraDisponibili)
                {
                    ingredientSelections.Add(new SelectListItem { Text = i.Nome, Value = i.IdIngredientiExtra.ToString() });
                }
                return ingredientSelections;
            }
        }

        private void InitializeAllOrders()
        {
            List<Ordini> listaOrdini = DB.Ordini.ToList();
            ViewBag.AllOrders = listaOrdini;
        }
        public ActionResult AddToCart(int id, int quantita)
        {
            string username = User.Identity.Name;
            Utenti utente = DB.Utenti.FirstOrDefault(u => u.Username == username);

            if (utente != null)
            {
                Prodotti prodottoSelezionato = DB.Prodotti.Find(id);

                if (prodottoSelezionato != null)
                {
                    List<DettagliOrdine> carrelloTemporaneo = Session["TempCart"] as List<DettagliOrdine>;

                    if (carrelloTemporaneo == null)
                    {
                        carrelloTemporaneo = new List<DettagliOrdine>();
                        Session["TempCart"] = carrelloTemporaneo;
                    }

                    DettagliOrdine dettaglio = new DettagliOrdine
                    {
                        IdProdotto = prodottoSelezionato.IdProdotto,
                        Quantita = quantita,
                        PrezzoTotale = prodottoSelezionato.Prezzo * quantita,
                    };

                    carrelloTemporaneo.Add(dettaglio);

                    return RedirectToAction("Index", "Prodotti");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ViewCart()
        {
            List<DettagliOrdine> carrelloTemporaneo = Session["TempCart"] as List<DettagliOrdine>;
            decimal? totale = 0;

            if (carrelloTemporaneo != null)
            {
                var carrelloModel = carrelloTemporaneo
                    .Join(DB.Prodotti, dettaglio => dettaglio.IdProdotto, product => product.IdProdotto,
                        (dettaglio, product) => new Carrello
                        {
                            DettaglioOrdine = dettaglio,
                            Nome = product.Nome
                        })
                    .ToList();

                totale = carrelloModel.Sum(item => item.DettaglioOrdine.PrezzoTotale);

                ViewBag.Totale = totale;
                return View(carrelloModel);
            }
            ViewBag.Totale = totale;
            return View(carrelloTemporaneo);
        }
        public ActionResult ConfirmOrder()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<DettagliOrdine> carrelloTemporaneo = Session["TempCart"] as List<DettagliOrdine>;

                if (carrelloTemporaneo != null && carrelloTemporaneo.Count > 0)
                {
                    var nomiProdotti = carrelloTemporaneo
                        .Select(dettaglio => DB.Prodotti.Find(dettaglio.IdProdotto).Nome)
                        .ToList();

                    decimal? totale = carrelloTemporaneo.Sum(dettaglio => dettaglio.PrezzoTotale);
                    ViewBag.Totale = totale;
                    ViewBag.NomiProdotti = nomiProdotti;
                    ViewBag.IngredientiExtra = IngredientiExtraViewBag;
                    return View(carrelloTemporaneo);
                }
            }

            return RedirectToAction("Index", "Prodotti");
        }

        [HttpPost, ActionName("ConfirmOrder")]
        public ActionResult ConfirmOrderPost(string indirizzoSpedizione, FormCollection ingredientiCheckBox)
        {
            string username = User.Identity.Name;
            Utenti utente = DB.Utenti.FirstOrDefault(u => u.Username == username);

            if (utente != null)
            {
                List<DettagliOrdine> carrelloTemporaneo = Session["TempCart"] as List<DettagliOrdine>;

                if (carrelloTemporaneo != null && carrelloTemporaneo.Count > 0)
                {
                    Ordini nuovoOrdine = new Ordini
                    {
                        IdUtente = utente.IdUtente,
                        DataOrdine = DateTime.Now,
                        StatoOrdine = "In elaborazione",
                    };

                    List<int> indici = new List<int>();

                    var indiciEstratti = ingredientiCheckBox.AllKeys.Where(key => key.StartsWith("ingredientiExtra_"))
                        .Select(key => key.Replace("ingredientiExtra_", ""));

                    foreach (var i in indiciEstratti)
                    {
                        indici.Add(Convert.ToInt32(i));
                    }

                    foreach (var dettaglio in carrelloTemporaneo)
                    {
                        var nuovoDettaglio = new DettagliOrdine
                        {
                            IdProdotto = dettaglio.IdProdotto,
                            Quantita = dettaglio.Quantita,
                            PrezzoTotale = dettaglio.PrezzoTotale,
                            IndirizzoSpedizione = indirizzoSpedizione,
                            NoteAggiuntive = dettaglio.NoteAggiuntive,
                        };

                        foreach (var indice in indici)
                        {
                            var ingredienteExtra = DB.IngredientiExtra.FirstOrDefault(ie => ie.IdIngredientiExtra == indice);

                            if (ingredienteExtra != null)
                            {
                                var nuovoDettaglioIngredienti = new DettaglioIngredienti
                                {
                                    IdDettaglioOrdine = dettaglio.IdDettaglio,
                                    IdIngredienteExtra = ingredienteExtra.IdIngredientiExtra
                                };

                                if (nuovoDettaglio.DettaglioIngredienti == null)
                                {
                                    nuovoDettaglio.DettaglioIngredienti = new List<DettaglioIngredienti>();
                                }

                                nuovoDettaglio.DettaglioIngredienti.Add(nuovoDettaglioIngredienti);
                            }
                        }

                        nuovoOrdine.DettagliOrdine.Add(nuovoDettaglio);
                    }

                    DB.Ordini.Add(nuovoOrdine);
                    DB.SaveChanges();

                    Session["TempCart"] = null;
                }
            }

            return RedirectToAction("Index", "Prodotti");

        }

        [Authorize(Roles = "Admin")]
        public ActionResult AllOrders(string statoOrdine = "In elaborazione")
        {
            var ordini = DB.Ordini.ToList();

            if (statoOrdine == "In elaborazione")
            {
                ordini = ordini.Where(o => o.StatoOrdine == "In elaborazione").ToList();
            }
            else if (statoOrdine == "Evaso")
            {
                ordini = ordini.Where(o => o.StatoOrdine == "Evaso").ToList();
            }

            ViewBag.StatoOrdine = statoOrdine;
            return View(ordini);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult OnGoingOrders()
        {
            var ordiniInCorso = DB.Ordini.Where(o => o.StatoOrdine == "In elaborazione").ToList();
            return View(ordiniInCorso);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ConcludedOrders(string statoOrdine = "Evaso")
        {
            var ordini = DB.Ordini.ToList();

            if (statoOrdine == "Evaso")
            {
                ordini = ordini.Where(o => o.StatoOrdine == "Evaso").ToList();
            }
            else if (statoOrdine == "In elaborazione")
            {
                ordini = ordini.Where(o => o.StatoOrdine == "In elaborazione").ToList();
            }

            ViewBag.StatoOrdine = statoOrdine;
            return View(ordini);
        }

        [HttpPost]
        public ActionResult ToggleOrderStatus(int idOrdine, string statoOrdine)
        {
            var ordine = DB.Ordini.FirstOrDefault(o => o.IdOrdine == idOrdine);

            if (ordine != null)
            {
                if (statoOrdine == "Evaso")
                {
                    ordine.StatoOrdine = "In elaborazione";
                }
                else if (statoOrdine == "In elaborazione")
                {
                    ordine.StatoOrdine = "Evaso";
                }
                DB.SaveChanges();
            }

            if (statoOrdine == "In elaborazione")
            {
                return RedirectToAction("OnGoingOrders");
            }
            else if (statoOrdine == "Evaso")
            {
                return RedirectToAction("ConcludedOrders");
            }

            return RedirectToAction("AllOrders");
        }

        public ActionResult Query()
        {
            return View();
        }

        public JsonResult TotalOrdersForDay()
        {
            InitializeAllOrders();
            DateTime dataOdierna = DateTime.Now.Date;
            List<Ordini> ordini = ViewBag.AllOrders;
            if (ordini != null)
            {
                int totaleOrdini = ordini
                    .Where(o => o.DataOrdine.HasValue && o.DataOrdine.Value.Date == dataOdierna.Date && o.StatoOrdine == "Evaso")
                    .Count();
                return Json(new { TotalOrders = totaleOrdini }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { TotalOrders = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TotalRevenueForDay(DateTime selectedDate)
        {
            InitializeAllOrders();
            DateTime dataOdierna = DateTime.Now.Date;
            List<Ordini> ordini = ViewBag.AllOrders;

            var totaleOrdini = ordini
                .Where(o => o.DataOrdine.HasValue && o.DataOrdine.Value.Date == selectedDate.Date &&
                            o.StatoOrdine == "Evaso")
                .Select(o => o.IdOrdine)
                .ToList();

            decimal? incassoTotale = DB.DettagliOrdine
                    .Where(d => totaleOrdini.Contains(d.IdOrdine))
                    .Sum(d => (decimal?)(d.PrezzoTotale));

                if (incassoTotale.HasValue)
                {
                    return Json(new { TotalRevenue = incassoTotale.Value }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { TotalRevenue = 0 }, JsonRequestBehavior.AllowGet);
                }
            }

        }

    }

