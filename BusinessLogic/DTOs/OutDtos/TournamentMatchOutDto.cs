public class TournamentMatchOutDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Player1Score { get; set; }
    public int Player2Score { get; set; }
    public int Round { get; set; }
    public int Player1Id { get; set; }
    public string Player1Name { get; set; }
    public int Player2Id { get; set; }
    public string Player2Name { get; set; }
}