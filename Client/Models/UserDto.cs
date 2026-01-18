using System.ComponentModel.DataAnnotations;

namespace popasu.Client.Models;

public class UserDto
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public Role Role { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Login { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
}

