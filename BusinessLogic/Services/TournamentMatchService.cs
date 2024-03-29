
using System.Globalization;

public class TournamentMatchService: ITournamentMatchService
{
    private readonly ITournamentMatchRepository _tournamentMatchRepository;
    private readonly ITournamentRepository _tournamentRepository;

    public TournamentMatchService(ITournamentMatchRepository tournamentMatchRepository,ITournamentRepository tournamentRepository)
    {
        _tournamentMatchRepository = tournamentMatchRepository;
        _tournamentRepository = tournamentRepository;
    }

    public async Task<TournamentMatchOutDto> CreateTournamentMatch(TournamentMatchInDto tournamentMatch)
    {
        if ((tournamentMatch.Player1Score + tournamentMatch.Player2Score) > 3)
        {
            throw new ArgumentException("Impossible score");
        }
        DateTime date = DateTime.Parse(tournamentMatch.Date, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        var newTournamentMatch = new TournamentMatch
        {
            Player1Id = tournamentMatch.Player1Id,
            Player1Score = tournamentMatch.Player1Score,
            Player2Id = tournamentMatch.Player2Id,
            Player2Score = tournamentMatch.Player2Score,
            Round = tournamentMatch.Round,
            TournamentId = tournamentMatch.TournamentId,
            Date = date
        };
        var t = await _tournamentMatchRepository.CreateTournamentMatch(newTournamentMatch);
        var result = new TournamentMatchOutDto
        {
            Player1Id = t.Player1Id,
            Player1Score = t.Player1Score,
            Player2Id = t.Player2Id,
            Player2Score = t.Player2Score,
            Round = t.Round,
            Id = t.Id,
            Date = date
        };
        return result;

    }

    public async Task<TournamentMatchOutDto> GetByPlayers(int player1Id, int player2Id)
    {
        var tournamentMatch =  await _tournamentMatchRepository.GetByPlayers(player1Id, player2Id);
        var result = new TournamentMatchOutDto
        {
            Player1Id = tournamentMatch.Player1Id,
            Player1Score = tournamentMatch.Player1Score,
            Player2Id = tournamentMatch.Player2Id,
            Player2Score = tournamentMatch.Player2Score,
            Round = tournamentMatch.Round,
            Date = tournamentMatch.Date,
            Id = tournamentMatch.Id
        };
        return result;        
    }

    public async Task<IEnumerable<TournamentMatchOutDto>> GetRoundMatches(int tournamentId, int round)
    {
        var matches = await _tournamentMatchRepository.GetRoundMatches(tournamentId, round);
        var result = matches.Select(tp => new TournamentMatchOutDto
    {
            Player1Id = tp.Player1Id,
            Player1Score = tp.Player1Score,
            Player2Id = tp.Player2Id,
            Player2Score = tp.Player2Score,
            Round = tp.Round,
            Date = tp.Date,
            Id = tp.Id
    }).ToList();
    return result;
    }

    public async Task<IEnumerable<TournamentMatchOutDto>> GetTournamentMatches(int tournamentId)
    {
        var matches = await _tournamentMatchRepository.GetTournamentMatches(tournamentId);
        var result = matches.Select(tp => new TournamentMatchOutDto
    {
            Player1Id = tp.Player1Id,
            Player1Score = tp.Player1Score,
            Player2Id = tp.Player2Id,
            Player2Score = tp.Player2Score,
            Round = tp.Round,
            Date = tp.Date,
            Id = tp.Id
    }).ToList();
    return result;
    }

    public async Task<TournamentMatchOutDto> UpdateTournamentMatch(int matchId, int player1Score, int player2Score, string date)
    {
        DateTime ParsedDate = DateTime.Parse(date, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

        var tournamentMatch = await _tournamentMatchRepository.findById(matchId);
        tournamentMatch.Player1Score = player1Score;
        tournamentMatch.Player2Score = player2Score;
        tournamentMatch.Date = ParsedDate.ToUniversalTime();
        await _tournamentMatchRepository.UpdateTournamentMatch(tournamentMatch);
        var result = new TournamentMatchOutDto 
        {
            Player1Id = tournamentMatch.Player1Id,
            Player1Score = tournamentMatch.Player1Score,
            Player2Id = tournamentMatch.Player2Id,
            Player2Score = tournamentMatch.Player2Score,
            Round = tournamentMatch.Round,
            Date = tournamentMatch.Date,
            Id = tournamentMatch.Id
        };
        return result;
    }
}