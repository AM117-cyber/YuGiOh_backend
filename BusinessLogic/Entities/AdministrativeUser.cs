using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Microsoft.AspNetCore.Identity;
public class AdministrativeUser : IdentityUser<int>
{
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // public int Id { get; set; }
    // public required string Name { get; set; }
    [InverseProperty("Administrator")]
    public virtual ICollection<Tournament> TournamentsCreated { get; set; }
}
