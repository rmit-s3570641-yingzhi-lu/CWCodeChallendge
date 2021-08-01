using CW.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace CWCodeChallendge.Models
{
    public class ProductDto
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        public string Price { get; set; }

        [EnumDataType(typeof(ProductTypes))]
        [Required]
        public string Type { get; set; }

        public bool IsActive { get; set; }
    }
}
