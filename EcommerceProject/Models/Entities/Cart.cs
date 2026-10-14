using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models.Entities
{
    [Table("carts")]
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } 

        public List<CartItem> CartItems { get; set; }
    }
}
