public interface ITournamentMatchService
{
    Task<TournamentMatchOutDto> CreateTournamentMatch(TournamentMatchInDto tournamentMatch);
    Task<IEnumerable<TournamentMatchOutDto>> GetTournamentMatches(int tournamentId);
    Task<TournamentMatchOutDto> UpdateTournamentMatch(int matchId, int player1Score, int player2Score, string date);
    Task<IEnumerable<TournamentMatchOutDto>> GetRoundMatches(int tournamentId, int round);
    Task<TournamentMatchOutDto> GetByPlayers(int player1Id, int player2Id);
    Task<IEnumerable<(string, int)>> getPlayersWithVictories(DateTime startDate, DateTime endDate);
    Task<IEnumerable<(string, int)>> mostPopularArchetypeInTournamentRound(int tournamentId, int round);
}