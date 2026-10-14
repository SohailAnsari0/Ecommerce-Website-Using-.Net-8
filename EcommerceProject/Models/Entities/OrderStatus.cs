using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models.Entities
{

    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int Id { get; set; }

        [Required]
        public int StatusId { get; set; }
        [Required, MaxLength(30)]
        public string? StatusName { get; set; }
    }
}
