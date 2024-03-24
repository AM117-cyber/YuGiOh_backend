using System.Globalization;

public class TournamentPlayerService: ITournamentPlayerService
{
    private readonly ITournamentPlayerRepository _tournamentPlayerRepository;
    private readonly ITournamentRepository _tournamentRepository;

    public TournamentPlayerService(ITournamentPlayerRepository tournamentPlayerRepository,ITournamentRepository tournamentRepository)
    {
        _tournamentPlayerRepository = tournamentPlayerRepository;
        _tournamentRepository = tournamentRepository;
    }

    public async Task AcceptSolicitude(int tournamentPlayerId)
    {
        var tournamentPlayer = await _tournamentPlayerRepository.findById(tournamentPlayerId);
        await _tournamentPlayerRepository.AcceptSolicitude(tournamentPlayer);
    }

    public async Task<TournamentPlayerOutDto> CreateTournamentPlayer(TournamentPlayerInDto tournamentPlayer)
    {
        var existingTournamentPlayer = await _tournamentPlayerRepository.findTournamentPlayer(tournamentPlayer);

    if (existingTournamentPlayer)
    {
        throw new ArgumentException("Este jugador ya solicitó unirse a este torneo");
    }
        Tournament tournament = await _tournamentRepository.findById(tournamentPlayer.TournamentId);
        if (DateTime.Now >= tournament.StartDate)
        {
            throw new ArgumentException("El plazo de inscripción en este torneo caducó");
        }

        TournamentPlayer tournamentPlayerForRepository = new TournamentPlayer
        {
            PlayerId = tournamentPlayer.PlayerId,
            DeckId = tournamentPlayer.DeckId,
            TournamentId = tournamentPlayer.TournamentId,
            Status = EntityStatus.hidden
        };

        var tournamentPlayerFromRepository = await _tournamentPlayerRepository.Create(tournamentPlayerForRepository);
        var tournamentPlayerOutDto = await _tournamentPlayerRepository.GetTournamentOutDto(tournamentPlayerFromRepository.Id);
    return tournamentPlayerOutDto;
}

    public Task DeleteSolicitude(int tournamentPlayerId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TournamentPlayerOutDto>> GetPendantSolicitudes(int tournamentId)
    {
        var tournamentPlayers = await _tournamentPlayerRepository.GetHiddenTournamentPlayers(tournamentId);
        var tournamentPlayerOutDtos = tournamentPlayers.Select(tp => new TournamentPlayerOutDto
    {
        Id = tp.Id,
        PlayerId = tp.PlayerId,
        PlayerName = tp.Player.UserName,
        DeckId = tp.DeckId,
        DeckName = tp.Deck.Name,
        TournamentId = tp.TournamentId,
        TournamentName = tp.Tournament.Name
    }).ToList();
    return tournamentPlayerOutDtos;
    }

    public async Task<IEnumerable<TournamentPlayerOutDto>> GetTournamentPlayers(int tournamentId)
    {
        var tournamentPlayers = await _tournamentPlayerRepository.TournamentPlayersByTournament(tournamentId);
        var tournamentPlayerOutDtos = tournamentPlayers.Select(tp => new TournamentPlayerOutDto
    {
        Id = tp.Id,
        PlayerId = tp.PlayerId,
        PlayerName = tp.Player.UserName,
        DeckId = tp.DeckId,
        DeckName = tp.Deck.Name,
    }).ToList();
    return tournamentPlayerOutDtos;
    }
}
