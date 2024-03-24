public interface ITournamentPlayerService
{
    Task<TournamentPlayerOutDto> CreateTournamentPlayer(TournamentPlayerInDto tournamentPlayer);
    Task<IEnumerable<TournamentPlayerOutDto>> GetPendantSolicitudes(int tournamentId);
    Task AcceptSolicitude(int tournamentPlayerId);
    Task DeleteSolicitude(int tournamentPlayerId);
    Task<IEnumerable<TournamentPlayerOutDto>> GetTournamentPlayers(int tournamentId);

}