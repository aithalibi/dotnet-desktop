using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages;

public class IndexModel : PageModel
{
    private readonly IVehiculeService _vehiculeService;
    private readonly IAuthService _authService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IVehiculeService vehiculeService, IAuthService authService, ILogger<IndexModel> logger)
    {
        _vehiculeService = vehiculeService;
        _authService = authService;
        _logger = logger;
    }

    public List<VehiculeDTO>? Vehicules { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }

    public async Task OnGetAsync()
    {
        var user = _authService.GetCurrentUser();
        
        // If user is not authenticated, don't load vehicles
        if (user == null)
        {
            return;
        }

        // Token is automatically injected by AuthorizationHeaderHandler middleware
        var response = await _vehiculeService.GetAllVehicuelsAsync();
        if (response.Success)
        {
            Vehicules = response.Data;
        }
        else
        {
            Message = response.Message ?? "Erreur lors du chargement des véhicules";
            IsError = true;
            _logger.LogWarning($"Error loading vehicles: {response.Message}");
        }
    }
}


