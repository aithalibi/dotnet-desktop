using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Locations;

public class CreateModel : PageModel
{
    private readonly ILocationService _locationService;
    private readonly IVehiculeService _vehiculeService;
    private readonly IAuthService _authService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        ILocationService locationService,
        IVehiculeService vehiculeService,
        IAuthService authService,
        ILogger<CreateModel> logger)
    {
        _locationService = locationService;
        _vehiculeService = vehiculeService;
        _authService = authService;
        _logger = logger;
    }

    [BindProperty(SupportsGet = true)]
    public int? VehiculeId { get; set; }

    public List<VehiculeDTO>? Vehicules { get; set; }
    public VehiculeDTO? VehiculeSelected { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }

    [BindProperty]
    public DateTime DateDebut { get; set; }

    [BindProperty]
    public DateTime DateFin { get; set; }

    public async Task OnGetAsync()
    {
        var user = _authService.GetCurrentUser();
        
        if (user == null)
        {
            RedirectToPage("/Auth/Login");
            return;
        }

        // Charger tous les véhicules
        var vehiculesResponse = await _vehiculeService.GetAllVehicuelsAsync();
        if (vehiculesResponse.Success)
        {
            Vehicules = vehiculesResponse.Data;
        }

        // Si un véhicule spécifique est sélectionné
        if (VehiculeId.HasValue)
        {
            var response = await _vehiculeService.GetVehiculeByIdAsync(VehiculeId.Value);
            if (response.Success)
            {
                VehiculeSelected = response.Data;
            }
            else
            {
                Message = "Véhicule non trouvé";
                IsError = true;
            }
        }

        // Définir les dates par défaut
        DateDebut = DateTime.Now.AddDays(1).Date;
        DateFin = DateTime.Now.AddDays(8).Date;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = _authService.GetCurrentUser();
        
        if (user == null)
        {
            return RedirectToPage("/Auth/Login");
        }

        // Valider les dates
        if (DateDebut < DateTime.Now.Date)
        {
            Message = "La date de début doit être à partir d'aujourd'hui";
            IsError = true;
            await OnGetAsync();
            return Page();
        }

        if (DateFin <= DateDebut)
        {
            Message = "La date de fin doit être après la date de début";
            IsError = true;
            await OnGetAsync();
            return Page();
        }

        if (VehiculeId == null || VehiculeId <= 0)
        {
            Message = "Veuillez sélectionner un véhicule";
            IsError = true;
            await OnGetAsync();
            return Page();
        }

        // Créer la location
        var request = new CreateLocationRequest
        {
            DateDebut = DateDebut,
            DateFin = DateFin,
            VehiculeId = VehiculeId.Value,
            ClientId = user.Id
        };

        var response = await _locationService.CreateLocationAsync(request);

        if (response.Success && response.Data != null)
        {
            _logger.LogInformation($"Location créée avec succès - ID: {response.Data.Id}");
            return RedirectToPage("/Locations/Detail", new { id = response.Data.Id });
        }
        else
        {
            Message = response.Message ?? "Erreur lors de la création de la location";
            IsError = true;
            await OnGetAsync();
            return Page();
        }
    }
}
