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
    public List<VehiculeDTO>? VehiculesFiltered { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }
    public bool IsAdmin { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SearchQuery { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? FilterMarque { get; set; }

    [BindProperty(SupportsGet = true)]
    public decimal? FilterPrixMin { get; set; }

    [BindProperty(SupportsGet = true)]
    public decimal? FilterPrixMax { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool? FilterDisponible { get; set; }

    public List<string>? Marques { get; set; }

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
            
            // Extraire les marques uniques pour le filtrage
            Marques = Vehicules
                .Select(v => v.Marque)
                .Distinct()
                .OrderBy(m => m)
                .ToList();
            
            // Appliquer les filtres
            ApplyFilters();
        }
        else
        {
            Message = response.Message ?? "Erreur lors du chargement des véhicules";
            IsError = true;
        }
    }

    private void ApplyFilters()
    {
        VehiculesFiltered = Vehicules ?? new List<VehiculeDTO>();

        // Filtre par recherche (marque, modèle, immatriculation)
        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            var searchLower = SearchQuery.ToLower();
            VehiculesFiltered = VehiculesFiltered.Where(v =>
                v.Marque.ToLower().Contains(searchLower) ||
                v.Modele.ToLower().Contains(searchLower) ||
                v.Immatriculation.ToLower().Contains(searchLower) ||
                (v.Couleur?.ToLower().Contains(searchLower) ?? false)
            ).ToList();
        }

        // Filtre par marque
        if (!string.IsNullOrWhiteSpace(FilterMarque))
        {
            VehiculesFiltered = VehiculesFiltered.Where(v => v.Marque == FilterMarque).ToList();
        }

        // Filtre par prix minimum
        if (FilterPrixMin.HasValue)
        {
            VehiculesFiltered = VehiculesFiltered.Where(v => v.PrixJournalier >= FilterPrixMin.Value).ToList();
        }

        // Filtre par prix maximum
        if (FilterPrixMax.HasValue)
        {
            VehiculesFiltered = VehiculesFiltered.Where(v => v.PrixJournalier <= FilterPrixMax.Value).ToList();
        }

        // Filtre par disponibilité
        if (FilterDisponible.HasValue)
        {
            VehiculesFiltered = VehiculesFiltered.Where(v => v.EstDisponible == FilterDisponible.Value).ToList();
        }
    }
}
