using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    
    [Required]
    public int PlayerAmount { get; set; }
    public virtual ICollection<Tournament_Player> TournamentPlayers { get; set; }
    
}
