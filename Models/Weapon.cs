using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPG_Project.Models
{
    [Table("Weapon")]
    public class Weapon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Damage { get; set; }


        public int CharacterId { get; set; }

    }
}