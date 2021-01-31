using System.ComponentModel.DataAnnotations;
namespace RPG_Project.DTOs
{
    public class AddProductDto
    {
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        [Required]
        public int ProductGroupId { get; set; }
    }
}