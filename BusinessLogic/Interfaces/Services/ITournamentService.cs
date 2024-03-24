public interface ITournamentService
{
    Task<Tournament> CreateTournament(TournamentInDto tournament);
    Task<Tournament> GetByName(string name);
    Task<IEnumerable<TournamentOutDto>> GetTournamentsByAdmin(int AdminId);
    Task<Tournament> UpdateTournament(TournamentInDto tournament, int Id);
    Task DeleteTournament(int tournamentId);
    Task<IEnumerable<TournamentOutDto>> GetUpcomingTournaments();
}