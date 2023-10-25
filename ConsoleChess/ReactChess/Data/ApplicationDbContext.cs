using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReactBoardGame.Models;

namespace ReactBoardGame.Data
{
    public partial class ApplicationDbContext : ApiAuthorizationDbContext<User>
    {
        public DbSet<Match> MatchSet { get; set; } = null!;
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }
      
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            OnModelCreatingPartial(builder);
            builder.Entity<Match>().HasOne(m => m.Player1).WithMany().HasForeignKey(m=>m.Player1Id).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Match>().HasOne(m => m.Player2).WithMany().HasForeignKey(m => m.Player2Id).OnDelete(DeleteBehavior.Restrict);
            
        }
        partial void OnModelCreatingPartial(ModelBuilder builder);

    }
}