using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("tournamentMatch")]
public class TournamentMatchController : ControllerBase
{
    private readonly ITournamentMatchService _tournamentMatchService;
    private readonly IConfiguration _configuration;

    public TournamentMatchController(ITournamentMatchService tournamentMatchService, IConfiguration configuration)
    {
        _tournamentMatchService = tournamentMatchService;
        _configuration = configuration;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTournamentMatch([FromBody] TournamentMatchInDto tournamentMatch)
    {
        var createdTournamentMatch = await _tournamentMatchService.CreateTournamentMatch(tournamentMatch);
        return Ok(createdTournamentMatch);
    }

    [HttpGet("getByPlayers/{PlayersIds}")]
    public async Task<IActionResult> GetByPlayers([FromQuery] (int Player1Id, int Player2Id) PlayersIds)
    {
        var TournamentMatchOutDto = await _tournamentMatchService.GetByPlayers(PlayersIds.Player1Id, PlayersIds.Player2Id);
        return Ok(TournamentMatchOutDto);
    }
    
    [HttpPut("updateTournamentMatch/{tournamentMatch}")]
    public async Task<IActionResult> UpdateTournamentMatch(int matchId, int player1Score, int player2Score, string date)
    {
        var updatedTournamentMatch = await _tournamentMatchService.UpdateTournamentMatch(matchId, player1Score, player2Score, date);
        return Ok(updatedTournamentMatch);
    }

    [HttpGet("getRoundMatches/{tournamentMatch}")]
    public async Task<IEnumerable<TournamentMatchOutDto>> GetRoundMatches(int tournamentId, int round)
    {
        var matches = await _tournamentMatchService.GetRoundMatches(tournamentId, round);
        return matches;
    }

}
