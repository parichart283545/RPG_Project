using System.Collections.Generic;
using System.Threading.Tasks;
using RPG_Project.DTOs;
using RPG_Project.DTOs.Figth;
using RPG_Project.Models;

namespace RPG_Project.Services.Character
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();

        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int characterId);

        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);

        Task<ServiceResponse<GetCharacterDto>> AddSkill(AddSkillDto newSkill);

        Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newSkill);

        Task<ServiceResponse<AttactResultDto>> WeaponAtk(WeaponAtkDto request);

        Task<ServiceResponse<AttactResultDto>> SkillAtk(SkillAtkDto request);
        Task<ServiceResponse<GetCharacterDto>> RemoveWeapon(int characterId);
        Task<ServiceResponse<GetCharacterDto>> RemoveSkill(int characterId);

    }
}