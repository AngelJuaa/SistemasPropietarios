using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Brand
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BrandId { get; set; }
    public string Name { get; set; } = string.Empty;

    public int SupplierId { get; set; }

    [ForeignKey(nameof(SupplierId))]
    public virtual Supplier Supplier { get; set; } = null!;
}
