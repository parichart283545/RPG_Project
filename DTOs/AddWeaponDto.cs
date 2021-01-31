using System.ComponentModel.DataAnnotations;
using RPG_Project.Validations;

namespace RPG_Project.DTOs
{
    public class AddWeaponDto
    {
        [FirstLetterUpperCaseAttribute]
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, 100)]
        public int Damage { get; set; }
        [Required]
        public int CharacterId { get; set; }
    }
}