using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IRepository<User> _repository;

    public UsersController(IRepository<User> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var users = await _repository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create([FromBody] CreateUserDto createDto)
    {
        if (createDto == null)
        {
            return BadRequest("Request body is required");
        }

        try
        {
            // Create a new User instance
            var user = new User(
                id: Guid.NewGuid(),
                name: createDto.Name,
                role: createDto.Role,
                login: createDto.Login,
                password: createDto.Password
            );

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto updateDto)
    {
        if (updateDto == null || id == Guid.Empty)
        {
            return BadRequest("Invalid request data");
        }

        var existingUser = await _repository.GetByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound($"User with ID {id} not found");
        }

        try
        {
            // Determine the password to use
            string passwordToUse = string.IsNullOrWhiteSpace(updateDto.Password) 
                ? existingUser.Password 
                : updateDto.Password;

            // Create a new User instance with updated values
            var updatedUser = new User(
                id: id,
                name: updateDto.Name,
                role: updateDto.Role,
                login: updateDto.Login,
                password: passwordToUse
            );

            _repository.Update(updatedUser);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _repository.Delete(user);
        await _repository.SaveChangesAsync();

        return NoContent();
    }
}

// DTOs for requests
public class CreateUserDto
{
    public string Name { get; set; } = string.Empty;
    public Domain.Enums.Role Role { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// DTO for Update request
public class UpdateUserDto
{
    public string Name { get; set; } = string.Empty;
    public Domain.Enums.Role Role { get; set; }
    public string Login { get; set; } = string.Empty;
    public string? Password { get; set; }
}
