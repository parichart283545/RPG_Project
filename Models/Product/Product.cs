using System.ComponentModel.DataAnnotations;
namespace RPG_Project.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        [Required]
        public int ProductGroupId { get; set; }


    }
}