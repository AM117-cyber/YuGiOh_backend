public interface ITournamentPlayerService
{
    Task<TournamentPlayerOutDto> CreateTournamentPlayer(TournamentPlayerInDto tournamentPlayer);
    Task<IEnumerable<TournamentPlayerOutDto>> GetPendantSolicitudes(int tournamentId);
    Task AcceptSolicitude(int tournamentPlayerId);
    Task DeleteSolicitude(int tournamentPlayerId);
    Task<IEnumerable<TournamentPlayerOutDto>> GetTournamentPlayers(int tournamentId);
    Task<(IEnumerable<string>, int)> mostPopularArchetypeInTournament(int tournamentId);
    Task<(IEnumerable<string>, int)> mostWinnersProvince(DateTime startDate, DateTime endDate);
    Task<(IEnumerable<MunicipalityOutDto>, int)> mostWinnersMunicipality(DateTime startDate, DateTime endDate);
    Task<IEnumerable<(string, int)>> mostPopularArchetypes();
}