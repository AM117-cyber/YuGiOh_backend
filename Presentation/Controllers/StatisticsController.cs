using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IDeckService _deckService;
    private readonly ITournamentService _tournamentService;
    private readonly ITournamentPlayerService _tournamentPlayerService;
    private readonly ITournamentMatchService _tournamentMatchService;
    private readonly IConfiguration _configuration;

    public StatisticsController(IUserService userService, IDeckService deckService, ITournamentPlayerService tournamentPlayerService, ITournamentService tournamentService, ITournamentMatchService tournamentMatchService, IConfiguration configuration)
    {
        _userService = userService;
        _deckService = deckService;
        _tournamentService = tournamentService;
        _tournamentPlayerService = tournamentPlayerService;
        _tournamentMatchService = tournamentMatchService;
        _configuration = configuration;
    }

    [HttpGet("playersDeckCount")]
    public async Task<IEnumerable<PlayerDeckCountDto>> GetPlayersDeckCount()
    {
        return await _userService.GetPlayersDeckCount();
    }

    [HttpGet("deckArchetypeCount")]
    public async Task<IEnumerable<DeckArchetypeCountDto>> GetDeckArchetypeCount()
    {
        return await _deckService.GetDeckArchetypeCount();
    }

    [HttpGet("mostPopularProvinceForArchetype/{archetype}")]
    public async Task<(IEnumerable<string>, int)> MostPopularProvinceForArchetype(string archetype)
    {
        return await _userService.MostPopularProvinceForArchetype(archetype);
    }

    [HttpGet("mostPopularMunicipalityForArchetype/{archetype}")]
    public async Task<(MunicipalityOutDto, int)> MostPopularMunicipalityForArchetype(string archetype)
    {
        return await _userService.MostPopularMunicipalityForArchetype(archetype);
        
    }

    [HttpGet("getChampion/{tournamentId}")]
    public async Task<TournamentPlayerOutDto> GetTournamentChampion(int tournamentId)
    {
        return await _tournamentService.GetTournamentChampion(tournamentId);
    }

    [HttpGet("getPlayersWithVictories")]
    public async Task<IEnumerable<(string,int)>> getPlayersWithVictories(DateTime startDate, DateTime endDate)
    {
        return await _tournamentMatchService.getPlayersWithVictories(startDate, endDate);
    }

    [HttpGet("mostPopularArchetypeInTournament/{tournamentId}")]
    public async Task<(IEnumerable<string>, int)> mostPopularArchetypeInTournament(int tournamentId)
    {
        return await _tournamentPlayerService.mostPopularArchetypeInTournament(tournamentId);
    }

    [HttpGet("winnerArchetypes/{tournamentsIds}")]
    public async Task<IEnumerable<(string,int)>> winnerArchetypes(IEnumerable<int> tournamentsIds, DateTime startDate, DateTime endDate)
    {
        return await _tournamentService.winnerArchetypes(tournamentsIds, startDate, endDate);
    }

    [HttpGet("mostWinnersProvince")]
    public async Task<(IEnumerable<string>, int)> mostWinnersProvince(DateTime startDate, DateTime endDate)
    {
        return await _tournamentPlayerService.mostWinnersProvince(startDate, endDate);
    }

    [HttpGet("mostWinnersMunicipality")]
    public async Task<(IEnumerable<MunicipalityOutDto>, int)> mostWinnersMunicipality(DateTime startDate, DateTime endDate)
    {
        return await _tournamentPlayerService.mostWinnersMunicipality(startDate, endDate);
    }

    [HttpGet("mostPopularArchetypeInTournamentRound/{tournamentId}/{round}")]
    public async Task<IEnumerable<(string,int)>> mostPopularArchetypeInTournamentRound(int tournamentId, int round)
    {
        return await _tournamentMatchService.mostPopularArchetypeInTournamentRound(tournamentId, round);
    }

    [HttpGet("mostPopularArchetypes")]
    public async Task<IEnumerable<(string,int)>> mostPopularArchetypes()
    {
        return await _tournamentPlayerService.mostPopularArchetypes();
    }


}