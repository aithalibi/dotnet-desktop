using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Auth;

public class LogoutModel : PageModel
{
    private readonly IAuthService _authService;

    public LogoutModel(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task OnGetAsync()
    {
        // Effectuer la déconnexion
        await _authService.LogoutAsync();
        
        // Invalider la session complètement
        HttpContext.Session.Clear();
    }
}
