using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ADTest.Models
{
    public class Proposal
    {
        [Required]
        public int ProposalId { get; set; }

        public string title { get; set; }
        public string type { get; set; }

        public string status { get; set; }

    }
}
