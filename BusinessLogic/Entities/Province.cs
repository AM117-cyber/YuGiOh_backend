using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Province
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProvinceId { get; set; }

    [Required]
    public string ProvinceName { get; set; }

    [InverseProperty("Province")]
    public virtual ICollection<Municipality> Municipalities { get; set; }
}

