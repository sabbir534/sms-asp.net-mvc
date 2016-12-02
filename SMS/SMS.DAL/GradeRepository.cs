using System.Collections.Generic;
using SMS.BOL;

namespace SMS.DAL
{
    public class GradeRepository:IGradeRepository
    {
        private readonly SMSDBEntities _smsdb;

        public GradeRepository()
        {
            _smsdb = new SMSDBEntities();
        }
        public IEnumerable<Grade> GetGrades()
        {
            return _smsdb.Grades;
        }
    }
}