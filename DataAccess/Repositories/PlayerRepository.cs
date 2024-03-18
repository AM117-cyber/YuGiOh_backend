
// // PlayerRepository.cs


// public class PlayerRepository
// {
//     private readonly ApplicationDbContext _context;

//     public PlayerRepository(ApplicationDbContext context)
//     {
//         _context = context;
//     }

//     public async Task<PlayerWorkflow.Player> CreatePlayer(PlayerWorkflow.Player player)
//     {
//         _context.Players.Add(player);
//         await _context.SaveChangesAsync();
//         return player;
//     }
// }