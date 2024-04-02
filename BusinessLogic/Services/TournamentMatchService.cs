
using System.Globalization;

public class TournamentMatchService: ITournamentMatchService
{
    private readonly ITournamentMatchRepository _tournamentMatchRepository;
    private readonly ITournamentPlayerRepository _tournamentPlayerRepository;
    private readonly ITournamentRepository _tournamentRepository;

    public TournamentMatchService(ITournamentMatchRepository tournamentMatchRepository, ITournamentPlayerRepository tournamentPlayerRepository, ITournamentRepository tournamentRepository)
    {
        _tournamentMatchRepository = tournamentMatchRepository;
        _tournamentPlayerRepository = tournamentPlayerRepository;
        _tournamentRepository = tournamentRepository;
    }

    public async Task<TournamentMatchOutDto> CreateTournamentMatch(TournamentMatchInDto tournamentMatch)
    {
        if ((tournamentMatch.Player1Score + tournamentMatch.Player2Score) > 3)
        {
            throw new ArgumentException("Impossible score");
        }
        DateTimeOffset date = DateTimeOffset.Parse(tournamentMatch.Date, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        var newTournamentMatch = new TournamentMatch
        {
            Player1Id = tournamentMatch.Player1Id,
            Player1Score = tournamentMatch.Player1Score,
            Player2Id = tournamentMatch.Player2Id,
            Player2Score = tournamentMatch.Player2Score,
            Round = tournamentMatch.Round,
            TournamentId = tournamentMatch.TournamentId,
            Date = date.UtcDateTime,
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
            Date = date.UtcDateTime,
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
        var matches = await _tournamentMatchRepository.GetTournamentMatchesOtherThan0(tournamentId);
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
        DateTimeOffset ParsedDate = DateTimeOffset.Parse(date, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

        var tournamentMatch = await _tournamentMatchRepository.findById(matchId);
        tournamentMatch.Player1Score = player1Score;
        tournamentMatch.Player2Score = player2Score;
        tournamentMatch.Date = ParsedDate.UtcDateTime;
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

    public async Task<IEnumerable<(string, int)>> getPlayersWithVictories(DateTime startDate, DateTime endDate)
{
    var matches = await _tournamentMatchRepository.GetMatchesWithPlayersNames(startDate, endDate);
    var result = matches
        .GroupBy(m => (m.Player1Score > m.Player2Score) ? m.Player1.UserName : m.Player2.UserName) // Group by the winner's username
        .Select(g => (UserName: g.Key, Count: g.Count())) // Select the username and the count
        .OrderByDescending(t => t.Count); // Order by count descending

    return result;
}

    public async Task<IEnumerable<(string, int)>> mostPopularArchetypeInTournamentRound(int tournamentId, int round)
    {
        var matches = await _tournamentMatchRepository.GetRoundMatches(tournamentId,round);
        var playersIds = matches
            .SelectMany(m => new[] { m.Player1.Id, m.Player2.Id }) // Select the Ids of both players in each match
            .Distinct(); // Remove duplicates
        IEnumerable<TournamentPlayer> tournamentPlayers = await _tournamentPlayerRepository.TournamentPlayerWithDeckArchetype(tournamentId, playersIds);    
        var result = tournamentPlayers
            .GroupBy(tp => tp.Deck)
            .Select(g => (Deck: g.Key, Count: g.Count()))
            .OrderByDescending(t => t.Count);
            List<(string,int)> answer = [];
            foreach (var item in result)
            {
                answer.Add((item.Deck.Archetype, item.Count));
            }
        return answer;
}

    public async Task<(IEnumerable<TournamentMatchOutDto> otherRounds, IEnumerable<TournamentMatchOutDto> RoundCero)> GetAllMatches(int tournamentId)
    {
       var otherRoundMatches = await _tournamentMatchRepository.GetTournamentMatchesOtherThan0(tournamentId);
        var otherRounds = otherRoundMatches.Select(tp => new TournamentMatchOutDto
    {
            Player1Id = tp.Player1Id,
            Player1Score = tp.Player1Score,
            Player2Id = tp.Player2Id,
            Player2Score = tp.Player2Score,
            Round = tp.Round,
            Date = tp.Date,
            Id = tp.Id
    }).ToList();

    var roundCeroMatches = await _tournamentMatchRepository.GetRoundMatches(tournamentId, 0);
        var roundCero = roundCeroMatches.Select(tp => new TournamentMatchOutDto
    {
            Player1Id = tp.Player1Id,
            Player1Score = tp.Player1Score,
            Player2Id = tp.Player2Id,
            Player2Score = tp.Player2Score,
            Round = tp.Round,
            Date = tp.Date,
            Id = tp.Id
    }).ToList();

    return (otherRounds, roundCero);
    }
}