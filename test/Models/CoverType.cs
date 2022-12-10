using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Cover Type")]
        public string Name { get; set; }
    }
}
