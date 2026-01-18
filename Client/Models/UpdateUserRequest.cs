using System.ComponentModel.DataAnnotations;

namespace popasu.Client.Models;

public class UpdateUserRequest
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Имя обязательно для заполнения")]
    [StringLength(200, ErrorMessage = "Имя не может превышать 200 символов")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Роль обязательна для заполнения")]
    public Role Role { get; set; }
    
    [Required(ErrorMessage = "Логин обязателен для заполнения")]
    [StringLength(100, ErrorMessage = "Логин не может превышать 100 символов")]
    public string Login { get; set; } = string.Empty;
    
    [StringLength(200, ErrorMessage = "Пароль не может превышать 200 символов")]
    public string? Password { get; set; }
}

