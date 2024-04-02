using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Microsoft.AspNetCore.Identity;
public class Player : IdentityUser<int>
{
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // public int Id { get; set; }
    // public required string Name { get; set; }
    public required string Address { get; set; }
    public double Money { get; set; }
    public virtual ICollection<Deck> Decks { get; set; }
    [InverseProperty("Player")]
    public virtual ICollection<TournamentPlayer> TournamentPlayers { get; set; }

    [ForeignKey("Municipality")]
    public int MunicipalityId { get; set; }

    public virtual Municipality Municipality { get; set; }
    
    [InverseProperty("Player1")]
    public virtual ICollection<TournamentMatch> Player1Matches { get; set; }
    [InverseProperty("Player2")]
    public virtual ICollection<TournamentMatch> Player2Matches { get; set; }
}
