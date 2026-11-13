using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Supplier
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SupplierId { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [MinLength(2)]
    [Column(TypeName = "varchar(50)")]
    public string Country { get; set; } = string.Empty;

    [Required]
    [MinLength(7)]
    [MaxLength(15)]
    [Column(TypeName = "varchar(15)")]
    public string Phone { get; set; } = string.Empty;

    public virtual ICollection<Brand> Brands { get; set; } = new List<Brand>();
}
