using Microsoft.EntityFrameworkCore;
using RPG_Project.Models;
using RPG_Project.Models.Report;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Project.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Weapon> Weapons { get; set; }

        public DbSet<CharacterSkill> CharacterSkills { get; set; }

        public DbSet<Skill> Skills { get; set; }
        public DbSet<ReportHeader> ReportHeaders { get; set; }
        public DbSet<ReportDetail> ReportDetails { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductGroup> ProductGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CharacterSkill>().HasKey(x => new { x.CharacterId, x.SkillId });
        }


    }
}