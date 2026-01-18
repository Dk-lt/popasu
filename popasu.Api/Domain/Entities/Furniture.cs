using Domain.Enums;

namespace Domain.Entities;

public class Furniture : MaterialItem
{
    public string Type { get; private set; }
    public string Material { get; private set; }
    public string Location { get; private set; }
    public string? ClassroomNumber { get; private set; }
    public Classroom? Classroom { get; private set; }

    public Furniture(
        Guid id,
        string name,
        string description,
        int quantity,
        DateTime receivedDate,
        string type,
        string material,
        string location)
        : base(id, name, description, quantity, receivedDate)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type cannot be null or empty.", nameof(type));
        
        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location cannot be null or empty.", nameof(location));

        Type = type;
        Material = material ?? string.Empty;
        Location = location;
    }

    public void WriteOff()
    {
        if (State == State.WrittenOff)
            throw new InvalidOperationException("Furniture is already written off.");

        State = State.WrittenOff;
    }

    public void Move(string newLocation)
    {
        if (string.IsNullOrWhiteSpace(newLocation))
            throw new ArgumentException("Location cannot be null or empty.", nameof(newLocation));

        if (State == State.WrittenOff)
            throw new InvalidOperationException("Cannot move written off furniture.");

        Location = newLocation;
    }

    public void AssignToClassroom(string classroomNumber)
    {
        if (string.IsNullOrWhiteSpace(classroomNumber))
            throw new ArgumentException("Classroom number cannot be null or empty.", nameof(classroomNumber));

        ClassroomNumber = classroomNumber;
    }

    internal void AssignToClassroom(Classroom classroom)
    {
        if (classroom == null)
            throw new ArgumentNullException(nameof(classroom));

        ClassroomNumber = classroom.Number;
        Classroom = classroom;
    }

    internal void UnassignFromClassroom()
    {
        ClassroomNumber = null;
        Classroom = null;
    }

    public override string ViewInformation()
    {
        return base.ViewInformation() + $"\n" +
               $"Type: {Type}\n" +
               $"Material: {Material}\n" +
               $"Location: {Location}";
    }
}

