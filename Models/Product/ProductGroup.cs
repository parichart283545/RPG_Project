using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace RPG_Project.Models
{
    public class ProductGroup
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Product { get; set; }
    }
}