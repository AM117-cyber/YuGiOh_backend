using System.ComponentModel;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

public class TournamentPlayerRepository: ITournamentPlayerRepository
{
    private readonly IApplicationDbContext _context;

    public TournamentPlayerRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TournamentPlayer>> AllTournamentPlayersWithArchetypes()
    {
        return await _context.TournamentPlayers
                        .Include(tp => tp.Deck.Archetype)
                        .ToListAsync();
    }           
    public async Task<TournamentPlayer> Create(TournamentPlayer tournamentPlayer)
    {
        _context.TournamentPlayers.Add(tournamentPlayer);
        await _context.SaveChangesAsync();
        return tournamentPlayer;
    }

    public async Task<TournamentPlayerOutDto> GetTournamentOutDto(int tournamentPlayerId)
{
    var tournamentPlayer = await _context.TournamentPlayers
        .Include(tp => tp.Player)
        .Include(tp => tp.Deck)
        .Include(tp => tp.Tournament)
        .SingleOrDefaultAsync(tp => tp.Id == tournamentPlayerId);

    if (tournamentPlayer == null)
    {
        throw new Exception("TournamentPlayer not found");
    }

    var tournamentOutDto = new TournamentPlayerOutDto
    {
        PlayerId = tournamentPlayer.PlayerId,
        PlayerName = tournamentPlayer.Player.UserName,
        DeckId = tournamentPlayer.DeckId,
        DeckName = tournamentPlayer.Deck.Name,
        TournamentId = tournamentPlayer.TournamentId,
        TournamentName = tournamentPlayer.Tournament.Name
    };

    return tournamentOutDto;
}

public async Task<bool> findTournamentPlayer(TournamentPlayerInDto tournamentPlayer)
{
    var existingTournamentPlayer = await _context.TournamentPlayers
        .AnyAsync(tp => tp.PlayerId == tournamentPlayer.PlayerId && tp.TournamentId == tournamentPlayer.TournamentId);
    return existingTournamentPlayer != null;
}
public async Task<TournamentPlayer> findById(int Id)
{
    return await _context.TournamentPlayers
        .FirstOrDefaultAsync(m => m.Id == Id);
}

public async Task<TournamentPlayer> findByIdWithPlayerAndDeck(int Id)
{
    return await _context.TournamentPlayers
        .Include(tp => tp.Player.UserName)
        .Include(tp => tp.Deck)
        .FirstOrDefaultAsync(m => m.Id == Id);
}

    public async Task<IEnumerable<TournamentPlayer>> GetHiddenTournamentPlayers(int tournamentId)
    {
        var tournamentPlayers = await _context.TournamentPlayers
        .Where(tp => tp.TournamentId == tournamentId && tp.Status == EntityStatus.hidden)
        .Include(tp => tp.Player)
        .Include(tp => tp.Deck)
        .Include(tp => tp.Tournament)
        .ToListAsync();
    return tournamentPlayers;
}

public async Task AcceptSolicitude(TournamentPlayer tournamentPlayer)
{
    tournamentPlayer.Status = EntityStatus.visible;
    _context.TournamentPlayers.Update(tournamentPlayer);
    await _context.SaveChangesAsync();
}

// Method to delete a TournamentPlayer by Id
public async Task DeleteSolicitude(int tournamentPlayerId)
{
    var tournamentPlayer = await _context.TournamentPlayers.FindAsync(tournamentPlayerId);
    _context.TournamentPlayers.Remove(tournamentPlayer);
    await _context.SaveChangesAsync();
}

    public async Task<IEnumerable<TournamentPlayer>> TournamentPlayersByTournament(int tournamentId)
    {
        var tournamentPlayers = await _context.TournamentPlayers
        .Where(tp => tp.TournamentId == tournamentId && tp.Status == EntityStatus.visible)
        .Include(tp => tp.Player)
        .Include(tp => tp.Deck)
        .Include(tp => tp.Tournament)
        .ToListAsync();
    return tournamentPlayers;
    }

    public async Task DeleteTournamentPlayers(ICollection<TournamentPlayer> tournamentPlayers)
    {
            // Remove the TournamentPlayers associated with the Tournament
        _context.TournamentPlayers.RemoveRange(tournamentPlayers);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TournamentPlayer>> TournamentPlayerWithDeckArchetype(int tournamentId, IEnumerable<int> playersIds)
    {
        var tournamentPlayers = await _context.TournamentPlayers
            .Where(tp => tp.TournamentId == tournamentId && playersIds.Contains(tp.PlayerId))
            .Include(tp => tp.Deck.Archetype)
            .ToListAsync();

        return tournamentPlayers;
    }

}