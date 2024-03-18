// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Presentation.DTO;
// // PlayerController.cs
// // [ApiController]
// // [Route("api/players")]
// // public class PlayerController : ControllerBase
// // {
// //     private readonly PlayerService _playerService;

// //     public PlayerController(PlayerService playerService)
// //     {
// //         _playerService = playerService;
// //     }

// //     [HttpPost]
// //     public async Task<ActionResult<Player>> CreatePlayer(PlayerDTO playerDTO)
// //     {
// //         var player = await _playerService.CreatePlayer(playerDTO);
// //         return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
// //     }
// // }



// [Route("api/players")]
// [ApiController]
// public class MyController : ControllerBase
// {
//     private readonly ApplicationDbContext _context;

//     public MyController(ApplicationDbContext context)
//     {
//         _context = context;
//     }


// public class PlayerOutDto
// {
//     public int Id { get; set; }
//     public string Name { get; set; }
//     public string Address { get; set; }
//     public List<DeckDto> Decks { get; set; }
// }

// public class DeckDto
// {
//     public string Name { get; set; }
//     public int PlayerId { get; set; }
// }

// [HttpGet("getAll")]
// public async Task<ActionResult<IEnumerable<PlayerOutDto>>> GetPlayersWithDecks()
// {
//     var players = await _context.Players
//         .Include(p => p.Decks)
//         .Select(p => new PlayerOutDto
//         {
//             Id = p.Id,
//             Name = p.Name,
//             Address = p.Address,
//             Decks = p.Decks.Select(d => new DeckDto
//             {
//                 Name = d.Name,
//                 PlayerId = d.PlayerId
//             }).ToList()
//         })
//         .ToListAsync();

//     return players;
// }

//     // CRUD operations for Player


//     // GET: api/MyController/Players
//     [HttpGet("findAll")]
//     public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
//     {
//         return await _context.Players.ToListAsync();
//     }

//     // GET: api/MyController/Players/{id}
//     [HttpGet("Players/{id}")]
//     public async Task<ActionResult<Player>> GetPlayer(int id)
//     {
//         var player = await _context.Players.FindAsync(id);
//         if (player == null)
//         {
//             return NotFound();
//         }
//         return player;
//     }

//     // POST: api/MyController/Players
//     [HttpPost("Players")]
//     public async Task<ActionResult<Player>> PostPlayer(PlayerDTO player)
//     {
//         var _player = new Player
//         {
//             Name = player.Name,
//             MunicipalityId = 1,
//             Municipality = null,
//             Address = player.Address,
//             PhoneNumber = player.PhoneNumber,
//             Decks = [],
//             Money = 0
//         };
//         _context.Players.Add(_player);
//         await _context.SaveChangesAsync();
//         return CreatedAtAction(nameof(GetPlayer), new { id = _player.Id }, _player);
//     }

//     // PUT: api/MyController/Players/{id}
//     [HttpPut("Players/{id}")]
//     public async Task<IActionResult> PutPlayer(int id, Player player)
//     {
//         if (id != player.Id)
//         {
//             return BadRequest();
//         }
//         _context.Entry(player).State = EntityState.Modified;
//         try
//         {
//             await _context.SaveChangesAsync();
//         }
//         catch (DbUpdateConcurrencyException)
//         {
//             if (!_context.Players.Any(e => e.Id == id))
//             {
//                 return NotFound();
//             }
//             else
//             {
//                 throw;
//             }
//         }
//         return NoContent();
//     }

//     // DELETE: api/MyController/Players/{id}
//     [HttpDelete("Players/{id}")]
//     public async Task<IActionResult> DeletePlayer(int id)
//     {
//         var player = await _context.Players.FindAsync(id);
//         if (player == null)
//         {
//             return NotFound();
//         }
//         _context.Players.Remove(player);
//         await _context.SaveChangesAsync();
//         return NoContent();
//     }


// }
