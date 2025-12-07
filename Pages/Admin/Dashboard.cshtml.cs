using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Admin;

public class DashboardModel : PageModel
{
    private readonly ILocationService _locationService;
    private readonly IVehiculeService _vehiculeService;
    private readonly IPaiementService _paiementService;
    private readonly IAuthService _authService;

    public DashboardModel(ILocationService locationService, IVehiculeService vehiculeService, 
                         IPaiementService paiementService, IAuthService authService)
    {
        _locationService = locationService;
        _vehiculeService = vehiculeService;
        _paiementService = paiementService;
        _authService = authService;
    }

    public List<LocationDTO>? RecentLocations { get; set; }
    public List<VehiculeDTO>? Vehicules { get; set; }
    public List<PaiementDTO>? RecentPaiements { get; set; }
    public int TotalVehicules { get; set; }
    public int TotalLocations { get; set; }
    public decimal RevenuTotal { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }

    public async Task OnGetAsync()
    {
        var user = _authService.GetCurrentUser();
        
        if (user?.TypeUtilisateur != "ADMINISTRATEUR" && user?.TypeUtilisateur != "EMPLOYE")
        {
            RedirectToPage("/Auth/Login");
            return;
        }

        // Load locations
        var locationsResponse = await _locationService.GetAllLocationsAsync();
        if (locationsResponse.Success)
        {
            var allLocations = locationsResponse.Data ?? new List<LocationDTO>();
            RecentLocations = allLocations.OrderByDescending(l => l.DateDebut).Take(5).ToList();
            TotalLocations = allLocations.Count;
        }

        // Load vehicles
        var vehiculesResponse = await _vehiculeService.GetAllVehicuelsAsync();
        if (vehiculesResponse.Success)
        {
            Vehicules = vehiculesResponse.Data;
            TotalVehicules = Vehicules?.Count ?? 0;
        }

        // Load payments
        var paiementsResponse = await _paiementService.GetAllPaiementsAsync();
        if (paiementsResponse.Success)
        {
            var allPaiements = paiementsResponse.Data ?? new List<PaiementDTO>();
            RecentPaiements = allPaiements.OrderByDescending(p => p.DatePaiement).Take(5).ToList();
            RevenuTotal = allPaiements.Where(p => p.Statut == "VALIDE").Sum(p => p.Montant);
        }
    }
}
