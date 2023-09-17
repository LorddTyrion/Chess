using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReactChess.Models;

namespace ReactChess.Data
{
    public partial class ApplicationDbContext : ApiAuthorizationDbContext<User>
    {
        public DbSet<Match> MatchSet { get; set; } = null!;
        public DbSet<PlayedMatch> PlayedMatchSet { get; set; } = null!;
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=onlab;Integrated Security= True;");
            }
        }*/
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            OnModelCreatingPartial(builder);
            builder.Entity<PlayedMatch>().HasOne(p => p.Match).WithMany().HasForeignKey(p=>p.MatchId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<PlayedMatch>().HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserID).OnDelete(DeleteBehavior.Restrict);
            
        }
        partial void OnModelCreatingPartial(ModelBuilder builder);

    }
}