using BackendPilketos.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendPilketos
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Calon> Calons { get; set; }
        public DbSet<Periode> Periodes { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserAndGroup> UserAndGroups { get; set; }

        public DbSet<VWVoteUserGroup> VwVoteUserGroups { get; set; }
        public DbSet<VWUserAndVote> VwUserAndVotes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VWVoteUserGroup>(c =>
            {
                c.HasNoKey();
                c.ToView("VW_Vote_User_Group");
            });

            modelBuilder.Entity<UserAndGroup>(u =>
            {
                u.HasNoKey();
                u.ToView("VW_UserGroup");
            });
            modelBuilder.Entity<VWUserAndVote>(u =>
            {
                u.HasNoKey();
                u.ToView("VW_UserAndVoting");
            });
        }
    }
}
