using Microsoft.EntityFrameworkCore;

public class TournamentRepository: ITournamentRepository
{
    private readonly IApplicationDbContext _context;

    public TournamentRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Tournament> Create(Tournament tournament)
    {
        _context.Tournaments.Add(tournament);
        await _context.SaveChangesAsync();
        return tournament;
    }

public async Task<Tournament> findByName(string name)
{
    return await _context.Tournaments
        .Include(t => t.Administrator) // Eager loading
        .FirstOrDefaultAsync(m => m.Name == name);
}

public async Task<Tournament> findById(int Id)
{
    return await _context.Tournaments
        .Include(t => t.Administrator) // Eager loading
        .FirstOrDefaultAsync(m => m.Id == Id);
}

public async Task<Tournament> findByIdWithMatches(int Id)
{
    return await _context.Tournaments
        .Include(t => t.TournamentMatches) // Eager loading
        .FirstOrDefaultAsync(m => m.Id == Id);
}

public async Task<Tournament> findByIdWithPlayers(int Id)
{
    var tournament = await _context.Tournaments
        .Include(t => t.TournamentPlayers)
        .SingleOrDefaultAsync(t => t.Id == Id);
        return tournament;
}

public async Task<IEnumerable<Tournament>> GetTournamentsByAdmin(int adminUserId)
{
    var tournaments = await _context.Tournaments
        .Where(t => t.Administrator.Id == adminUserId)
        .ToListAsync();

    return tournaments;
}

    public async Task<IEnumerable<Tournament>> GetUpcomingTournaments()
    {
        
        return await _context.Tournaments
        .Where(t => t.StartDate > DateTime.UtcNow)
        .ToListAsync();
    }

    public async Task<IEnumerable<Tournament>> GetStartedTournaments()
    {
         return await _context.Tournaments
            .Where(t => t.Status == TournamentStatus.started)
            .ToListAsync();
    }

        public async Task<IEnumerable<Tournament>> GetAllTournaments()
    {
        return await _context.Tournaments
        .ToListAsync();
    }

    public async Task UpdateTournament(Tournament tournament)
    {
        _context.Tournaments.Update(tournament);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTournament(Tournament tournament)
    {
        _context.Tournaments.Remove(tournament);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Tournament>> GetStartedTournamentsWithMatches()
    {
        return await _context.Tournaments
                .Include(t => t.TournamentMatches)
                .Where(t => t.Status == TournamentStatus.started)
                .ToListAsync();
    }

    public async Task<IEnumerable<Tournament>> GetTournamentsAwaitingConfirmation()
    {
        return await _context.Tournaments
        .Where(t => t.StartDate < DateTime.UtcNow && t.Status == TournamentStatus.pendant)
        .ToListAsync();
    }

}