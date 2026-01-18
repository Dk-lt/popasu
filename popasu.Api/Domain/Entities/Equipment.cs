using Domain.Enums;

namespace Domain.Entities;

public class Equipment : MaterialItem
{
    public string SerialNumber { get; private set; }
    public string Location { get; private set; }
    public string TechnicalCharacteristics { get; private set; }
    public string? ClassroomNumber { get; private set; }
    public Classroom? Classroom { get; private set; }

    public Equipment(
        Guid id,
        string name,
        string description,
        int quantity,
        DateTime receivedDate,
        string serialNumber,
        string location,
        string technicalCharacteristics)
        : base(id, name, description, quantity, receivedDate)
    {
        if (string.IsNullOrWhiteSpace(serialNumber))
            throw new ArgumentException("Serial number cannot be null or empty.", nameof(serialNumber));
        
        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location cannot be null or empty.", nameof(location));

        SerialNumber = serialNumber;
        Location = location;
        TechnicalCharacteristics = technicalCharacteristics ?? string.Empty;
    }

    public void WriteOff()
    {
        if (State == State.WrittenOff)
            throw new InvalidOperationException("Equipment is already written off.");

        State = State.WrittenOff;
    }

    public void Move(string newLocation)
    {
        if (string.IsNullOrWhiteSpace(newLocation))
            throw new ArgumentException("Location cannot be null or empty.", nameof(newLocation));

        if (State == State.WrittenOff)
            throw new InvalidOperationException("Cannot move written off equipment.");

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
               $"Serial Number: {SerialNumber}\n" +
               $"Location: {Location}\n" +
               $"Technical Characteristics: {TechnicalCharacteristics}";
    }
}

