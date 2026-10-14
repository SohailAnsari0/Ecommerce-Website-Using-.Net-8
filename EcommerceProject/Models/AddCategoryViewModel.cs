using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Models
{
    public class AddCategoryViewModel
    {
        [Required]
        [MaxLength(40)]
        public string CategoryName { get; set; }
    }
}
