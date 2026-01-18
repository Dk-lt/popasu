namespace Domain.Entities;

public class Classroom
{
    public string Number { get; private set; }
    public int Capacity { get; private set; }
    public string ClassroomType { get; private set; }
    public List<Equipment> Equipment { get; private set; } = new();
    public List<Furniture> Furniture { get; private set; } = new();

    public Classroom(
        string number,
        int capacity,
        string classroomType)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Classroom number cannot be null or empty.", nameof(number));
        
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
        
        if (string.IsNullOrWhiteSpace(classroomType))
            throw new ArgumentException("Classroom type cannot be null or empty.", nameof(classroomType));

        Number = number;
        Capacity = capacity;
        ClassroomType = classroomType;
    }

    public void AddEquipment(Equipment item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (Equipment.Contains(item))
            throw new InvalidOperationException("Equipment item is already in this classroom.");

        Equipment.Add(item);
        item.AssignToClassroom(this);
    }

    public void RemoveEquipment(Equipment item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (!Equipment.Remove(item))
            throw new InvalidOperationException("Equipment item is not in this classroom.");

        item.UnassignFromClassroom();
    }

    public void AddFurniture(Furniture item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (Furniture.Contains(item))
            throw new InvalidOperationException("Furniture item is already in this classroom.");

        Furniture.Add(item);
        item.AssignToClassroom(this);
    }

    public void RemoveFurniture(Furniture item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (!Furniture.Remove(item))
            throw new InvalidOperationException("Furniture item is not in this classroom.");

        item.UnassignFromClassroom();
    }

    public string ViewContents()
    {
        var result = $"Classroom {Number} ({ClassroomType})\n" +
                     $"Capacity: {Capacity}\n" +
                     $"\nEquipment ({Equipment.Count} items):\n";

        if (Equipment.Count == 0)
        {
            result += "  No equipment\n";
        }
        else
        {
            foreach (var item in Equipment)
            {
                result += $"  - {item.Name} (Serial: {item.SerialNumber})\n";
            }
        }

        result += $"\nFurniture ({Furniture.Count} items):\n";

        if (Furniture.Count == 0)
        {
            result += "  No furniture\n";
        }
        else
        {
            foreach (var item in Furniture)
            {
                result += $"  - {item.Name} (Type: {item.Type})\n";
            }
        }

        return result;
    }
}

