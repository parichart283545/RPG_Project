using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPG_Project.Data;
using RPG_Project.DTOs;
using RPG_Project.DTOs.Figth;
using RPG_Project.Models;

namespace RPG_Project.Services.Character
{
    public class CharacterService : ICharacterService
    {
        private readonly AppDBContext _dBContext;
        private readonly IMapper _mapper;
        private readonly ILogger _log;

        public CharacterService(AppDBContext dBContext, IMapper mapper, ILogger<CharacterService> log)
        {
            this._dBContext = dBContext;
            this._mapper = mapper;
            this._log = log;
        }


        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newSkill)
        {
            var character = await _dBContext.Characters.Include(x => x.Weapon).Include(x => x.CharacterSkills).ThenInclude(x => x.Skill).FirstOrDefaultAsync(x => x.Id == newSkill.CharacterId);

            if (character == null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Character not found");
            }

            var skill = await _dBContext.Skills.FirstOrDefaultAsync(x => x.Id == newSkill.SkillId);

            if (skill == null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Skill not found");
            }

            var c_Skill = new CharacterSkill();

            c_Skill.CharacterId = newSkill.CharacterId;
            c_Skill.SkillId = newSkill.SkillId;

            _dBContext.CharacterSkills.Add(c_Skill);
            await _dBContext.SaveChangesAsync();

            var dto = _mapper.Map<GetCharacterDto>(character);
            return ResponseResult.Success(dto);

        }

