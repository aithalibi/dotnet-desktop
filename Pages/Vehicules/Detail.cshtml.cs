using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Vehicules;

public class DetailModel : PageModel
{
    private readonly IVehiculeService _vehiculeService;
    private readonly ILocationService _locationService;

    public DetailModel(IVehiculeService vehiculeService, ILocationService locationService)
    {
        _vehiculeService = vehiculeService;
        _locationService = locationService;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public VehiculeDTO? Vehicule { get; set; }

    public async Task OnGetAsync()
    {
        var response = await _vehiculeService.GetVehiculeByIdAsync(Id);
        if (response.Success)
        {
            Vehicule = response.Data;
        }
    }
}
