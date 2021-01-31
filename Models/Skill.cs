using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RPG_Project.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Damage { get; set; }

        public List<CharacterSkill> CharacterSkill { get; set; }
    }
}