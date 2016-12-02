using System.Collections.Generic;
using SMS.BOL;

namespace SMS.DAL
{
    public interface IGradeRepository
    {
        IEnumerable<Grade> GetGrades();
    }
}