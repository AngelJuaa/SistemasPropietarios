using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Models;

namespace Backend.Models;

public class Sale
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SaleId { get; set; }

    public int InventoryId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "date")]
    public DateTime SaleDate { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string ClientName { get; set; } = string.Empty;

    [ForeignKey(nameof(InventoryId))]
    public virtual Inventory Inventory { get; set; } = null!;
}
