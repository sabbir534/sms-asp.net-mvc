using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace SMS.BOL
{
    public class StudentInfoViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required"), MaxLength(50), MinLength(3, ErrorMessage = "Minimum 3 Characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Father Name is Required"), MaxLength(50), MinLength(3)]
        [Display(Name = "Father's Name")]
        public string FatherName { get; set; }
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        [Display(Name = "Upload Photo")]
        public string Image { get; set; }
        [Display(Name = "Class")]
        public int? GradeId { get; set; }
        [Display(Name = "Roll No")]
        public Grade Grade { get; set; }
        public IEnumerable<Grade> Grades { get; set; }
        public int? RollNo { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int MaxContentLength = 1024 * 1024 * 3; //3 MB
            string[] allowedFileExtensions = new string[] { ".jpg", ".gif", ".png"};
            var file = value as HttpPostedFileBase;
            if (file == null)
                return false;
            else if (!allowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Please upload Your Photo of type: " + string.Join(", ", allowedFileExtensions);
                return false;
            }
            else if (file.ContentLength > MaxContentLength)
            {
                ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "MB";
                return false;
            }
            else
                return true;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AttachmentAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
          ValidationContext validationContext)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;

            // The file is required.
            if (file == null)
            {
                return new ValidationResult("Please upload a file!");
            }

            // The meximum allowed file size is 10MB.
            if (file.ContentLength > 10 * 1024 * 1024)
            {
                return new ValidationResult("This file is too big!");
            }

            // Only PDF can be uploaded.
            string ext = Path.GetExtension(file.FileName);
            if (String.IsNullOrEmpty(ext) ||
               !ext.Equals(".gif", StringComparison.OrdinalIgnoreCase)
                ||
               !ext.Equals(".png", StringComparison.OrdinalIgnoreCase)
                ||
               !ext.Equals(".jpg", StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult("This file is not a PDF!");
            }

            // Everything OK.
            return ValidationResult.Success;
        }
    }
}