using System.Globalization;

public class TournamentService: ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITournamentPlayerRepository _tournamentPlayerRepository;

    public TournamentService(ITournamentRepository tournamentRepository, ITournamentPlayerRepository tournamentPlayerRepository, IUserRepository userRepository)
    {
        _tournamentRepository = tournamentRepository;
        _tournamentPlayerRepository = tournamentPlayerRepository;
        _userRepository = userRepository;
    }

    public async Task<Tournament> CreateTournament(TournamentInDto tournament)
    {
        DateTimeOffset ParsedStartDate = DateTimeOffset.Parse(tournament.StartDate, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
    // Check that the StartDate is in the future
    if (ParsedStartDate.UtcDateTime <= DateTime.UtcNow)
    {
        throw new ArgumentException("StartDate must be in the future.");
    }

    // Check that the number of rounds is between 1 and 6
    if (tournament.Rounds < 1 || tournament.Rounds > 6)
    {
        throw new ArgumentException("Rounds must be between 1 and 6.");
    }

    // Check that the number of players is greater than 10
    if (tournament.PlayerAmount <= 7)
    {
        throw new ArgumentException("PlayerAmount must be greater than 10.");
    }

    // Check that the number of players is greater or equal to 2^Rounds
    if (tournament.PlayerAmount < Math.Pow(2, tournament.Rounds))
    {
        throw new ArgumentException("PlayerAmount must be greater or equal to 2^Rounds.");
    }

        Tournament tournamentForRepository = new Tournament
        {
            Address = tournament.Address,
            Name = tournament.Name,
            Rounds = tournament.Rounds,
            StartDate = ParsedStartDate.UtcDateTime,
            PlayerAmount = tournament.PlayerAmount,
            Status = TournamentStatus.pendant,
            PlayersSubscribed = 0,
            AdministrativeUserId = tournament.AdministrativeUserId
        };

        return await _tournamentRepository.Create(tournamentForRepository);
    }

    public async Task DeleteTournament(int tournamentId)
    {
        var tournament = await _tournamentRepository.findByIdWithPlayers(tournamentId);
    if (tournament == null)
    {
        throw new Exception("Tournament not found");
    }

    // if (tournament.Status == TournamentStatus.started)
    // {
    //     throw new Exception("Tournament has already started and cannot be deleted");
    // }
    await _tournamentPlayerRepository.DeleteTournamentPlayers(tournament.TournamentPlayers);
    await _tournamentRepository.DeleteTournament(tournament);
    }

    public async Task<IEnumerable<TournamentOutDto>> GetAllTournaments()
    {
        var tournaments = await _tournamentRepository.GetAllTournaments();
        var tournamentOutDtos = tournaments.Select(t => new TournamentOutDto 
    { 
        Name = t.Name, 
        Id = t.Id, 
        StartDate = t.StartDate,
        Rounds = t.Rounds,
        Status = t.Status,
        PlayersSubscribed = t.PlayersSubscribed,
        Address = t.Address,
        PlayerAmount = t.PlayerAmount
    }).ToList();

    return tournamentOutDtos;
    }

    public async Task<TournamentOutDto> GetByName(string name)
    {
        var tournament = await _tournamentRepository.findByName(name);
        var tournamentOutDto = new TournamentOutDto{
        Name = tournament.Name, 
        Id = tournament.Id, 
        StartDate = tournament.StartDate,
        Status = tournament.Status,
        PlayersSubscribed = tournament.PlayersSubscribed,
        Rounds = tournament.Rounds,
        Address = tournament.Address,
        PlayerAmount = tournament.PlayerAmount
        };
        return tournamentOutDto;
    }

    public async Task<IEnumerable<TournamentOutDto>> GetTournamentsByAdmin(int AdminId)
    {
        var tournaments = await _tournamentRepository.GetTournamentsByAdmin(AdminId);
        var tournamentOutDtos = tournaments.Select(t => new TournamentOutDto 
    { 
        Name = t.Name, 
        Id = t.Id, 
        StartDate = t.StartDate,
        Rounds = t.Rounds,
        Status = t.Status,
        PlayersSubscribed = t.PlayersSubscribed,
        Address = t.Address,
        PlayerAmount = t.PlayerAmount
    }).ToList();

    return tournamentOutDtos;
    }

    public async Task<IEnumerable<TournamentOutDto>> GetUpcomingTournaments()
    {
        var tournaments = await _tournamentRepository.GetUpcomingTournaments();
        var tournamentOutDtos = tournaments.Select(t => new TournamentOutDto 
    { 
        Name = t.Name, 
        Id = t.Id, 
        StartDate = t.StartDate,
        PlayersSubscribed = t.PlayersSubscribed,
        Rounds = t.Rounds,
        Address = t.Address,
        PlayerAmount = t.PlayerAmount
    }).ToList();

    return tournamentOutDtos;
    }

    public async Task<IEnumerable<TournamentOutDto>> GetStartedTournaments()
    {
        var tournaments = await _tournamentRepository.GetStartedTournaments();
        var tournamentOutDtos = tournaments.Select(t => new TournamentOutDto 
    { 
        Name = t.Name, 
        Id = t.Id, 
        StartDate = t.StartDate,
        Rounds = t.Rounds,
        Address = t.Address,
        PlayerAmount = t.PlayerAmount
    }).ToList();

    return tournamentOutDtos;
    }

    public async Task<Tournament> UpdateTournament(TournamentInDto tournamentInDto, int Id)
    {
        var tournament = await _tournamentRepository.findById(Id);

    if (tournament == null)
    {
        throw new Exception("Tournament not found");
    }

    if (tournament.Status == TournamentStatus.started)
    {
        throw new Exception("Tournament has already started and cannot be updated");
    }
    DateTimeOffset ParsedStartDate = DateTimeOffset.Parse(tournamentInDto.StartDate, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

    tournament.Name = tournamentInDto.Name;
    tournament.StartDate = ParsedStartDate.UtcDateTime;
    tournament.Address = tournamentInDto.Address;
    tournament.Rounds = tournamentInDto.Rounds;
    tournament.PlayerAmount = tournamentInDto.PlayerAmount;

    await _tournamentRepository.UpdateTournament(tournament);
    return tournament;
}

    public async Task<TournamentPlayerOutDto> GetTournamentChampion(int tournamentId)
    {
        var tournament = await _tournamentRepository.findByIdWithMatches(tournamentId);

        var finalRound = 1;
        var finalMatch = tournament.TournamentMatches
            .FirstOrDefault(tm => tm.Round == finalRound);

        if (finalMatch == null)
            {
             throw new ArgumentException("El torneo no ha finalizado o no tiene los datos de todas sus partidas.");
            }

    // The player with the highest score in the final match is the champion
    int championId = finalMatch.Player1Score > finalMatch.Player2Score
        ? finalMatch.Player1Id
        : finalMatch.Player2Id;

    var tp = await _tournamentPlayerRepository.findByIdWithPlayerAndDeck(championId);
    TournamentPlayerOutDto champion = new TournamentPlayerOutDto{
        Id = tp.Id,
        PlayerId = tp.PlayerId,
        PlayerName = tp.Player.UserName,
        DeckId = tp.DeckId,
        DeckName = tp.Deck.Name,
        TournamentId = tp.TournamentId,
        TournamentName = tp.Tournament.Name
    };

    return champion;
}

    public async Task<IEnumerable<(string, int)>> winnerArchetypes(IEnumerable<int> tournamentsIds, DateTime startDate, DateTime endDate)
    {
    // Get the tournaments that started within the given period
    List<TournamentPlayer> champions = [];
    foreach (var tournamentId in tournamentsIds)
    {
        var tournament = await _tournamentRepository.findByIdWithMatches(tournamentId);
        var finalRound = 1;
        var finalMatch = tournament.TournamentMatches
            .FirstOrDefault(tm => tm.Round == finalRound);

        if (finalMatch == null || finalMatch.Date < startDate || finalMatch.Date > endDate)
            {
             throw new ArgumentException("El torneo no declaró su ganador en el período de tiempo especificado finalizado.");
            }

    // The player with the highest score in the final match is the champion
    int championId = finalMatch.Player1Score > finalMatch.Player2Score
        ? finalMatch.Player1Id
        : finalMatch.Player2Id;

    var tournamentChampion = await _tournamentPlayerRepository.findByIdWithPlayerAndDeck(championId);
    champions.Add(tournamentChampion);
    }
    
    var result = champions
        .GroupBy(champs => champs.Deck.Archetype) // Group by the archetype of the deck
        .Select(g => (Archetype: g.Key, Count: g.Count())) // Select the archetype and the count
        .OrderByDescending(t => t.Count); // Order by count descending

    return result;
}

    public async Task<IEnumerable<TournamentOutDto>> GetTournamentsAwaitingConfirmation()
    {
        var tournaments = await _tournamentRepository.GetTournamentsAwaitingConfirmation();
        var tournamentOutDtos = tournaments.Select(t => new TournamentOutDto 
    { 
        Name = t.Name, 
        Id = t.Id, 
        StartDate = t.StartDate,
        Rounds = t.Rounds,
        Status = t.Status,
        PlayersSubscribed = t.PlayersSubscribed,
        Address = t.Address,
        PlayerAmount = t.PlayerAmount
    }).ToList();

    return tournamentOutDtos;
    
    }

    public async Task<bool> ConfirmTournamentStart(int tournamentId)
    {
        var tournament = await _tournamentRepository.findById(tournamentId);
        if (tournament.StartDate < DateTime.UtcNow && tournament.PlayersSubscribed == tournament.PlayerAmount)
        {
            tournament.Status = TournamentStatus.started;
            await _tournamentRepository.UpdateTournament(tournament);
            return true;
        }
        throw new ArgumentException("El torneo no es válido para comenzar");
    }
}


