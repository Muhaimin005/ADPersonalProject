﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADTest.Models
{
    public class Student
    {
        public string StudentId { get; set; }

        public string StudentName { get; set; }

        [ForeignKey("AcademicProgram")]
        public string ProgramId { get; set; }
        public virtual AcademicProgram AcademicProgram { get; set; }

        public string? LecturerId { get; set; }
        [ForeignKey(nameof(LecturerId))]
        [ValidateNever]
        public virtual Lecturer? lecturer { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string applicationStatus { get; set; }

        public int semester {  get; set; }

        public string academicSession {  get; set; }

        public virtual  Proposal proposal { get; set; }
    }
}