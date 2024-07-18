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
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string LecturerAddress { get; set; }

        public string FieldofStudy { get; set; }
        public string? domain { get; set; }

        public string? isCommittee { get; set; }
    }
}
