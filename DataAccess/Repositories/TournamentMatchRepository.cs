using Microsoft.EntityFrameworkCore;

public class TournamentMatchRepository: ITournamentMatchRepository
{
    private readonly IApplicationDbContext _context;

    public TournamentMatchRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TournamentMatch> CreateTournamentMatch(TournamentMatch newTournamentMatch)
    {
        _context.TournamentMatches.Add(newTournamentMatch);
        await _context.SaveChangesAsync();
        return newTournamentMatch;
    }

    public async Task<TournamentMatch> findById(int matchId)
    {
        return await _context.TournamentMatches.FindAsync(matchId);
    }

    public async Task<TournamentMatch> GetByPlayers(int player1Id, int player2Id)
    {
        return await _context.TournamentMatches
            .FirstOrDefaultAsync(m => (m.Player1Id == player1Id && m.Player2Id == player2Id) 
                                   || (m.Player1Id == player2Id && m.Player2Id == player1Id));
    }

    public async Task<IEnumerable<TournamentMatch>> GetRoundMatches(int tournamentId, int round)
    {
        return await _context.TournamentMatches
            .Where(m => m.TournamentId == tournamentId && m.Round == round)
            .ToListAsync();
    }

    public async Task<IEnumerable<TournamentMatch>> GetTournamentMatches(int tournamentId)
    {
        return await _context.TournamentMatches
            .Where(m => m.TournamentId == tournamentId)
            .ToListAsync();
    }

    public async Task UpdateTournamentMatch(TournamentMatch tournamentMatch)
    {
        _context.TournamentMatches.Update(tournamentMatch);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TournamentMatch>> GetMatchesWithPlayersNames(DateTime startDate, DateTime endDate)
    {
        return await _context.TournamentMatches
            .Where(m => m.Date > startDate && m.Date < endDate)
            .ToListAsync();
    }

}
