public interface ITournamentRepository
{
    Task<Tournament> Create(Tournament tournament);
    Task<Tournament> findByName(string name);
    Task<Tournament> findById(int Id);
    Task<Tournament> findByIdWithMatches(int Id);
    Task<Tournament> findByIdWithPlayers(int Id);
    Task<IEnumerable<Tournament>> GetTournamentsByAdmin(int adminUserId);
    Task<IEnumerable<Tournament>> GetUpcomingTournaments();
    Task<IEnumerable<Tournament>> GetTournamentsAwaitingConfirmation();
    Task<IEnumerable<Tournament>> GetStartedTournaments();
    Task UpdateTournament(Tournament tournament);
    Task DeleteTournament(Tournament tournament);
    Task<IEnumerable<Tournament>> GetAllTournaments();
    Task<IEnumerable<Tournament>> GetStartedTournamentsWithMatches();
}
