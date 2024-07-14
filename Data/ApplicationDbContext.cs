using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using ADTest.Models;

namespace ADTest.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> students { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }

        public DbSet<Proposal> proposals { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var Admin = new IdentityRole("Admin");
            Admin.NormalizedName = "Admin";

            var Student = new IdentityRole("Student");
            Student.NormalizedName = "Student";

            var Lecturer = new IdentityRole("Lecturer");
            Lecturer.NormalizedName = "Lecturer";

            builder.Entity<IdentityRole>().HasData(Admin, Student, Lecturer);
        }

    }
}
