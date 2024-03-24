using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("deck")]
public class DeckController : ControllerBase
{
    private readonly IDeckService _deckService;
    private readonly IConfiguration _configuration;

    public DeckController(IDeckService deckService, IConfiguration configuration)
    {
        _deckService = deckService;
        _configuration = configuration;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateDeck([FromBody] DeckInDto deck)
    {
        var createdDeck = await _deckService.CreateDeck(deck);
        return Ok(createdDeck);
    }

    [HttpGet("getByName")]
    public async Task<IActionResult> GetByName([FromQuery] string name)
    {
        var deck = await _deckService.GetByName(name);
        if (deck == null)
        {
            return NotFound();
        }
        return Ok(deck);
    }
    
}