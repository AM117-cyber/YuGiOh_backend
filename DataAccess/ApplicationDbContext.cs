using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Deck> Decks { get; set; }
    public DbSet<Municipality> Municipalities { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<TournamentMatch> TournamentMatches { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<TournamentPlayer> TournamentPlayers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUserLogin<int>>().HasKey(p => p.UserId);
        modelBuilder.Entity<TournamentMatch>()
            .HasOne(tm => tm.Player1)
            .WithMany(p => p.Player1Matches)
            .HasForeignKey(tm => tm.Player1Id);

        modelBuilder.Entity<TournamentMatch>()
            .HasOne(tm => tm.Player2)
            .WithMany(p => p.Player2Matches)
            .HasForeignKey(tm => tm.Player2Id)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}
