using System.Collections.Generic;
using System.Web;
using SMS.BOL;

namespace SMS.DAL
{
    public interface IStudentRepository
    {
        IEnumerable<StudentInfo> GetStudentInfos();
        StudentInfo GetStudentById(int? id);
        void InsertStudent(StudentInfoViewModel vm, HttpPostedFileBase imageFile);
        StudentInfoViewModel EditStudent(StudentInfo student);
        void UpdateStudent(StudentInfoViewModel vm, HttpPostedFileBase imageFile);
        void DeleteStudent(int? id);
        void Save();

    }
}