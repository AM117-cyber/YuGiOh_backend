public interface ITournamentPlayerRepository
{
    Task<TournamentPlayer> Create(TournamentPlayer tournamentPlayer);
    Task<TournamentPlayer> findById(int Id);
    Task<TournamentPlayerOutDto> GetTournamentOutDto(int tournamentPlayerId);
    Task<bool> findTournamentPlayer(TournamentPlayerInDto tournamentPlayer);
    Task<IEnumerable<TournamentPlayer>> GetHiddenTournamentPlayers(int tournamentId);
    Task AcceptSolicitude(TournamentPlayer tournamentPlayer);
    Task DeleteSolicitude(int tournamentPlayerId);
    Task<IEnumerable<TournamentPlayer>> TournamentPlayersByTournament(int tournamentId);
    Task DeleteTournamentPlayers(ICollection<TournamentPlayer> tournamentPlayers);
}
