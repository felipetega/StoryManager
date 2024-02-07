using API.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Context
{
    public class ApiContext:DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //USER
            modelBuilder.Entity<User>().HasKey(p => p.Id);

            modelBuilder.Entity<User>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();

            modelBuilder.Entity<User>().Property(p => p.Name).IsRequired().HasMaxLength(25);

            //STORY
            modelBuilder.Entity<Story>().HasKey(p => p.Id);

            modelBuilder.Entity<Story>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();

            modelBuilder.Entity<Story>().Property(p => p.Title).IsRequired().HasMaxLength(25);

            modelBuilder.Entity<Story>().Property(p => p.Description).IsRequired().HasMaxLength(200);

            modelBuilder.Entity<Story>().Property(p => p.Department).IsRequired().HasMaxLength(25);

            // Configuração da tabela VOTE
            modelBuilder.Entity<Vote>().HasKey(p => p.Id);
            modelBuilder.Entity<Vote>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Vote>().Property(p => p.VoteValue).IsRequired();

            // Relacionamentos
            modelBuilder.Entity<Vote>()
                .HasOne(vote => vote.User)
                .WithMany(user => user.Votes)
                .HasForeignKey(vote => vote.UserId);

            modelBuilder.Entity<Vote>()
                .HasOne(vote => vote.Story)
                .WithMany(story => story.Votes)
                .HasForeignKey(vote => vote.StoryId);

        }
    }
}
