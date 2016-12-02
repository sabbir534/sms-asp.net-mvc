using System.Collections.Generic;
using SMS.BOL;
using SMS.DAL;

namespace SMS.BLL
{
    public class GradeBL
    {
        private readonly GradeRepository _gradeObj;
        public GradeBL()
        {
            _gradeObj = new GradeRepository();
        }

        public IEnumerable<Grade> GetGrades()
        {
            return _gradeObj.GetGrades();
        }
    }
}