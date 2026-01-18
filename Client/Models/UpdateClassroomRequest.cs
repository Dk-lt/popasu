using System.ComponentModel.DataAnnotations;

namespace popasu.Client.Models;

public class UpdateClassroomRequest
{
    [Required(ErrorMessage = "Номер обязателен для заполнения")]
    [StringLength(50, ErrorMessage = "Номер не может превышать 50 символов")]
    public string Number { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Вместимость обязательна для заполнения")]
    [Range(1, int.MaxValue, ErrorMessage = "Вместимость должна быть больше нуля")]
    public int Capacity { get; set; }
    
    [Required(ErrorMessage = "Тип аудитории обязателен для заполнения")]
    [StringLength(100, ErrorMessage = "Тип аудитории не может превышать 100 символов")]
    public string ClassroomType { get; set; } = string.Empty;
}

