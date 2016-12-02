using System.Collections.Generic;
using System.Web;
using SMS.BOL;
using SMS.DAL;

namespace SMS.BLL
{
    public class StudentBL
    {
        private readonly StudentRepository _studentObj;

        public StudentBL()
        {
            _studentObj = new StudentRepository();
        }

        public IEnumerable<StudentInfo> GetStudentInfos()
        {
            return _studentObj.GetStudentInfos();
        }
        public StudentInfo GetStudentById(int? id)
        {
            return _studentObj.GetStudentById(id);
        }

        public void InsertStudent(StudentInfoViewModel vm, HttpPostedFileBase imageFile)
        {
            _studentObj.InsertStudent(vm, imageFile);
        }

        public StudentInfoViewModel EditStudent(StudentInfo studentInfo)
        {
            return _studentObj.EditStudent(studentInfo);
        }

        public void UpdateStudent(StudentInfoViewModel vm, HttpPostedFileBase imageFile)
        {
            _studentObj.UpdateStudent(vm, imageFile);
        }

        public void DeleteStudent(int? id)
        {
            _studentObj.DeleteStudent(id);
        }
    }
}