using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models.Entities
{
    [Table("products")]
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Colour { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Image { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<CartItem> CartItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
    }
}
