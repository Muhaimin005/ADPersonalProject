using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ADTest.Models
{
    public class Lecturer
    {
        [Key]
        public string LecturerId { get; set; }

        public string LecturerName { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        public string LecturerAddress { get; set; }

        [ForeignKey("AcademicProgram")]
        public string ProgramId { get; set; }

        public string FieldofStudy { get; set; }
        public string? domain { get; set; }


    }
}
