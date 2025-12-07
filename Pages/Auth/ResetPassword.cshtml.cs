using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using System.ComponentModel.DataAnnotations;

namespace Location_voiture_front_web.Pages.Auth;

public class ResetPasswordModel : PageModel
{
    private readonly IAuthService _authService;

    public ResetPasswordModel(IAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    [Required(ErrorMessage = "L'email est requis")]
    public string? Email { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Le code est requis")]
    public string? Code { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Le nouveau mot de passe est requis")]
    [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
    public string? NewPassword { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Veuillez confirmer le mot de passe")]
    [Compare("NewPassword", ErrorMessage = "Les mots de passe ne correspondent pas")]
    public string? ConfirmPassword { get; set; }

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public void OnGet(string? email, string? code)
    {
        Email = email;
        Code = code;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var request = new ResetPasswordRequest
        {
            Email = Email,
            Code = Code,
            NewPassword = NewPassword,
            ConfirmPassword = ConfirmPassword
        };

        var response = await _authService.ResetPasswordAsync(request);

        if (response.Success)
        {
            SuccessMessage = "Votre mot de passe a été changé avec succès ! Vous pouvez maintenant vous connecter.";
        }
        else
        {
            ErrorMessage = response.Message ?? "Erreur lors de la réinitialisation. Le code a peut-être expiré.";
        }

        return Page();
    }
}
