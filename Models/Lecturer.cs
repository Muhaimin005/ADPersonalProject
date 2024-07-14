using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ADTest.Models
{
    public class Lecturer
    {
        public string LecturerId { get; set; }

        public string LecturerName { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        public string LecturerAddress { get; set; }
    }
}
