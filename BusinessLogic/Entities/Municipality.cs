using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Municipality
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public required string Name { get; set; }

    [ForeignKey("Province")]
    public int ProvinceId { get; set; }

    public virtual Province Province { get; set; }

    [InverseProperty("Municipality")]
    public virtual ICollection<Player> Players { get; set; }
}