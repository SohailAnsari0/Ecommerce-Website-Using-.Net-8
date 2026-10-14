using EcommerceProject.Models.Entities;

namespace EcommerceProject.Models.DTOs
{
    public class OrderDetailModalDTO
    {
        public string DivId { get; set; }

        public IEnumerable<OrderItem> OrderItem { get; set; }
    }
}
