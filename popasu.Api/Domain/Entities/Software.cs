using Domain.Enums;

namespace Domain.Entities;

public class Software : MaterialItem
{
    public string Version { get; private set; }
    public string License { get; private set; }
    public DateTime LicenseExpirationDate { get; private set; }

    public Software(
        Guid id,
        string name,
        string description,
        int quantity,
        DateTime receivedDate,
        string version,
        string license,
        DateTime licenseExpirationDate)
        : base(id, name, description, quantity, receivedDate)
    {
        if (string.IsNullOrWhiteSpace(version))
            throw new ArgumentException("Version cannot be null or empty.", nameof(version));
        
        if (string.IsNullOrWhiteSpace(license))
            throw new ArgumentException("License cannot be null or empty.", nameof(license));

        Version = version;
        License = license;
        LicenseExpirationDate = licenseExpirationDate;
    }

    public void ExtendLicense(DateTime newDate)
    {
        if (newDate <= DateTime.Now)
            throw new ArgumentException("License expiration date must be in the future.", nameof(newDate));

        if (newDate <= LicenseExpirationDate)
            throw new ArgumentException("New expiration date must be later than current expiration date.", nameof(newDate));

        LicenseExpirationDate = newDate;
    }

    public override string ViewInformation()
    {
        return base.ViewInformation() + $"\n" +
               $"Version: {Version}\n" +
               $"License: {License}\n" +
               $"License Expiration Date: {LicenseExpirationDate:yyyy-MM-dd}";
    }
}

