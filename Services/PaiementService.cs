using Location_voiture_front_web.Models;

namespace Location_voiture_front_web.Services;

public interface IPaiementService
{
    Task<ApiResponse<List<PaiementDTO>>> GetAllPaiementsAsync();
    Task<ApiResponse<PaiementDTO>> GetPaiementByIdAsync(int id);
    Task<ApiResponse<PaiementDTO>> CreatePaiementAsync(CreatePaiementRequest request);
    Task<ApiResponse<PaiementDTO>> UpdatePaiementAsync(UpdatePaiementRequest request);
    Task<ApiResponse> DeletePaiementAsync(int id);
    Task<ApiResponse<List<PaiementDTO>>> GetPaiementsByLocationAsync(int locationId);
}

public class PaiementService : IPaiementService
{
    private readonly IApiService _apiService;
    private readonly ILogger<PaiementService> _logger;

    public PaiementService(IApiService apiService, ILogger<PaiementService> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<ApiResponse<List<PaiementDTO>>> GetAllPaiementsAsync()
    {
        return await _apiService.GetAsync<List<PaiementDTO>>("/api/Paiements");
    }

    public async Task<ApiResponse<PaiementDTO>> GetPaiementByIdAsync(int id)
    {
        return await _apiService.GetAsync<PaiementDTO>($"/api/Paiements/{id}");
    }

    public async Task<ApiResponse<PaiementDTO>> CreatePaiementAsync(CreatePaiementRequest request)
    {
        return await _apiService.PostAsync<PaiementDTO>("/api/Paiements", request);
    }

    public async Task<ApiResponse<PaiementDTO>> UpdatePaiementAsync(UpdatePaiementRequest request)
    {
        return await _apiService.PutAsync<PaiementDTO>($"/api/Paiements/{request.Id}", request);
    }

    public async Task<ApiResponse> DeletePaiementAsync(int id)
    {
        return await _apiService.DeleteAsync($"/api/Paiements/{id}");
    }

    public async Task<ApiResponse<List<PaiementDTO>>> GetPaiementsByLocationAsync(int locationId)
    {
        return await _apiService.GetAsync<List<PaiementDTO>>($"/api/Paiements/location/{locationId}");
    }
}
