using Domain.Enums;

namespace Domain.Entities;

public class ManagementSystem
{
    public List<MaterialItem> MaterialItems { get; private set; }
    public List<Classroom> Classrooms { get; private set; }

    public ManagementSystem()
    {
        MaterialItems = new List<MaterialItem>();
        Classrooms = new List<Classroom>();
    }

    public void RegisterItem(MaterialItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (MaterialItems.Any(m => m.Id == item.Id))
            throw new InvalidOperationException("Item with this ID is already registered.");

        item.Register();
        MaterialItems.Add(item);
    }

    public MaterialItem? FindItemByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        return MaterialItems.FirstOrDefault(item => 
            item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public string GenerateReport()
    {
        var report = "=== Material Management System Report ===\n\n";
        
        report += $"Total Material Items: {MaterialItems.Count}\n";
        
        var equipmentCount = MaterialItems.OfType<Equipment>().Count();
        var furnitureCount = MaterialItems.OfType<Furniture>().Count();
        var softwareCount = MaterialItems.OfType<Software>().Count();
        
        report += $"\nBy Type:\n";
        report += $"  Equipment: {equipmentCount}\n";
        report += $"  Furniture: {furnitureCount}\n";
        report += $"  Software: {softwareCount}\n";
        
        var operationalCount = MaterialItems.Count(m => m.State == State.Operational);
        var writtenOffCount = MaterialItems.Count(m => m.State == State.WrittenOff);
        var underRepairCount = MaterialItems.Count(m => m.State == State.UnderRepair);
        
        report += $"\nBy State:\n";
        report += $"  Operational: {operationalCount}\n";
        report += $"  Written Off: {writtenOffCount}\n";
        report += $"  Under Repair: {underRepairCount}\n";
        
        report += $"\nTotal Classrooms: {Classrooms.Count}\n";
        
        if (Classrooms.Count > 0)
        {
            report += $"\nClassrooms:\n";
            foreach (var classroom in Classrooms)
            {
                report += $"  {classroom.Number} ({classroom.ClassroomType}) - " +
                         $"Capacity: {classroom.Capacity}, " +
                         $"Equipment: {classroom.Equipment.Count}, " +
                         $"Furniture: {classroom.Furniture.Count}\n";
            }
        }
        
        return report;
    }
}

