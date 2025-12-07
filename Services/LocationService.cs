using Location_voiture_front_web.Models;

namespace Location_voiture_front_web.Services;

public interface ILocationService
{
    Task<ApiResponse<List<LocationDTO>>> GetAllLocationsAsync();
    Task<ApiResponse<LocationDTO>> GetLocationByIdAsync(int id);
    Task<ApiResponse<LocationDTO>> CreateLocationAsync(CreateLocationRequest request);
    Task<ApiResponse<LocationDTO>> UpdateLocationAsync(UpdateLocationRequest request);
    Task<ApiResponse<LocationDTO>> CancelLocationAsync(int id);
    Task<ApiResponse<List<LocationDTO>>> GetLocationsByClientAsync(int clientId);
    Task<ApiResponse<List<LocationDTO>>> GetLocationsByVehiculeAsync(int vehiculeId);
    Task<ApiResponse<object>> RegenererQRCodeAsync(int id);
}

public class LocationService : ILocationService
{
    private readonly IApiService _apiService;
    private readonly ILogger<LocationService> _logger;

    public LocationService(IApiService apiService, ILogger<LocationService> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<ApiResponse<List<LocationDTO>>> GetAllLocationsAsync()
    {
        return await _apiService.GetAsync<List<LocationDTO>>("/api/Locations");
    }

    public async Task<ApiResponse<LocationDTO>> GetLocationByIdAsync(int id)
    {
        return await _apiService.GetAsync<LocationDTO>($"/api/Locations/{id}");
    }

    public async Task<ApiResponse<LocationDTO>> CreateLocationAsync(CreateLocationRequest request)
    {
        return await _apiService.PostAsync<LocationDTO>("/api/Locations", request);
    }

    public async Task<ApiResponse<LocationDTO>> UpdateLocationAsync(UpdateLocationRequest request)
    {
        return await _apiService.PutAsync<LocationDTO>($"/api/Locations/{request.Id}", request);
    }

    public async Task<ApiResponse<LocationDTO>> CancelLocationAsync(int id)
    {
        // Backend exposes a dedicated annulation endpoint instead of HTTP DELETE.
        return await _apiService.PostAsync<LocationDTO>($"/api/Locations/{id}/annuler", null);
    }

    public async Task<ApiResponse<List<LocationDTO>>> GetLocationsByClientAsync(int clientId)
    {
        return await _apiService.GetAsync<List<LocationDTO>>($"/api/Locations/client/{clientId}");
    }

    public async Task<ApiResponse<List<LocationDTO>>> GetLocationsByVehiculeAsync(int vehiculeId)
    {
        return await _apiService.GetAsync<List<LocationDTO>>($"/api/Locations/vehicule/{vehiculeId}");
    }

    public async Task<ApiResponse<object>> RegenererQRCodeAsync(int id)
    {
        return await _apiService.PostAsync<object>($"/api/Locations/{id}/regenerer-qrcode", null);
    }
}
