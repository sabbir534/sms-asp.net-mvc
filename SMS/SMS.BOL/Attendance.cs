//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMS.BOL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int GradeId { get; set; }
        public bool IsPresent { get; set; }
        public bool IsCounted { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
    
        public virtual Grade Grade { get; set; }
        public virtual StudentInfo StudentInfo { get; set; }
    }
}
