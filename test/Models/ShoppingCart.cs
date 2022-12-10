using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models
{
    public class ShoppingCart
    {

        public int Id { get; set; }

        [Range(1, 1000, ErrorMessage = "value must be between 1 to 100")]
        public int count { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser apllicationUser { get; set; }
        
        public int ProductId { get; set; }
        public Product product { get; set; }
        [NotMapped]
        public double price { get; set; }

    }
}
