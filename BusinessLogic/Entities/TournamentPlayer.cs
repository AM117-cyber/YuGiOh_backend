using System.ComponentModel.DataAnnotations.Schema;

public class TournamentPlayer
{
    public int Id { get; set; }
    public EntityStatus Status{ get; set; }

    // Navigation properties for related player, tournament and deck
    [ForeignKey("Player")]
    public int PlayerId { get; set; }
    public virtual Player Player { get; set; }
    [ForeignKey("Tournament")]
    public int TournamentId { get; set; }
    public virtual Tournament Tournament { get; set; }
    [ForeignKey("Deck")]
     public int DeckId { get; set; }
    public virtual Deck Deck { get; set; }
}
