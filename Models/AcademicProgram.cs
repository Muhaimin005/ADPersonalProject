using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADTest.Models
{
    public class AcademicProgram
    {
        [Key]
        [Required]
        public string ProgramId { get; set; }
        public string ProgramName { get; set; }
    }
}
