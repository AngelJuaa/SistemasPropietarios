namespace backend.DTOs;

public class SupplierDto
{
    public int SupplierId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;
}
