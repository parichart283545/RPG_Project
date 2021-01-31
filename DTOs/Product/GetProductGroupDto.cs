using System.Collections.Generic;

namespace RPG_Project.DTOs
{
    public class GetProductGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GetProductDto> Product { get; set; }

    }
}