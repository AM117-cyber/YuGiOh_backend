

public interface ITournamentMatchRepository
{
    Task<TournamentMatch> CreateTournamentMatch(TournamentMatch newTournamentMatch);
    Task<TournamentMatch> findById(int matchId);
    Task<TournamentMatch> GetByPlayers(int player1Id, int player2Id);
    Task<IEnumerable<TournamentMatch>> GetRoundMatches(int tournamentId, int round);
    Task<IEnumerable<TournamentMatch>> GetTournamentMatches(int tournamentId);
    Task<IEnumerable<TournamentMatch>> GetMatchesWithPlayersNames(DateTime startDate, DateTime endDate);
    Task UpdateTournamentMatch(TournamentMatch tournamentMatch);
}