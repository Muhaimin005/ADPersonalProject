using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ADTest.Models;

namespace ADTest.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public Student student { get; set; }
        public Lecturer lecturer { get; set; }

    }
}
