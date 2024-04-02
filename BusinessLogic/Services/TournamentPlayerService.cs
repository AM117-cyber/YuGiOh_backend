using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Query.Internal;

public class TournamentPlayerService: ITournamentPlayerService
{
    private readonly ITournamentPlayerRepository _tournamentPlayerRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IUserRepository _userRepository;

    public TournamentPlayerService(ITournamentPlayerRepository tournamentPlayerRepository,ITournamentRepository tournamentRepository, IUserRepository userRepository)
    {
        _tournamentPlayerRepository = tournamentPlayerRepository;
        _tournamentRepository = tournamentRepository;
        _userRepository = userRepository;
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
        if (DateTime.UtcNow >= tournament.StartDate)
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
        var tournamentPlayerForDto = await _tournamentPlayerRepository.GetTournamentPlayer(tournamentPlayerFromRepository.Id);
        if (tournamentPlayer == null)
    {
        throw new Exception("TournamentPlayer not found");
    }

    var tournamentPlayerOutDto = new TournamentPlayerOutDto
    {
        PlayerId = tournamentPlayerForDto.PlayerId,
        PlayerName = tournamentPlayerForDto.Player.UserName,
        DeckId = tournamentPlayerForDto.DeckId,
        DeckName = tournamentPlayerForDto.Deck.Name,
        TournamentId = tournamentPlayerForDto.TournamentId,
        TournamentName = tournamentPlayerForDto.Tournament.Name
    };
    return tournamentPlayerOutDto;
}

    public async Task DeleteSolicitude(int tournamentPlayerId)
    {
        await _tournamentPlayerRepository.DeleteSolicitude(tournamentPlayerId);
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

    public async Task<(IEnumerable<string>, int)> mostPopularArchetypeInTournament(int tournamentId)
    {
        var tournamentPlayers = await _tournamentPlayerRepository.TournamentPlayersByTournament(tournamentId);
        var archetypes = tournamentPlayers
            .GroupBy(tp => tp.Deck.Archetype)
            .Select(tp => (Archetype: tp.Key, Count: tp.Count()))
            .OrderByDescending(t => t.Count);
        int maxCount = archetypes.First().Count;
        var result = archetypes
            .Where(a => a.Count == maxCount)
            .Select(tp => tp.Archetype);
        return (result, maxCount);
    }

    public async Task<IEnumerable<(string, int)>> mostPopularArchetypes()
    {
        var tournamentPlayers = await _tournamentPlayerRepository.AllTournamentPlayersWithArchetypes();
        var archetypes = tournamentPlayers
            .GroupBy(tp => tp.Deck.Archetype)
            .Select(tp => (Archetype: tp.Key, Count: tp.Count()))
            .OrderByDescending(t => t.Count);
        return archetypes;
    }

    public async Task<(IEnumerable<MunicipalityOutDto>, int)> mostWinnersMunicipality(DateTime startDate, DateTime endDate)
    {
        var allTournaments = await _tournamentRepository.GetStartedTournamentsWithMatches();
        //get all winners
            List<int> championsIds = [];
            foreach (var tournament in allTournaments)
            {
                var finalRound = 1;
                var finalMatch = tournament.TournamentMatches
                    .FirstOrDefault(tm => tm.Round == finalRound);

                if (finalMatch == null || finalMatch.Date < startDate || finalMatch.Date > endDate)
                    {
                        continue;
                        //throw new ArgumentException("El torneo no declaró su ganador en el período de tiempo especificado finalizado.");
                    }

                // The player with the highest score in the final match is the champion
                int championId = finalMatch.Player1Score > finalMatch.Player2Score
                    ? finalMatch.Player1Id
                    : finalMatch.Player2Id;
                championsIds.Add(championId);
            }

            var municipalities = await _userRepository.GetPlayersMunicipalities(championsIds);
            var result = municipalities
                .GroupBy(m => new { m.Name, m.Province.ProvinceName })
                .Select(g => ( Name: g.Key.Name, ProvinceName: g.Key.ProvinceName, Count: g.Count() ))
                .OrderByDescending(t => t.Count);

            var maxCount = result.First().Count;
            var mostPopularMunicipalities = result
                    .Where(m => m.Count == maxCount)
                    .Select(m => new MunicipalityOutDto { Name = m.Name, ProvinceName = m.ProvinceName });
        //get all players with municipality that are in the list of winners, group by municipality, get maxCount and return those that have it
        return (mostPopularMunicipalities, maxCount);
    }

    public async Task<(IEnumerable<string>, int)> mostWinnersProvince(DateTime startDate, DateTime endDate)
    {
        var allTournaments = await _tournamentRepository.GetStartedTournamentsWithMatches();
        //get all winners
            List<int> championsIds = [];
            foreach (var tournament in allTournaments)
            {
                var finalRound = 1;
                var finalMatch = tournament.TournamentMatches
                    .FirstOrDefault(tm => tm.Round == finalRound);

                if (finalMatch == null || finalMatch.Date < startDate || finalMatch.Date > endDate)
                    {
                        continue;
                        //throw new ArgumentException("El torneo no declaró su ganador en el período de tiempo especificado finalizado.");
                    }

                // The player with the highest score in the final match is the champion
                int championId = finalMatch.Player1Score > finalMatch.Player2Score
                    ? finalMatch.Player1Id
                    : finalMatch.Player2Id;
                championsIds.Add(championId);
            }

            var municipalities = await _userRepository.GetPlayersMunicipalities(championsIds);
            var result = municipalities
                .GroupBy(m => m.Province.ProvinceName)
                .Select(g => (Name: g.Key, Count: g.Count() ))
                .OrderByDescending(t => t.Count);

            var maxCount = result.First().Count;
            var mostPopularProvinces = result
                    .Where(m => m.Count == maxCount)
                    .Select(m => m.Name);
        //get all players with municipality that are in the list of winners, group by municipality, get maxCount and return those that have it
        return (mostPopularProvinces, maxCount);
    }
}
