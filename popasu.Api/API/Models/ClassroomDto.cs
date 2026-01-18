namespace API.Models;

public class ClassroomDto
{
    public string Number { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string ClassroomType { get; set; } = string.Empty;
    public List<ClassroomEquipmentDto> Equipment { get; set; } = new();
    public List<ClassroomFurnitureDto> Furniture { get; set; } = new();
}

public class ClassroomEquipmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

public class ClassroomFurnitureDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
}

