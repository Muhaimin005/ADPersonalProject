using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADTest.Models.ViewModel
{
    public class ProposalInputModel
    {
        public string StudentId { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public int semester { get; set; }
        public string session { get; set; }
        public IFormFile proposalForm { get; set; } // Store the file data as a byte array
    }
}
