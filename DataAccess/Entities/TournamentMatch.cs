using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TournamentMatch
{
    public int Id { get; set; }

    // Foreign keys for related player, tournament and deck
    public int Player1Id { get; set; }
    public int Player2Id { get; set; }
    public int TournamentId { get; set; }
    public required DateTime Date { get; set; }

    public int Player1Score { get; set; }
    public int Player2Score { get; set; }
    public int Round { get; set; }

    // Navigation properties for related player, tournament and deck
    [ForeignKey("Player1Id")]
    public virtual Player Player1 { get; set; }
    [ForeignKey("Player2Id")]
    public virtual Player Player2 { get; set; }
    [ForeignKey("TournamentId")]
    public virtual Tournament Tournament { get; set; }
}
