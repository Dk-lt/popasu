using System.ComponentModel.DataAnnotations;

namespace popasu.Client.Models;

public class ClassroomDto
{
    [Required]
    [StringLength(50)]
    public string Number { get; set; } = string.Empty;
    
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than zero")]
    public int Capacity { get; set; }
    
    [Required]
    [StringLength(100)]
    public string ClassroomType { get; set; } = string.Empty;
    
    public List<EquipmentDto> Equipment { get; set; } = new();
    public List<FurnitureDto> Furniture { get; set; } = new();
}

