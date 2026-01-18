using popasu.Client.Models;
using System.Net.Http.Json;

namespace popasu.Client.Services;

public class ClassroomsApi
{
    private readonly HttpClient _httpClient;

    public ClassroomsApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ClassroomDto>> GetAllAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<ClassroomDto>>("api/classrooms");
            return response ?? new List<ClassroomDto>();
        }
        catch
        {
            return new List<ClassroomDto>();
        }
    }

    public async Task<ClassroomDto?> GetByNumberAsync(string number)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ClassroomDto>($"api/classrooms/{Uri.EscapeDataString(number)}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<ClassroomDto> CreateAsync(CreateClassroomRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/classrooms", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ClassroomDto>() ?? throw new Exception("Failed to create classroom");
    }

    public async Task UpdateAsync(string number, UpdateClassroomRequest request)
    {
        request.Number = number;
        var response = await _httpClient.PutAsJsonAsync($"api/classrooms/{Uri.EscapeDataString(number)}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string number)
    {
        var response = await _httpClient.DeleteAsync($"api/classrooms/{Uri.EscapeDataString(number)}");
        response.EnsureSuccessStatusCode();
    }
}

