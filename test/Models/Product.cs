using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Title { get; set; }
        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public String Author { get; set; }

        [Required]
        [Range(1,10000)]
        public Double ListPrice { get; set; }

        [Required]
        [Range(1,10000)]
        public Double price { get; set; }

        [Required]
        [Range(1,10000)]
        public Double price50 { get; set; }

        [Required,Range(1,10000)]
        public Double price100 { get; set; }
        [ValidateNever]
        public string UrlImage { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        public int CoverTypeId { get; set; }
        [ValidateNever]
        public CoverType CoverType { get; set; }

       

    }
}
