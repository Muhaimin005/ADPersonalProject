using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ADTest.Models;

namespace ADTest.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        [StringLength(12)]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PhoneNumber { get; set; }
    }
}
