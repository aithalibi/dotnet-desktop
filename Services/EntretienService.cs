using Location_voiture_front_web.Models;

namespace Location_voiture_front_web.Services;

public interface IEntretienService
{
    Task<ApiResponse<List<EntretienDTO>>> GetAllEntretiemsAsync();
    Task<ApiResponse<EntretienDTO>> GetEntretienByIdAsync(int id);
    Task<ApiResponse<EntretienDTO>> CreateEntretienAsync(CreateEntretienRequest request);
    Task<ApiResponse<EntretienDTO>> UpdateEntretienAsync(UpdateEntretienRequest request);
    Task<ApiResponse> DeleteEntretienAsync(int id);
    Task<ApiResponse<List<EntretienDTO>>> GetEntretiensByVehiculeAsync(int vehiculeId);
}

public class EntretienService : IEntretienService
{
    private readonly IApiService _apiService;
    private readonly ILogger<EntretienService> _logger;

    public EntretienService(IApiService apiService, ILogger<EntretienService> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<ApiResponse<List<EntretienDTO>>> GetAllEntretiemsAsync()
    {
        return await _apiService.GetAsync<List<EntretienDTO>>("/api/Entretiens");
    }

    public async Task<ApiResponse<EntretienDTO>> GetEntretienByIdAsync(int id)
    {
        return await _apiService.GetAsync<EntretienDTO>($"/api/Entretiens/{id}");
    }

    public async Task<ApiResponse<EntretienDTO>> CreateEntretienAsync(CreateEntretienRequest request)
    {
        return await _apiService.PostAsync<EntretienDTO>("/api/Entretiens", request);
    }

    public async Task<ApiResponse<EntretienDTO>> UpdateEntretienAsync(UpdateEntretienRequest request)
    {
        return await _apiService.PutAsync<EntretienDTO>($"/api/Entretiens/{request.Id}", request);
    }

    public async Task<ApiResponse> DeleteEntretienAsync(int id)
    {
        return await _apiService.DeleteAsync($"/api/Entretiens/{id}");
    }

    public async Task<ApiResponse<List<EntretienDTO>>> GetEntretiensByVehiculeAsync(int vehiculeId)
    {
        return await _apiService.GetAsync<List<EntretienDTO>>($"/api/Entretiens/vehicule/{vehiculeId}");
    }
}
