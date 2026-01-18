namespace popasu.Client.Models;

public class MaterialItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime ReceivedDate { get; set; }
    public State State { get; set; }
    public string ItemType { get; set; } = string.Empty; // Discriminator from TPH
}

public class EquipmentDto : MaterialItemDto
{
    public string SerialNumber { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string TechnicalCharacteristics { get; set; } = string.Empty;
    public string? ClassroomNumber { get; set; }
}

public class FurnitureDto : MaterialItemDto
{
    public string Type { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? ClassroomNumber { get; set; }
}

public class SoftwareDto : MaterialItemDto
{
    public string Version { get; set; } = string.Empty;
    public string License { get; set; } = string.Empty;
    public DateTime LicenseExpirationDate { get; set; }
}

