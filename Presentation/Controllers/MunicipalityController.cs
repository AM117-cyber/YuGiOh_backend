using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("municipality")]
public class MunicipalityController : ControllerBase
{
    private readonly IMunicipalityService _municipalityService;
    private readonly IConfiguration _configuration;

    public MunicipalityController(IMunicipalityService municipalityService, IConfiguration configuration)
    {
        _municipalityService = municipalityService;
        _configuration = configuration;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateMunicipality([FromBody] Municipality municipality)
    {
        var createdMunicipality = await _municipalityService.CreateMunicipality(municipality);
        return Ok(createdMunicipality);
    }

    [HttpGet("getByName")]
    public async Task<IActionResult> GetByName([FromQuery] string name)
    {
        var municipality = await _municipalityService.GetByName(name);
        if (municipality == null)
        {
            return NotFound();
        }
        return Ok(municipality);
    }
    
}