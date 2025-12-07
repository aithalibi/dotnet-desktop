using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using System.ComponentModel.DataAnnotations;

namespace Location_voiture_front_web.Pages.Auth;

public class VerifyResetCodeModel : PageModel
{
    private readonly IAuthService _authService;

    public VerifyResetCodeModel(IAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    [Required(ErrorMessage = "L'email est requis")]
    public string? Email { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Le code est requis")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Le code doit contenir 6 chiffres")]
    public string? Code { get; set; }

    public string? ErrorMessage { get; set; }
    public bool CodeVerified { get; set; } = false;

    public void OnGet(string? email)
    {
        Email = email;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var request = new VerifyResetCodeRequest
        {
            Email = Email,
            Code = Code
        };

        var response = await _authService.VerifyResetCodeAsync(request);

        if (response.Success && response.Data?.Valid == true)
        {
            CodeVerified = true;
        }
        else
        {
            ErrorMessage = response.Message ?? "Code invalide ou expiré.";
        }

        return Page();
    }
}
