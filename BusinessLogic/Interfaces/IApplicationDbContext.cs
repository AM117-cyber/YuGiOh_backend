using Microsoft.EntityFrameworkCore;

public interface IApplicationDbContext
{
    DbSet<Player> Players { get; set; }
    DbSet<Deck> Decks { get; set; }
    DbSet<Municipality> Municipalities { get; set; }
    DbSet<Province> Provinces { get; set; }
    DbSet<TournamentMatch> TournamentMatches { get; set; }
    DbSet<Tournament> Tournaments { get; set; }
    DbSet<TournamentPlayer> TournamentPlayers { get; set; }

    Task<int> SaveChangesAsync();
}
