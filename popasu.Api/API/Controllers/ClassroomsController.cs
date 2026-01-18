using API.Models;
using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassroomsController : ControllerBase
{
    private readonly IClassroomRepository _repository;

    public ClassroomsController(IClassroomRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassroomDto>>> GetAll()
    {
        var classrooms = await _repository.GetAllAsync();
        var dtos = classrooms.Select(c => new ClassroomDto
        {
            Number = c.Number,
            Capacity = c.Capacity,
            ClassroomType = c.ClassroomType,
            Equipment = c.Equipment.Select(e => new ClassroomEquipmentDto
            {
                Id = e.Id,
                Name = e.Name,
                SerialNumber = e.SerialNumber,
                Location = e.Location
            }).ToList(),
            Furniture = c.Furniture.Select(f => new ClassroomFurnitureDto
            {
                Id = f.Id,
                Name = f.Name,
                Type = f.Type,
                Material = f.Material
            }).ToList()
        });
        return Ok(dtos);
    }

    [HttpGet("{number}")]
    public async Task<ActionResult<ClassroomDto>> GetByNumber(string number)
    {
        var classroom = await _repository.GetByNumberAsync(number);
        
        if (classroom == null)
        {
            return NotFound();
        }

        var dto = new ClassroomDto
        {
            Number = classroom.Number,
            Capacity = classroom.Capacity,
            ClassroomType = classroom.ClassroomType,
            Equipment = classroom.Equipment.Select(e => new ClassroomEquipmentDto
            {
                Id = e.Id,
                Name = e.Name,
                SerialNumber = e.SerialNumber,
                Location = e.Location
            }).ToList(),
            Furniture = classroom.Furniture.Select(f => new ClassroomFurnitureDto
            {
                Id = f.Id,
                Name = f.Name,
                Type = f.Type,
                Material = f.Material
            }).ToList()
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<Classroom>> Create([FromBody] Classroom classroom)
    {
        if (classroom == null)
        {
            return BadRequest();
        }

        await _repository.AddAsync(classroom);
        await _repository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetByNumber), new { number = classroom.Number }, classroom);
    }

    [HttpPut("{number}")]
    public async Task<IActionResult> Update(string number, [FromBody] Classroom classroom)
    {
        if (classroom == null || classroom.Number != number)
        {
            return BadRequest();
        }

        var existingClassroom = await _repository.GetByNumberAsync(number);
        if (existingClassroom == null)
        {
            return NotFound();
        }

        _repository.Update(classroom);
        await _repository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{number}")]
    public async Task<IActionResult> Delete(string number)
    {
        var classroom = await _repository.GetByNumberAsync(number);
        if (classroom == null)
        {
            return NotFound();
        }

        _repository.Delete(classroom);
        await _repository.SaveChangesAsync();

        return NoContent();
    }
}

