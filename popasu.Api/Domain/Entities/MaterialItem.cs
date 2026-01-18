using Domain.Enums;

namespace Domain.Entities;

public abstract class MaterialItem
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public DateTime ReceivedDate { get; private set; }
    public State State { get; protected set; }

    protected MaterialItem(
        Guid id,
        string name,
        string description,
        int quantity,
        DateTime receivedDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));

        Id = id;
        Name = name;
        Description = description ?? string.Empty;
        Quantity = quantity;
        ReceivedDate = receivedDate;
        State = State.Operational;
    }

    public virtual void Register()
    {
        if (State != State.Operational)
            throw new InvalidOperationException("Only operational items can be registered.");
    }

    public virtual void UpdateInformation(string name, string description, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));

        Name = name;
        Description = description ?? string.Empty;
        Quantity = quantity;
    }

    public virtual string ViewInformation()
    {
        return $"ID: {Id}\n" +
               $"Name: {Name}\n" +
               $"Description: {Description}\n" +
               $"Quantity: {Quantity}\n" +
               $"Received Date: {ReceivedDate:yyyy-MM-dd}\n" +
               $"State: {State}";
    }
}

