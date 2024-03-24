using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("tournamentplayer")]
public class TournamentPlayerController : ControllerBase
{
    private readonly ITournamentPlayerService _tournamentPlayerService;
    private readonly IConfiguration _configuration;

    public TournamentPlayerController(ITournamentPlayerService tournamentPlayerService, IConfiguration configuration)
    {
        _tournamentPlayerService = tournamentPlayerService;
        _configuration = configuration;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTournamentPlayer([FromBody] TournamentPlayerInDto tournamentPlayer)
    {
        var createdTournamentPlayer = await _tournamentPlayerService.CreateTournamentPlayer(tournamentPlayer);
        return Ok(createdTournamentPlayer);
    }

    [HttpGet("getPendantSolicitudes/{tournamentId}")]
    public async Task<IActionResult> GetPendantSolicitudes (int tournamentId)
    {
        var result = await _tournamentPlayerService.GetPendantSolicitudes(tournamentId);
        return Ok(result);
    }

    [HttpGet("getTournamentPlayers/{tournamentId}")]
    public async Task<IActionResult> GetTournamentPlayers (int tournamentId)
    {
        var result = await _tournamentPlayerService.GetTournamentPlayers(tournamentId);
        return Ok(result);
    }

    [HttpPut("acceptSolicitude/{tournamentPlayerId}")]
    public async Task AcceptSolicitude(int tournamentPlayerId)
    {
        await _tournamentPlayerService.AcceptSolicitude(tournamentPlayerId);
    }
    
    [HttpDelete("deleteSolicitude/{tournamentPlayerId}")]
    public async Task DeleteSolicitude(int tournamentPlayerId)
    {
        await _tournamentPlayerService.DeleteSolicitude(tournamentPlayerId);
    }
    
}