using Location_voiture_front_web.Models;

namespace Location_voiture_front_web.Services;

public interface IClientService
{
    Task<ApiResponse<ClientProfileDto>> GetCurrentClientProfileAsync();
    Task<ApiResponse<ClientProfileDto>> GetClientProfileAsync(int id);
    Task<ApiResponse<ClientProfileDto>> UpdateClientProfileAsync(UpdateClientProfileDto dto);
    Task<ApiResponse<List<ClientListDto>>> GetAllClientsAsync();
    Task<ApiResponse<List<LocationDetailDto>>> GetClientLocationsAsync(int id);
    Task<ApiResponse<ClientStatsDto>> GetClientsStatisticsAsync();
}

public class ClientService : IClientService
{
    private readonly IApiService _apiService;

    public ClientService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<ApiResponse<ClientProfileDto>> GetCurrentClientProfileAsync()
    {
        return await _apiService.GetAsync<ClientProfileDto>("/api/Clients/me");
    }

    public async Task<ApiResponse<ClientProfileDto>> GetClientProfileAsync(int id)
    {
        return await _apiService.GetAsync<ClientProfileDto>($"/api/Clients/{id}");
    }

    public async Task<ApiResponse<ClientProfileDto>> UpdateClientProfileAsync(UpdateClientProfileDto dto)
    {
        var result = await _apiService.PutAsync<ClientProfileDto>("/api/Clients/me", dto);
        return result;
    }

    public async Task<ApiResponse<List<ClientListDto>>> GetAllClientsAsync()
    {
        return await _apiService.GetAsync<List<ClientListDto>>("/api/Clients");
    }

    public async Task<ApiResponse<List<LocationDetailDto>>> GetClientLocationsAsync(int id)
    {
        return await _apiService.GetAsync<List<LocationDetailDto>>($"/api/Clients/{id}/locations");
    }

    public async Task<ApiResponse<ClientStatsDto>> GetClientsStatisticsAsync()
    {
        return await _apiService.GetAsync<ClientStatsDto>("/api/Clients/stats/overview");
    }
}

// DTOs
public class ClientProfileDto
{
    public int Id { get; set; }
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Adresse { get; set; }
    public string? NumeroPermis { get; set; }
    public DateTime DateInscription { get; set; }
    public bool EstActif { get; set; }
    public int NombreLocations { get; set; }
    public List<LocationDetailDto>? Locations { get; set; }
}

public class UpdateClientProfileDto
{
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Telephone { get; set; }
    public string? Adresse { get; set; }
    public string? NumeroPermis { get; set; }
}

public class ClientListDto
{
    public int Id { get; set; }
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public DateTime DateInscription { get; set; }
    public int NombreLocations { get; set; }
}

public class LocationDetailDto
{
    public int Id { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public int VehiculeId { get; set; }
    public string? VehiculeNom { get; set; }
    public string? Statut { get; set; }
    public decimal PrixTotal { get; set; }
    public DateTime DateCreation { get; set; }
}

public class ClientStatsDto
{
    public int TotalClients { get; set; }
    public int ClientsThisMonth { get; set; }
    public int TotalLocations { get; set; }
    public decimal AverageLocationsPerClient { get; set; }
}
