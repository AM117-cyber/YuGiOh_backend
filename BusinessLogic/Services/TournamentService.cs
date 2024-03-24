using System.Globalization;

public class TournamentService: ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITournamentPlayerRepository _tournamentPlayerRepository;

    public TournamentService(ITournamentRepository tournamentRepository, ITournamentPlayerRepository tournamentPlayerRepository)
    {
        _tournamentRepository = tournamentRepository;
        _tournamentPlayerRepository = tournamentPlayerRepository;
    }

    public async Task<Tournament> CreateTournament(TournamentInDto tournament)
    {
        DateTime ParsedStartDate = DateTime.Parse(tournament.StartDate, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
    // Check that the StartDate is in the future
    if (ParsedStartDate <= DateTime.Now)
    {
        throw new ArgumentException("StartDate must be in the future.");
    }

    // Check that the number of rounds is between 1 and 6
    if (tournament.Rounds < 1 || tournament.Rounds > 6)
    {
        throw new ArgumentException("Rounds must be between 1 and 6.");
    }

    // Check that the number of players is greater than 10
    if (tournament.PlayerAmount <= 10)
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
            StartDate = ParsedStartDate.ToUniversalTime(),
            PlayerAmount = tournament.PlayerAmount,
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

    if (tournament.StartDate <= DateTime.Today)
    {
        throw new Exception("Tournament has already started and cannot be deleted");
    }
    await _tournamentPlayerRepository.DeleteTournamentPlayers(tournament.TournamentPlayers);
    await _tournamentRepository.DeleteTournament(tournament);
    }

    public async Task<Tournament> GetByName(string name)
    {
        return await _tournamentRepository.findByName(name);
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

    if (tournament.StartDate <= DateTime.Today)
    {
        throw new Exception("Tournament has already started and cannot be updated");
    }
    DateTime ParsedStartDate = DateTime.Parse(tournamentInDto.StartDate, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

    tournament.Name = tournamentInDto.Name;
    tournament.StartDate = ParsedStartDate.ToUniversalTime();
    tournament.Address = tournamentInDto.Address;
    tournament.Rounds = tournamentInDto.Rounds;
    tournament.PlayerAmount = tournamentInDto.PlayerAmount;

    await _tournamentRepository.UpdateTournament(tournament);
    return tournament;
}

    }


