using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Entretiens;

public class IndexModel : PageModel
{
    private readonly IEntretienService _entretienService;
    private readonly IAuthService _authService;

    public IndexModel(IEntretienService entretienService, IAuthService authService)
    {
        _entretienService = entretienService;
        _authService = authService;
    }

    public List<EntretienDTO>? Entretiens { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }

    public async Task OnGetAsync()
    {
        var user = _authService.GetCurrentUser();
        
        if (user == null || user.TypeUtilisateur != "EMPLOYE" && user.TypeUtilisateur != "ADMINISTRATEUR")
        {
            RedirectToPage("/Auth/Login");
            return;
        }

        var response = await _entretienService.GetAllEntretiemsAsync();
        if (response.Success)
        {
            Entretiens = response.Data;
        }
        else
        {
            Message = response.Message ?? "Erreur lors du chargement des entretiens";
            IsError = true;
        }
    }
}
