using Travelers.Context;
using Travelers.Infrastructure.Concrete;
using Travelers.ModalLogin.Models;
using Travelers.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Travelers.Controllers
{
    public class CityController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        

        public ActionResult Detail(int id)
        {
            Sehir sehir = db.Sehirler.FirstOrDefault(x => x.Id == id);

            return View(sehir);
        }

        [HttpPost]
        public ActionResult AddComment(int id, string commenttext)
        {
           

            if (ModelState.IsValid)
            {
                Sehir sehir = db.Sehirler.FirstOrDefault(x => x.Id == id);
                LoginUser currentUser = Session["login"] as LoginUser;

                Yorum yeniYorum = new Yorum()
                {
                    LoginUserId = currentUser.Id,
                    SehirId = sehir.Id,
                    Tarih = DateTime.Now,
                    YorumMetni = commenttext
                };

                db.Yorumlar.Add(yeniYorum);
                db.SaveChanges();
            }

            return Redirect($"/City/Detail/{id}#comments");
        }

        // GET: Cities
        public ActionResult Index()
        {
            
            return View(db.Sehirler.ToList());
        }

        // GET: Cities/Create
        public ActionResult Create()
        {
           

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Sehir sehir, HttpPostedFileBase cityImage)
        {
            

            sehir.OlusturmaTarihi = DateTime.Now;
            sehir.GuncellenmeTarihi = DateTime.Now;
            sehir.EkleyenUyeAdi = SessionHelper.CurrentUser.Username;

            if (cityImage != null)
            {
                string imageName = Guid.NewGuid().ToString() + ".jpg";
                cityImage.SaveAs(Server.MapPath("~/images/medias/" + imageName));

                sehir.Resmi = imageName;
            }
            else
            {
                sehir.Resmi = "sehir4.jpg";
            }

            if (ModelState.IsValid)
            {
                db.Sehirler.Add(sehir);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(sehir);
        }

        // GET: Cities/Edit/5
        public ActionResult Edit(int? id)
        {
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sehir sehir = db.Sehirler.Find(id);
            if (sehir == null)
            {
                return HttpNotFound();
            }
            return View(sehir);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Sehir model, HttpPostedFileBase cityImage)
        {
            
            Sehir sehir = db.Sehirler.Find(model.Id);
            sehir.Adi = model.Adi;
            sehir.DigerBilgiler = model.DigerBilgiler;
            sehir.GezilecekYer = model.GezilecekYer;
            sehir.Slogan = model.Slogan;
            sehir.Tarihi = model.Tarihi;
            sehir.Yemekler = model.Yemekler;

            sehir.GuncellenmeTarihi = DateTime.Now;
            sehir.EkleyenUyeAdi = SessionHelper.CurrentUser.Username;

            if (cityImage != null)
            {
                string imageName = Guid.NewGuid().ToString() + ".jpg";
                cityImage.SaveAs(Server.MapPath("~/images/medias/" + imageName));

                sehir.Resmi = imageName;
            }

            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sehir);
        }

        // GET: Cities/Delete/5
        public ActionResult Delete(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sehir sehir = db.Sehirler.Find(id);
            if (sehir == null)
            {
                return HttpNotFound();
            }
            return View(sehir);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           

            Sehir sehir = db.Sehirler.Find(id);
            db.Sehirler.Remove(sehir);
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