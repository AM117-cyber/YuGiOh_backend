using System.ComponentModel.DataAnnotations.Schema;

public class Tournament_Player
{
    public int Id { get; set; }

    // Foreign keys for related player, tournament and deck
    public int PlayerId { get; set; }
    public int TournamentId { get; set; }
    public int DeckId { get; set; }

    public EntityStatus Status{ get; set; }

    // Navigation properties for related player, tournament and deck
    [ForeignKey("PlayerId")]
    public virtual Player Player { get; set; }
    [ForeignKey("TournamentId")]
    public virtual Tournament Tournament { get; set; }
    [ForeignKey("DeckId")]
    public virtual Deck Deck { get; set; }
}
