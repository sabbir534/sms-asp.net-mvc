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
    public class AttendancesController : Controller
    {
        private SMSDBEntities db = new SMSDBEntities();

        // GET: Attendances
        public ActionResult Index()
        {
            var attendances = db.Attendances.Include(a => a.Grade).Include(a => a.StudentInfo);
            return View(attendances.ToList());
        }

        public ActionResult GetClass()
        {
            var grade = db.Grades.ToList();
            return View(grade);
        }
        public ActionResult CreateAttendance(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var listOfStudent = db.StudentInfoes.Where(g => g.GradeId == id).ToList();
            var notAttendeeList =
                listOfStudent.SelectMany(s => s.Attendances.Where(a => a.AttendanceDate != DateTime.Today)).ToList();
                
            return View(listOfStudent);
        }

        public ActionResult Attend(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = db.StudentInfoes.First(s => s.Id == id);
            Attendance attend = new Attendance
            {
                StudentId = student.Id,
                GradeId = student.Grade.Id,
                IsPresent = true,
                IsCounted = true,
                AttendanceDate = DateTime.Now
            };
            db.Attendances.Add(attend);
            db.SaveChanges();
            return RedirectToAction("CreateAttendance", "Attendances", new {id = student.Grade.Id});
        }
        // GET: Attendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
        public ActionResult Create()
        {
            ViewBag.GradeId = new SelectList(db.Grades, "Id", "ClassName");
            ViewBag.StudentId = new SelectList(db.StudentInfoes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                attendance.IsCounted = true;
                attendance.AttendanceDate = DateTime.Now;
                db.Attendances.Add(attendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GradeId = new SelectList(db.Grades, "Id", "ClassName", attendance.GradeId);
            ViewBag.StudentId = new SelectList(db.StudentInfoes, "Id", "Name", attendance.StudentId);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.GradeId = new SelectList(db.Grades, "Id", "ClassName", attendance.GradeId);
            ViewBag.StudentId = new SelectList(db.StudentInfoes, "Id", "Name", attendance.StudentId);
            return View(attendance);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,GradeId,IsPresent,IsCounted,AttendanceDate")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GradeId = new SelectList(db.Grades, "Id", "ClassName", attendance.GradeId);
            ViewBag.StudentId = new SelectList(db.StudentInfoes, "Id", "Name", attendance.StudentId);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendance attendance = db.Attendances.Find(id);
            db.Attendances.Remove(attendance);
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
