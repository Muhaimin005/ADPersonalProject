using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADTest.Models
{
    public class Proposal
    {
        [Required]
        public int ProposalId { get; set; }
        public string StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        [ValidateNever]
        public virtual Student? student { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string? LecturerId1 { get; set; }
        [ForeignKey(nameof(LecturerId1))]
        [ValidateNever]
        public virtual Lecturer? lecturer1 { get; set; }
        public string? LecturerId2 { get; set; }
        [ForeignKey(nameof(LecturerId2))]
        [ValidateNever]
        public virtual Lecturer? lecturer2 { get; set; }
        public string status { get; set; }



    }
}
