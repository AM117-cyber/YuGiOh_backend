
public interface ITournamentService
{
    Task<Tournament> CreateTournament(TournamentInDto tournament);
    Task<TournamentOutDto> GetByName(string name);
    Task<IEnumerable<TournamentOutDto>> GetTournamentsByAdmin(int AdminId);
    Task<Tournament> UpdateTournament(TournamentInDto tournament, int Id);
    Task DeleteTournament(int tournamentId);
    Task<IEnumerable<TournamentOutDto>> GetUpcomingTournaments();
    Task<IEnumerable<TournamentOutDto>> GetTournamentsAwaitingConfirmation();
    Task<IEnumerable<TournamentOutDto>> GetStartedTournaments();
    Task<IEnumerable<TournamentOutDto>> GetAllTournaments();
    Task<bool> ConfirmTournamentStart(int tournamentId);
    Task<TournamentPlayerOutDto> GetTournamentChampion(int tournamentId);
    Task<IEnumerable<(string, int)>> winnerArchetypes(IEnumerable<int> tournamentsIds, DateTime startDate, DateTime endDate);
}