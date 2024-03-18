// // PlayerService.cs
// using Presentation;

// public class PlayerService
// {
//     private readonly PlayerRepository _playerRepository;
//     private readonly MunicipalityRepository _municipalityRepository;

//     public PlayerService(PlayerRepository playerRepository, MunicipalityRepository municipalityRepository)
//     {
//         _playerRepository = playerRepository;
//         _municipalityRepository = municipalityRepository;
//     }

//     public async Task<Player> CreatePlayer(PlayerDTO playerDTO)
//     {
//         var municipality = await _municipalityRepository.GetMunicipalityByNameAndProvince(playerDTO.Municipality, playerDTO.Province);
//         if (municipality == null)
//         {
//             throw new Exception("Municipality does not belong to the provided province");
//         }

//         var player = new Player
//         {
//             Name = playerDTO.Name,
//             MunicipalityId = municipality.Id,
//             Municipality = municipality,
//             Address = playerDTO.Address,
//             PhoneNumber = playerDTO.PhoneNumber,
//             Decks = [],
//             Money = 0
//         };

//         return await _playerRepository.CreatePlayer(player);
//     }
// }
