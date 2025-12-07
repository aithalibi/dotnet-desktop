using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Vehicules;

public class IndexModel : PageModel
{
    private readonly IVehiculeService _vehiculeService;
    private readonly IAuthService _authService;

    public IndexModel(IVehiculeService vehiculeService, IAuthService authService)
    {
        _vehiculeService = vehiculeService;
        _authService = authService;
    }

    public List<VehiculeDTO>? Vehicules { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }
    public bool IsAdmin { get; set; }

    public async Task OnGetAsync()
    {
        var user = _authService.GetCurrentUser();
        IsAdmin = user?.TypeUtilisateur == "ADMINISTRATEUR" || user?.TypeUtilisateur == "EMPLOYE";
        
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
        }
    }
}
