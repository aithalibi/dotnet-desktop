using Location_voiture_front_web.Models;

namespace Location_voiture_front_web.Services;

public interface IVehiculeService
{
    Task<ApiResponse<List<VehiculeDTO>>> GetAllVehicuelsAsync();
    Task<ApiResponse<VehiculeDTO>> GetVehiculeByIdAsync(int id);
    Task<ApiResponse<VehiculeDTO>> CreateVehiculeAsync(CreateVehiculeRequest request);
    Task<ApiResponse<VehiculeDTO>> UpdateVehiculeAsync(UpdateVehiculeRequest request);
    Task<ApiResponse> DeleteVehiculeAsync(int id);
    Task<ApiResponse<List<TypeVehiculeDTO>>> GetAllTypesAsync();
}

public class VehiculeService : IVehiculeService
{
    private readonly IApiService _apiService;
    private readonly ILogger<VehiculeService> _logger;

    public VehiculeService(IApiService apiService, ILogger<VehiculeService> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<ApiResponse<List<VehiculeDTO>>> GetAllVehicuelsAsync()
    {
        return await _apiService.GetAsync<List<VehiculeDTO>>("/api/Vehicules");
    }

    public async Task<ApiResponse<VehiculeDTO>> GetVehiculeByIdAsync(int id)
    {
        return await _apiService.GetAsync<VehiculeDTO>($"/api/Vehicules/{id}");
    }

    public async Task<ApiResponse<VehiculeDTO>> CreateVehiculeAsync(CreateVehiculeRequest request)
    {
        return await _apiService.PostAsync<VehiculeDTO>("/api/Vehicules", request);
    }

    public async Task<ApiResponse<VehiculeDTO>> UpdateVehiculeAsync(UpdateVehiculeRequest request)
    {
        return await _apiService.PutAsync<VehiculeDTO>($"/api/Vehicules/{request.Id}", request);
    }

    public async Task<ApiResponse> DeleteVehiculeAsync(int id)
    {
        return await _apiService.DeleteAsync($"/api/Vehicules/{id}");
    }

    public async Task<ApiResponse<List<TypeVehiculeDTO>>> GetAllTypesAsync()
    {
        return await _apiService.GetAsync<List<TypeVehiculeDTO>>("/api/Vehicules/types");
    }
}
