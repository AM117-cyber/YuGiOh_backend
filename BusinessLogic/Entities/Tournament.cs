using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

public class Tournament
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Address { get; set; }

    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public int Rounds { get; set; }
    public TournamentStatus Status {get; set; }
    public int PlayersSubscribed {get; set; }
    
    [Required]
    public int PlayerAmount { get; set; }
    [ForeignKey("AdminId")]
    public int AdministrativeUserId { get; set; }
    public virtual AdministrativeUser Administrator { get; set; }
    [InverseProperty("Tournament")]
    public virtual ICollection<TournamentPlayer> TournamentPlayers { get; set; }
    [InverseProperty("Tournament")]
    public virtual ICollection<TournamentMatch> TournamentMatches { get; set; }
    
}
