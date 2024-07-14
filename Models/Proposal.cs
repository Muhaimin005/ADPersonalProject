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

        public string title { get; set; }
        public string type { get; set; }
        public string? LecturerId { get; set; }
        [ForeignKey(nameof(LecturerId))]
        [ValidateNever]
        public virtual Lecturer? lecturer { get; set; }
        public string status { get; set; }



    }
}
