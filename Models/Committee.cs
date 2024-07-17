using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADTest.Models
{
    public class Committee
    {
        [Key]
        public string CommitteeId { get; set; }
        public string CommitteeName { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("AcademicProgram")]
        public string ProgramId { get; set; }
        public virtual AcademicProgram AcademicProgram { get; set; }

    }
}
