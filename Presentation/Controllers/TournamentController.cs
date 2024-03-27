using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("tournament")]
public class TournamentController : ControllerBase
{
    private readonly ITournamentService _tournamentService;
    private readonly IConfiguration _configuration;

    public TournamentController(ITournamentService tournamentService, IConfiguration configuration)
    {
        _tournamentService = tournamentService;
        _configuration = configuration;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTournament([FromBody] TournamentInDto tournament)
    {
        var createdTournament = await _tournamentService.CreateTournament(tournament);
        return Ok(createdTournament);
    }

    [HttpGet("getByName/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var tournament = await _tournamentService.GetByName(name);
        if (tournament == null)
        {
            return NotFound();
        }
        return Ok(tournament);
    }

    [HttpGet("getAdminTournaments/{AdminId}")]
    public async Task<IEnumerable<TournamentOutDto>> GetTournamentsByAdmin(int AdminId)
    {
        var tournaments = await _tournamentService.GetTournamentsByAdmin(AdminId);
        return tournaments;
    }
    
    [HttpPut("updateTournament/{tournament}/{id}")]
    public async Task<IActionResult> UpdateTournament(TournamentInDto tournament, int id)
    {
        var updatedTournament = await _tournamentService.UpdateTournament(tournament, id);
        return Ok(updatedTournament);
    }

    [HttpDelete("deleteTournament/{tournamentId}")]
    public async Task DeleteTournament(int tournamentId)
    {
        await _tournamentService.DeleteTournament(tournamentId);
    }

    
    [HttpGet("upcomingTournaments")]
    public async Task<IEnumerable<TournamentOutDto>> UpcomingTournaments()
    {
        var tournaments = await _tournamentService.GetUpcomingTournaments();
        return tournaments;
    }

    [HttpGet("allTournaments")]
    public async Task<IEnumerable<TournamentOutDto>> GetAllTournaments()
    {
        var tournaments = await _tournamentService.GetAllTournaments();
        return tournaments;
    }
}