using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using System.ComponentModel.DataAnnotations;

namespace Location_voiture_front_web.Pages.Auth;

public class ForgotPasswordModel : PageModel
{
    private readonly IAuthService _authService;

    public ForgotPasswordModel(IAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    [Required(ErrorMessage = "L'email est requis")]
    [EmailAddress(ErrorMessage = "Format d'email invalide")]
    public string? Email { get; set; }

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }
    public bool CodeSent { get; set; } = false;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var request = new ForgotPasswordRequest
        {
            Email = Email
        };

        var response = await _authService.ForgotPasswordAsync(request);

        if (response.Success)
        {
            CodeSent = true;
            SuccessMessage = "Un code de vérification a été envoyé à votre email.";
        }
        else
        {
            ErrorMessage = response.Message ?? "Une erreur est survenue. Veuillez réessayer.";
        }

        return Page();
    }
}
