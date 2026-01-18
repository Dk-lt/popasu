using System.ComponentModel.DataAnnotations;

namespace popasu.Client.Models;

public class CreateMaterialItemRequest
{
    [Required(ErrorMessage = "Тип обязателен для заполнения")]
    public string Kind { get; set; } = string.Empty; // "equipment" | "furniture" | "software"
    
    public Guid? Id { get; set; }
    
    [Required(ErrorMessage = "Наименование обязательно для заполнения")]
    [StringLength(200, ErrorMessage = "Наименование не может превышать 200 символов")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(1000, ErrorMessage = "Описание не может превышать 1000 символов")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Количество обязательно для заполнения")]
    [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
    public int Quantity { get; set; }
    
    [Required(ErrorMessage = "Дата поступления обязательна для заполнения")]
    public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;
    
    [Required(ErrorMessage = "Состояние обязательно для заполнения")]
    public int State { get; set; } = 0; // 0 Operational, 1 WrittenOff, 2 UnderRepair

    // Equipment fields
    public string? SerialNumber { get; set; }
    public string? Location { get; set; }
    public string? TechnicalCharacteristics { get; set; }
    public string? ClassroomNumber { get; set; }

    // Furniture fields
    public string? Type { get; set; }
    public string? Material { get; set; }

    // Software fields
    public string? Version { get; set; }
    public string? License { get; set; }
    public DateTime? LicenseExpirationDate { get; set; }
}

