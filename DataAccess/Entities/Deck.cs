using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Deck
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public required string Name { get; set; }
    public required string Archetype { get; set; }

    public required int MainCards { get; set; }
    public required int ExtraCards { get; set; }
    public required int SideCards { get; set; }
    public EntityStatus MyStatus{ get; set; }
    public virtual ICollection<Tournament_Player> TournamentPlayers { get; set; }

    [ForeignKey("Player")]
    public int PlayerId { get; set; }

    public required Player Player { get; set; }

}