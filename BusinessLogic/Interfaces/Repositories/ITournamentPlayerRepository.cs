public interface ITournamentPlayerRepository
{
    Task<IEnumerable<TournamentPlayer>> AllTournamentPlayersWithArchetypes();
    Task<TournamentPlayer> Create(TournamentPlayer tournamentPlayer);
    Task<TournamentPlayer> findById(int Id);
    Task<TournamentPlayer> findByIdWithPlayerAndDeck(int Id);
    Task<TournamentPlayer> GetTournamentPlayer(int tournamentPlayerId);
    Task<bool> findTournamentPlayer(TournamentPlayerInDto tournamentPlayer);
    Task<IEnumerable<TournamentPlayer>> GetHiddenTournamentPlayers(int tournamentId);
    Task AcceptSolicitude(TournamentPlayer tournamentPlayer);
    Task DeleteSolicitude(int tournamentPlayerId);
    Task<IEnumerable<TournamentPlayer>> TournamentPlayersByTournament(int tournamentId);
    Task DeleteTournamentPlayers(ICollection<TournamentPlayer> tournamentPlayers);
    Task<IEnumerable<TournamentPlayer>> TournamentPlayerWithDeckArchetype(int tournamentId, IEnumerable<int> playersIds);
}
