using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMS.BOL;

namespace SMS.UI.Controllers
{
    public class PaymentDetailsController : Controller
    {
        private SMSDBEntities db = new SMSDBEntities();

        // GET: PaymentDetails
        public ActionResult Index()
        {
            var paymentDetails = db.PaymentDetails.Include(p => p.Grade).Include(p => p.StudentInfo);
            return View(paymentDetails.ToList());
        }

        // GET: PaymentDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentDetail paymentDetail = db.PaymentDetails.Find(id);
            if (paymentDetail == null)
            {
                return HttpNotFound();
            }
            return View(paymentDetail);
        }

        // GET: PaymentDetails/Create
        public ActionResult Create()
        {
            ViewBag.GradeId = new SelectList(db.Grades, "Id", "ClassName");
            ViewBag.StudentId = new SelectList(db.StudentInfoes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaymentDetail paymentDetail)
        {
            if (ModelState.IsValid)
            {
                paymentDetail.PaymentDate = DateTime.UtcNow;
                db.PaymentDetails.Add(paymentDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GradeId = new SelectList(db.Grades, "Id", "ClassName", paymentDetail.GradeId);
            ViewBag.StudentId = new SelectList(db.StudentInfoes, "Id", "Name", paymentDetail.StudentId);
            return View(paymentDetail);
        }

        // GET: PaymentDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentDetail paymentDetail = db.PaymentDetails.Find(id);
            if (paymentDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.GradeId = new SelectList(db.Grades, "Id", "ClassName", paymentDetail.GradeId);
            ViewBag.StudentId = new SelectList(db.StudentInfoes, "Id", "Name", paymentDetail.StudentId);
            return View(paymentDetail);
        }

        // POST: PaymentDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,GradeId,TuitionFee,PaymentDate,IsPaid,ForMonth")] PaymentDetail paymentDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GradeId = new SelectList(db.Grades, "Id", "ClassName", paymentDetail.GradeId);
            ViewBag.StudentId = new SelectList(db.StudentInfoes, "Id", "Name", paymentDetail.StudentId);
            return View(paymentDetail);
        }

        // GET: PaymentDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentDetail paymentDetail = db.PaymentDetails.Find(id);
            if (paymentDetail == null)
            {
                return HttpNotFound();
            }
            return View(paymentDetail);
        }

        // POST: PaymentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentDetail paymentDetail = db.PaymentDetails.Find(id);
            db.PaymentDetails.Remove(paymentDetail);
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