        public Task<ServiceResponse<GetCharacterDto>> AddSkill(AddSkillDto newSkill)
        {

            throw new System.NotImplementedException();
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var character = await _dBContext.Characters.Include(x => x.Weapon).FirstOrDefaultAsync(x => x.Id == newWeapon.CharacterId);

            if (character == null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Character not found");
            }

            var weapon = new Weapon
            {
                Name = newWeapon.Name,
                Damage = newWeapon.Damage,
                CharacterId = newWeapon.CharacterId
            };

            _dBContext.Weapons.Add(weapon);
            await _dBContext.SaveChangesAsync();

            var dto = _mapper.Map<GetCharacterDto>(character);
            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var characters = await _dBContext.Characters.Include(x => x.Weapon).Include(x => x.CharacterSkills).ThenInclude(x => x.Skill).AsNoTracking().ToListAsync();


            var dto = _mapper.Map<List<GetCharacterDto>>(characters);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int characterId)
        {
            var character = await _dBContext.Characters.Include(x => x.Weapon).Include(x => x.CharacterSkills).ThenInclude(x => x.Skill).FirstOrDefaultAsync(x => x.Id == characterId);
            //var characters = await _dBContext.Characters.Include(x => x.Weapon).AsNoTracking().ToListAsync();
            if (character == null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Character not found.");
            }

            var dto = _mapper.Map<GetCharacterDto>(character);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetCharacterDto>> RemoveSkill(int characterId)
        {
            var character = await _dBContext.Characters
            .Include(x => x.Weapon)
            .Include(x => x.CharacterSkills).ThenInclude(x => x.Skill)
            .FirstOrDefaultAsync(x => x.Id == characterId);
            if (character is null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Character not found.");
            }

            var charSkill = await _dBContext.CharacterSkills.Where(x => x.CharacterId == characterId).ToListAsync();
            if (charSkill is null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Skill not found.");
            }

            _dBContext.CharacterSkills.RemoveRange(charSkill);
            await _dBContext.SaveChangesAsync();

            var dto = _mapper.Map<GetCharacterDto>(character);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetCharacterDto>> RemoveWeapon(int characterId)
        {
            var character = await _dBContext.Characters
            .Include(x => x.Weapon)
            .Include(x => x.CharacterSkills).ThenInclude(x => x.Skill)
            .FirstOrDefaultAsync(x => x.Id == characterId);
            if (character is null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Character not found.");
            }

            var weapon = await _dBContext.Weapons.Where(x => x.CharacterId == characterId).FirstOrDefaultAsync();
            if (weapon is null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Weapon not found.");
            }

            _dBContext.Weapons.Remove(weapon);
            await _dBContext.SaveChangesAsync();

            var dto = _mapper.Map<GetCharacterDto>(character);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<AttactResultDto>> SkillAtk(SkillAtkDto request)
        {
            try
            {
                var attacker = await _dBContext.Characters
                .Include(x => x.CharacterSkills).ThenInclude(x => x.Skill)
                .FirstOrDefaultAsync(x => x.Id == request.AttackerId);

                if (attacker is null)
                {
                    var msg = $"This attackerId {request.AttackerId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttactResultDto>(msg);
                }

                var opponent = await _dBContext.Characters
                .Include(x => x.Weapon)
                .FirstOrDefaultAsync(x => x.Id == request.OpponentId);

                if (opponent is null)
                {
                    var msg = $"This opponentId {request.OpponentId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttactResultDto>(msg);
                }

                var charSkill = await _dBContext.CharacterSkills.Include(x => x.Skill)
                .FirstOrDefaultAsync(x => x.CharacterId == request.AttackerId && x.SkillId == request.SkillId);
                if (charSkill is null)
                {
                    var msg = $"This Attacker doesn't know this skill {request.OpponentId}.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttactResultDto>(msg);
                }

                int damage;
                damage = charSkill.Skill.Damage + attacker.Intelligence;
                damage -= opponent.Defense;

                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }

                string atkResultMessage;

                if (opponent.HitPoints <= 0)
                {
                    atkResultMessage = $"{opponent.Name} is dead.";
                }
                else
                {
                    atkResultMessage = $"{opponent.Name} HP Remain {opponent.HitPoints}";
                }

                _dBContext.Characters.Update(opponent);
                await _dBContext.SaveChangesAsync();

                var dto = new AttactResultDto
                {
                    AttackerName = attacker.Name,
                    AttackHP = attacker.HitPoints,
                    OpponentName = opponent.Name,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage,
                    AttackResultMessage = atkResultMessage
                };

                _log.LogInformation(atkResultMessage);
                _log.LogInformation("Weapon attack done.");

                return ResponseResult.Success(dto, atkResultMessage);

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return ResponseResult.Failure<AttactResultDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<AttactResultDto>> WeaponAtk(WeaponAtkDto request)
        {
            try
            {
                var attacker = await _dBContext.Characters.Include(x => x.Weapon).FirstOrDefaultAsync(x => x.Id == request.AttackerId);

                if (attacker is null)
                {
                    var msg = $"This attackerId {request.AttackerId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttactResultDto>(msg);
                }

                var opponent = await _dBContext.Characters.Include(x => x.Weapon).FirstOrDefaultAsync(x => x.Id == request.OpponentId);

                if (opponent is null)
                {
                    var msg = $"This opponentId {request.OpponentId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttactResultDto>(msg);
                }

                int damage;
                damage = attacker.Weapon.Damage + attacker.Strength;
                damage -= opponent.Defense;

                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }

                string atkResultMessage;

                if (opponent.HitPoints <= 0)
                {
                    atkResultMessage = $"{opponent.Name} is dead.";
                }
                else
                {
                    atkResultMessage = $"{opponent.Name} HP Remain {opponent.HitPoints}";
                }



                _dBContext.Characters.Update(opponent);
                _dBContext.Characters.Update(opponent);
                await _dBContext.SaveChangesAsync();

                var dto = new AttactResultDto
                {
                    AttackerName = attacker.Name,
                    AttackHP = attacker.HitPoints,
                    OpponentName = opponent.Name,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage,
                    AttackResultMessage = atkResultMessage

                };
                _log.LogInformation(atkResultMessage);
                _log.LogInformation("Weapon attack done.");
                return ResponseResult.Success(dto, atkResultMessage);

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return ResponseResult.Failure<AttactResultDto>(ex.Message);
            }
        }
    }
}