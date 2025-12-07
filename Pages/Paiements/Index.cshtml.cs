using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Paiements;

public class IndexModel : PageModel
{
    private readonly IPaiementService _paiementService;
    private readonly IAuthService _authService;

    public IndexModel(IPaiementService paiementService, IAuthService authService)
    {
        _paiementService = paiementService;
        _authService = authService;
    }

    public List<PaiementDTO>? Paiements { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }

    public async Task OnGetAsync()
    {
        var user = _authService.GetCurrentUser();
        
        if (user == null)
        {
            RedirectToPage("/Auth/Login");
            return;
        }

        var response = await _paiementService.GetAllPaiementsAsync();
        if (response.Success)
        {
            Paiements = response.Data;
        }
        else
        {
            Message = response.Message ?? "Erreur lors du chargement des paiements";
            IsError = true;
        }
    }
}
