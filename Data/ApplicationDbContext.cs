﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using ADTest.Models;

namespace ADTest.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> student { get; set; }
        public DbSet<Lecturer> lecturer { get; set; }

        public DbSet<Proposal> proposal { get; set; }

        public DbSet<ADTest.Models.AcademicProgram> AcademicProgram { get; set; }

        public DbSet<Committee> committee { get; set; }

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

            var Committee = new IdentityRole("Committee");
            Committee.NormalizedName = "Committee";

            builder.Entity<IdentityRole>().HasData(Admin, Student, Lecturer, Committee);
        }    
    }
}
