namespace API.Models;

public class CreateMaterialItemRequest
{
    public string Kind { get; set; } = string.Empty; // "equipment" | "furniture" | "software"
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime ReceivedDate { get; set; }
    public int State { get; set; } // enum value as int (0 Operational, 1 WrittenOff, 2 UnderRepair)

    // Equipment fields (optional)
    public string? SerialNumber { get; set; }
    public string? Location { get; set; }
    public string? TechnicalCharacteristics { get; set; }
    public string? ClassroomNumber { get; set; }

    // Furniture fields (optional)
    public string? Type { get; set; }
    public string? Material { get; set; }

    // Software fields (optional)
    public string? Version { get; set; }
    public string? License { get; set; }
    public DateTime? LicenseExpirationDate { get; set; }
}

