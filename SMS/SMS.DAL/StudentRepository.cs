using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.ModelBinding;
using SMS.BOL;

namespace SMS.DAL
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SMSDBEntities _smsdb;

        public StudentRepository()
        {
            _smsdb = new SMSDBEntities();
        }

        public IEnumerable<StudentInfo> GetStudentInfos()
        {
            return _smsdb.StudentInfoes.Include(s => s.Grade).Where(s => !s.IsDeleted.Value).ToList();
        }

        public StudentInfo GetStudentById(int? id)
        {
            return _smsdb.StudentInfoes.First(s => s.Id == id);
        }

        public void InsertStudent(StudentInfoViewModel vm, HttpPostedFileBase imageFile)
        {
            var newStudent = new StudentInfo
            {
                Name = vm.Name,
                FatherName = vm.FatherName,
                Address = vm.Address,
                DOB = vm.DOB,
                GradeId = vm.GradeId,
                Image = imageFile.FileName,
                IsDeleted = false,
                RollNo = vm.RollNo
            };
            _smsdb.StudentInfoes.Add(newStudent);
            try
            {
                Save();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                );
            }
            imageFile.SaveAs(HttpContext.Current.Server.MapPath("~/Images") + "/" + newStudent.Id.ToString() + "_" + imageFile.FileName);
        }

        public StudentInfoViewModel EditStudent(StudentInfo studentInfo)
        {
            var vm = new StudentInfoViewModel
            {
                Id = studentInfo.Id,
                Address = studentInfo.Address,
                DOB = studentInfo.DOB,
                Name = studentInfo.Name,
                FatherName = studentInfo.FatherName,
                GradeId = studentInfo.GradeId,
                IsDeleted = studentInfo.IsDeleted,
                RollNo = studentInfo.RollNo,
                Image = studentInfo.Image


            };
            return vm;
        }

        public void UpdateStudent(StudentInfoViewModel vm, HttpPostedFileBase imageFile)
        {
            var updateStudent = GetStudentById(vm.Id);
            updateStudent.Name = vm.Name;
            updateStudent.FatherName = vm.FatherName;
            updateStudent.DOB = vm.DOB;
            updateStudent.GradeId = vm.GradeId;
            updateStudent.Address = vm.Address;
            updateStudent.RollNo = vm.RollNo;
            if (imageFile != null)
            {
                updateStudent.Image = imageFile.FileName;
                imageFile.SaveAs(HttpContext.Current.Server.MapPath("~/Images") + "/" + updateStudent.Id.ToString() + "_" + imageFile.FileName);
            }
            Save();
        }

        public void DeleteStudent(int? id)
        {
            var studentDelete = GetStudentById(id);
            studentDelete.IsDeleted = true;
            //db.StudentInfoes.Remove(studentInfo);
            Save();
        }

        public void Save()
        {
            _smsdb.SaveChanges();
        }
    }
}