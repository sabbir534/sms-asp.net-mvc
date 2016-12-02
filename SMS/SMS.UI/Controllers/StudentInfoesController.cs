using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CrystalDecisions.CrystalReports.Engine;
using SMS.BLL;
using SMS.BOL;

namespace SMS.UI.Controllers
{
    public class StudentInfoesController : Controller
    {
        private readonly StudentBL _studentBl;
        private readonly PaymentBL _paymentBl;
        private readonly GradeBL _gradeBl;
        public StudentInfoesController()
        {
            _studentBl = new StudentBL();
            _paymentBl = new PaymentBL();
            _gradeBl = new GradeBL();

        }
        
        // GET: StudentInfoes
        public ActionResult Index()
        {
            var studentInfoes = _studentBl.GetStudentInfos();
            return View(studentInfoes);
        }

        public ActionResult PaymentDetailsByStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = _studentBl.GetStudentById(id);
            var paymentDetail = _paymentBl.GetPaymentDetailsByStudent(id);
            ViewBag.StudentId = id;
            ViewBag.StudentName = student.Name;
            ViewBag.Grade = student.Grade.ClassName;
            return View(paymentDetail);

        }

        public ActionResult AddPaymentByStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var student = _studentBl.GetStudentById(id);
            if (student == null) throw new ArgumentNullException(nameof(student));
            ViewBag.GradeId = new SelectList(_gradeBl.GetGrades(), "Id", "ClassName", student.GradeId);
            ViewBag.StudentId = new SelectList(_studentBl.GetStudentInfos(), "Id", "Name", student.Id);
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPaymentByStudent(int? id, PaymentDetail paymentDetail)
        {
            if (ModelState.IsValid)
            {
                _paymentBl.InsertPaymentByStudent(paymentDetail);
                return RedirectToAction("PaymentDetailsByStudent", "StudentInfoes", new { id = id });
            }

            ViewBag.GradeId = new SelectList(_gradeBl.GetGrades(), "Id", "ClassName", paymentDetail.GradeId);
            ViewBag.StudentId = new SelectList(_studentBl.GetStudentInfos(), "Id", "Name", paymentDetail.StudentId);
            return View(paymentDetail);

        }
        // GET: StudentInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentInfo = _studentBl.GetStudentById(id);
            if (studentInfo == null)
            {
                return HttpNotFound();
            }
            return View(studentInfo);
        }

        // GET: StudentInfoes/Create
        public ActionResult Create()
        {
            ViewBag.GradeId = new SelectList(_gradeBl.GetGrades(), "Id", "ClassName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentInfoViewModel vm, HttpPostedFileBase imageFile)
        {

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    if (!(imageFile.FileName.ToLower().EndsWith(".jpg") || imageFile.FileName.ToLower().EndsWith(".png") || imageFile.FileName.ToLower().EndsWith(".gif")))
                    {

                        ModelState.AddModelError("Image", "Invalid Image Format");
                    }
                    else
                    {
                        _studentBl.InsertStudent(vm, imageFile);

                        return RedirectToAction("Index");
                    }
                }

            }
            ModelState.AddModelError("Image", "Image is Rquired");
            ViewBag.GradeId = new SelectList(_gradeBl.GetGrades(), "Id", "ClassName", vm.GradeId);
            return View(vm);


        }

        // GET: StudentInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentInfo = _studentBl.GetStudentById(id);
            if (studentInfo == null)
            {
                return HttpNotFound();
            }
            var vm = _studentBl.EditStudent(studentInfo);
            ViewBag.GradeId = new SelectList(_gradeBl.GetGrades(), "Id", "ClassName", studentInfo.GradeId);
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentInfoViewModel vm, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && !(imageFile.FileName.ToLower().EndsWith(".jpg") || imageFile.FileName.ToLower().EndsWith(".png") || imageFile.FileName.ToLower().EndsWith(".gif")))
                {
                    ModelState.AddModelError("Image", "Invalid Image Format");
                }
                else
                {
                    _studentBl.UpdateStudent(vm, imageFile);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.GradeId = new SelectList(_gradeBl.GetGrades(), "Id", "ClassName", vm.GradeId);
            return View(vm);
        }

        // GET: StudentInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentInfo = _studentBl.GetStudentById(id);
            if (studentInfo == null)
            {
                return HttpNotFound();
            }
            return View(studentInfo);
        }

        // POST: StudentInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _studentBl.DeleteStudent(id);
            return RedirectToAction("Index");
        }

        /*protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/

        /*public ActionResult GetReport()
        {
            var rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "StudentReport.rpt"));
            rd.SetDatabaseLogon("sa", "000000", "DESKTOP-M84FJ0F", "SMSDB");
            var s = db.StudentInfoes.Where(t => t.Id == 20).Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                GradeId = p.Grade.ClassName,
                PaymentDetails = p.PaymentDetails.Select(pm => new
                {
                    StudentName = pm.StudentId.Value,
                    TuitionFee = pm.TuitionFee
                }).ToList()
            });
            rd.Database.Tables[0].SetDataSource(s);
            /*rd.SetDataSource(db.StudentInfoes.Select(s => new
            {
                Id = s.Id,
                Name = s.Name,
                GradeId = s.Grade.ClassName
            }).ToList());#1#
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Payment.pdf");
            }
            catch (Exception ex)
            {

                throw;
            }
        }*/
    }
}

