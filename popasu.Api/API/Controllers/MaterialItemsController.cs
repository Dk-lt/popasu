using API.Models;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialItemsController : ControllerBase
{
    private readonly IRepository<MaterialItem> _repository;

    public MaterialItemsController(IRepository<MaterialItem> repository)
    {
        _repository = repository;
    }

    private static DateTime ToUtc(DateTime dt) =>
        dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(dt, DateTimeKind.Utc)
        };

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaterialItemDto>>> GetAll()
    {
        var items = await _repository.GetAllAsync();
        var dtos = items.Select(item => MapToDto(item));
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MaterialItemDto>> GetById(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        
        if (item == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(item));
    }

    private static MaterialItemDto MapToDto(MaterialItem item)
    {
        return item switch
        {
            Equipment equipment => new EquipmentDto
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Description = equipment.Description,
                Quantity = equipment.Quantity,
                ReceivedDate = equipment.ReceivedDate,
                State = equipment.State,
                ItemType = "Equipment",
                SerialNumber = equipment.SerialNumber,
                Location = equipment.Location,
                TechnicalCharacteristics = equipment.TechnicalCharacteristics,
                ClassroomNumber = equipment.ClassroomNumber
            },
            Furniture furniture => new FurnitureDto
            {
                Id = furniture.Id,
                Name = furniture.Name,
                Description = furniture.Description,
                Quantity = furniture.Quantity,
                ReceivedDate = furniture.ReceivedDate,
                State = furniture.State,
                ItemType = "Furniture",
                Type = furniture.Type,
                Material = furniture.Material,
                Location = furniture.Location,
                ClassroomNumber = furniture.ClassroomNumber
            },
            Software software => new SoftwareDto
            {
                Id = software.Id,
                Name = software.Name,
                Description = software.Description,
                Quantity = software.Quantity,
                ReceivedDate = software.ReceivedDate,
                State = software.State,
                ItemType = "Software",
                Version = software.Version,
                License = software.License,
                LicenseExpirationDate = software.LicenseExpirationDate
            },
            _ => new MaterialItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Quantity = item.Quantity,
                ReceivedDate = item.ReceivedDate,
                State = item.State,
                ItemType = "Unknown"
            }
        };
    }

    [HttpPost]
    public async Task<ActionResult<MaterialItem>> Create([FromBody] CreateMaterialItemRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request body is required.");
        }

        // Validate kind
        var kind = request.Kind?.ToLowerInvariant();
        if (kind != "equipment" && kind != "furniture" && kind != "software")
        {
            return BadRequest($"Invalid kind: '{request.Kind}'. Supported values: 'equipment', 'furniture', 'software'.");
        }

        // Generate ID if not provided
        var id = request.Id ?? Guid.NewGuid();
        if (id == Guid.Empty)
        {
            id = Guid.NewGuid();
        }

        // Map state enum (State property has protected setter, so we use domain methods where available)

        MaterialItem entity;

        try
        {
            switch (kind)
            {
                case "equipment":
                    if (string.IsNullOrWhiteSpace(request.SerialNumber))
                        return BadRequest("SerialNumber is required for equipment.");
                    if (string.IsNullOrWhiteSpace(request.Location))
                        return BadRequest("Location is required for equipment.");

                    entity = new Equipment(
                        id,
                        request.Name,
                        request.Description,
                        request.Quantity,
                        ToUtc(request.ReceivedDate),
                        request.SerialNumber!,
                        request.Location!,
                        request.TechnicalCharacteristics ?? string.Empty);

                    // Assign classroom if provided
                    if (!string.IsNullOrWhiteSpace(request.ClassroomNumber))
                    {
                        ((Equipment)entity).AssignToClassroom(request.ClassroomNumber);
                    }

                    // Handle state: WrittenOff (1) can be set via WriteOff() method
                    if (request.State == 1 && ((Equipment)entity).State == State.Operational)
                    {
                        ((Equipment)entity).WriteOff();
                    }
                    break;

                case "furniture":
                    if (string.IsNullOrWhiteSpace(request.Type))
                        return BadRequest("Type is required for furniture.");
                    if (string.IsNullOrWhiteSpace(request.Location))
                        return BadRequest("Location is required for furniture.");

                    entity = new Furniture(
                        id,
                        request.Name,
                        request.Description,
                        request.Quantity,
                        ToUtc(request.ReceivedDate),
                        request.Type!,
                        request.Material ?? string.Empty,
                        request.Location!);

                    // Assign classroom if provided
                    if (!string.IsNullOrWhiteSpace(request.ClassroomNumber))
                    {
                        ((Furniture)entity).AssignToClassroom(request.ClassroomNumber);
                    }

                    // Handle state: WrittenOff (1) can be set via WriteOff() method
                    if (request.State == 1 && ((Furniture)entity).State == State.Operational)
                    {
                        ((Furniture)entity).WriteOff();
                    }
                    break;

                case "software":
                    if (string.IsNullOrWhiteSpace(request.Version))
                        return BadRequest("Version is required for software.");
                    if (string.IsNullOrWhiteSpace(request.License))
                        return BadRequest("License is required for software.");
                    if (!request.LicenseExpirationDate.HasValue)
                        return BadRequest("LicenseExpirationDate is required for software.");

                    entity = new Software(
                        id,
                        request.Name,
                        request.Description,
                        request.Quantity,
                        ToUtc(request.ReceivedDate),
                        request.Version!,
                        request.License!,
                        ToUtc(request.LicenseExpirationDate.Value));
                    break;

                default:
                    return BadRequest($"Unsupported kind: '{request.Kind}'.");
            }
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        // Reload to get the entity with all properties set
        var createdItem = await _repository.GetByIdAsync(entity.Id);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, MapToDto(createdItem!));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateMaterialItemRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request body is required.");
        }

        var existingItem = await _repository.GetByIdAsync(id);
        if (existingItem == null)
        {
            return NotFound();
        }

        // Validate kind matches existing item type
        var kind = request.Kind?.ToLowerInvariant();
        var expectedKind = existingItem switch
        {
            Equipment => "equipment",
            Furniture => "furniture",
            Software => "software",
            _ => null
        };

        if (kind != expectedKind)
        {
            return BadRequest($"Cannot change item kind. Existing item is '{expectedKind}', but request specifies '{request.Kind}'.");
        }

        // For update, we need to create a new entity with the same ID and replace the old one
        // Note: This is a simplified approach. In a real scenario, you might want to use domain methods
        // to update properties individually if they exist.
        
        MaterialItem updatedEntity;

        try
        {
            switch (kind)
            {
                case "equipment":
                    if (string.IsNullOrWhiteSpace(request.SerialNumber))
                        return BadRequest("SerialNumber is required for equipment.");
                    if (string.IsNullOrWhiteSpace(request.Location))
                        return BadRequest("Location is required for equipment.");

                    updatedEntity = new Equipment(
                        id,
                        request.Name,
                        request.Description,
                        request.Quantity,
                        ToUtc(request.ReceivedDate),
                        request.SerialNumber!,
                        request.Location!,
                        request.TechnicalCharacteristics ?? string.Empty);

                    // Assign classroom if provided
                    if (!string.IsNullOrWhiteSpace(request.ClassroomNumber))
                    {
                        ((Equipment)updatedEntity).AssignToClassroom(request.ClassroomNumber);
                    }

                    // Handle state: WrittenOff (1) can be set via WriteOff() method
                    if (request.State == 1 && ((Equipment)updatedEntity).State == State.Operational)
                    {
                        ((Equipment)updatedEntity).WriteOff();
                    }
                    break;

                case "furniture":
                    if (string.IsNullOrWhiteSpace(request.Type))
                        return BadRequest("Type is required for furniture.");
                    if (string.IsNullOrWhiteSpace(request.Location))
                        return BadRequest("Location is required for furniture.");

                    updatedEntity = new Furniture(
                        id,
                        request.Name,
                        request.Description,
                        request.Quantity,
                        ToUtc(request.ReceivedDate),
                        request.Type!,
                        request.Material ?? string.Empty,
                        request.Location!);

                    // Assign classroom if provided
                    if (!string.IsNullOrWhiteSpace(request.ClassroomNumber))
                    {
                        ((Furniture)updatedEntity).AssignToClassroom(request.ClassroomNumber);
                    }

                    // Handle state: WrittenOff (1) can be set via WriteOff() method
                    if (request.State == 1 && ((Furniture)updatedEntity).State == State.Operational)
                    {
                        ((Furniture)updatedEntity).WriteOff();
                    }
                    break;

                case "software":
                    if (string.IsNullOrWhiteSpace(request.Version))
                        return BadRequest("Version is required for software.");
                    if (string.IsNullOrWhiteSpace(request.License))
                        return BadRequest("License is required for software.");
                    if (!request.LicenseExpirationDate.HasValue)
                        return BadRequest("LicenseExpirationDate is required for software.");

                    updatedEntity = new Software(
                        id,
                        request.Name,
                        request.Description,
                        request.Quantity,
                        ToUtc(request.ReceivedDate),
                        request.Version!,
                        request.License!,
                        ToUtc(request.LicenseExpirationDate.Value));
                    break;

                default:
                    return BadRequest($"Unsupported kind: '{request.Kind}'.");
            }
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        _repository.Update(updatedEntity);
        await _repository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        _repository.Delete(item);
        await _repository.SaveChangesAsync();

        return NoContent();
    }
}

