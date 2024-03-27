public interface ITournamentRepository
{
    Task<Tournament> Create(Tournament tournament);
    Task<Tournament> findByName(string name);
    Task<Tournament> findById(int Id);
    Task<Tournament> findByIdWithPlayers(int Id);
    Task<IEnumerable<Tournament>> GetTournamentsByAdmin(int adminUserId);
    Task<IEnumerable<Tournament>> GetUpcomingTournaments();
    Task UpdateTournament(Tournament tournament);
    Task DeleteTournament(Tournament tournament);
    Task<IEnumerable<Tournament>> GetAllTournaments();
}
