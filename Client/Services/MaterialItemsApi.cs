using popasu.Client.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace popasu.Client.Services;

public class MaterialItemsApi
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public MaterialItemsApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<MaterialItemDto>> GetAllAsync()
    {
        try
        {
            var json = await _httpClient.GetStringAsync("api/materialitems");
            var items = JsonSerializer.Deserialize<List<JsonElement>>(json, _jsonOptions);
            
            var result = new List<MaterialItemDto>();
            if (items != null)
            {
                foreach (var item in items)
                {
                    var itemType = item.GetProperty("itemType").GetString() ?? "";
                    MaterialItemDto? dto = itemType.ToLowerInvariant() switch
                    {
                        "equipment" => JsonSerializer.Deserialize<EquipmentDto>(item.GetRawText(), _jsonOptions),
                        "furniture" => JsonSerializer.Deserialize<FurnitureDto>(item.GetRawText(), _jsonOptions),
                        "software" => JsonSerializer.Deserialize<SoftwareDto>(item.GetRawText(), _jsonOptions),
                        _ => JsonSerializer.Deserialize<MaterialItemDto>(item.GetRawText(), _jsonOptions)
                    };
                    if (dto != null)
                    {
                        result.Add(dto);
                    }
                }
            }
            return result;
        }
        catch
        {
            return new List<MaterialItemDto>();
        }
    }

    public async Task<MaterialItemDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var json = await _httpClient.GetStringAsync($"api/materialitems/{id}");
            var item = JsonSerializer.Deserialize<JsonElement>(json, _jsonOptions);
            var itemType = item.GetProperty("itemType").GetString() ?? "";
            
            return itemType.ToLowerInvariant() switch
            {
                "equipment" => JsonSerializer.Deserialize<EquipmentDto>(item.GetRawText(), _jsonOptions),
                "furniture" => JsonSerializer.Deserialize<FurnitureDto>(item.GetRawText(), _jsonOptions),
                "software" => JsonSerializer.Deserialize<SoftwareDto>(item.GetRawText(), _jsonOptions),
                _ => JsonSerializer.Deserialize<MaterialItemDto>(item.GetRawText(), _jsonOptions)
            };
        }
        catch
        {
            return null;
        }
    }

    public async Task<MaterialItemDto> CreateAsync(CreateMaterialItemRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/materialitems", request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var item = JsonSerializer.Deserialize<JsonElement>(json, _jsonOptions);
        var itemType = item.GetProperty("itemType").GetString() ?? "";
        
        return itemType.ToLowerInvariant() switch
        {
            "equipment" => JsonSerializer.Deserialize<EquipmentDto>(item.GetRawText(), _jsonOptions) ?? throw new Exception("Failed to create equipment"),
            "furniture" => JsonSerializer.Deserialize<FurnitureDto>(item.GetRawText(), _jsonOptions) ?? throw new Exception("Failed to create furniture"),
            "software" => JsonSerializer.Deserialize<SoftwareDto>(item.GetRawText(), _jsonOptions) ?? throw new Exception("Failed to create software"),
            _ => throw new Exception("Unknown item type")
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/materialitems/{id}");
        response.EnsureSuccessStatusCode();
    }
}

