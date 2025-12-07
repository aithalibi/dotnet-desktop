using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly IAuthService _authService;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IAuthService authService, ILogger<LoginModel> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [BindProperty]
    public string? Email { get; set; }

    [BindProperty]
    public string? MotDePasse { get; set; }

    public string? Message { get; set; }
    public bool IsError { get; set; }

    public async Task OnPostAsync()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(MotDePasse))
        {
            Message = "Email et mot de passe requis";
            IsError = true;
            return;
        }

        var request = new LoginRequest { Email = Email, MotDePasse = MotDePasse };
        var response = await _authService.LoginAsync(request);

        _logger.LogInformation($"Login response - Success: {response.Success}, Data: {(response.Data != null ? "exists" : "null")}");

        if (response.Success && response.Data != null)
        {
            try
            {
                // Store user information in session
                var user = new UtilisateurDTO
                {
                    Id = response.Data.User?.Id ?? 0,
                    Nom = response.Data.User?.Nom ?? "",
                    Prenom = response.Data.User?.Prenom ?? "",
                    Email = response.Data.User?.Email ?? Email ?? "",
                    TypeUtilisateur = response.Data.User?.TypeUtilisateur ?? "CLIENT",
                    EstActif = true
                };
                _authService.SetCurrentUser(user);
                _logger.LogInformation($"User stored in session: {user.Email}");
                
                Response.Redirect("/Index");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error storing user in session");
                Message = "Erreur lors du stockage de la session";
                IsError = true;
            }
        }
        else
        {
            Message = response.Message ?? "Erreur d'authentification";
            IsError = true;
            _logger.LogWarning($"Login failed: {Message}");
        }
    }
}
