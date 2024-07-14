using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADTest.Models
{
    public class Student : ApplicationUser
    {
        public string StudentId { get; set; }

        public string StudentName { get; set; }

        [ForeignKey("Supervisor")]
        public string? LecturerId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
    }
}
