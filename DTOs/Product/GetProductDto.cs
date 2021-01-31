using System.Collections.Generic;

namespace RPG_Project.DTOs
{
    public class GetProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        public int ProductGroupId { get; set; }
        // public GetProductGroupDto ProductGroup { get; set; }

    }
}