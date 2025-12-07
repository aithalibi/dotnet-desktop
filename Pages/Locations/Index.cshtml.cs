using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Locations;

public class IndexModel : PageModel
{
    private readonly ILocationService _locationService;
    private readonly IAuthService _authService;

    public IndexModel(ILocationService locationService, IAuthService authService)
    {
        _locationService = locationService;
        _authService = authService;
    }

    public List<LocationDTO>? Locations { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }

    public async Task OnGetAsync()
    {
        await LoadLocationsForCurrentUserAsync();
    }

    public async Task OnPostCancelAsync(int id)
    {
        var response = await _locationService.CancelLocationAsync(id);
        if (response.Success)
        {
            Message = "Location annulée avec succès";
        }
        else
        {
            Message = response.Message ?? "Erreur lors de l'annulation";
            IsError = true;
        }

        await LoadLocationsForCurrentUserAsync();
    }

    private async Task LoadLocationsForCurrentUserAsync()
    {
        var user = _authService.GetCurrentUser();

        if (user == null)
        {
            Response.Redirect("/Auth/Login");
            return;
        }

        var response = await _locationService.GetAllLocationsAsync();
        if (response.Success)
        {
            // Filter locations by current user
            Locations = response.Data?.Where(l => l.ClientId == user.Id).ToList();
        }
        else
        {
            Message = response.Message ?? "Erreur lors du chargement des locations";
            IsError = true;
        }
    }
}
