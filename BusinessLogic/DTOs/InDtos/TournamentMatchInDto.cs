public class TournamentMatchInDto
{
    public required string Date { get; set; }
    public int Player1Score { get; set; }
    public int Player2Score { get; set; }
    public int Round { get; set; }
    public int Player1Id { get; set; }
    public int Player2Id { get; set; }
    public int TournamentId { get; set; }
}