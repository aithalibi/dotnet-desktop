using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Auth;

public class RegisterModel : PageModel
{
    private readonly IAuthService _authService;

    public RegisterModel(IAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public string? Nom { get; set; }

    [BindProperty]
    public string? Prenom { get; set; }

    [BindProperty]
    public string? Email { get; set; }

    [BindProperty]
    public string? Telephone { get; set; }

    [BindProperty]
    public string? Adresse { get; set; }

    [BindProperty]
    public string? NumeroPermis { get; set; }

    [BindProperty]
    public string? MotDePasse { get; set; }

    [BindProperty]
    public string? MotDePasseConfirm { get; set; }

    public string? Message { get; set; }
    public bool IsError { get; set; }

    public async Task OnPostAsync()
    {
        if (string.IsNullOrEmpty(Nom) || string.IsNullOrEmpty(Prenom) || string.IsNullOrEmpty(Email) || 
            string.IsNullOrEmpty(MotDePasse) || string.IsNullOrEmpty(Telephone) || 
            string.IsNullOrEmpty(Adresse) || string.IsNullOrEmpty(NumeroPermis))
        {
            Message = "Tous les champs sont requis";
            IsError = true;
            return;
        }

        if (MotDePasse != MotDePasseConfirm)
        {
            Message = "Les mots de passe ne correspondent pas";
            IsError = true;
            return;
        }

        var request = new RegisterRequest
        {
            Nom = Nom,
            Prenom = Prenom,
            Email = Email,
            Telephone = Telephone,
            Adresse = Adresse,
            NumeroPermis = NumeroPermis,
            MotDePasse = MotDePasse,
            MotDePasseConfirm = MotDePasseConfirm
        };

        var response = await _authService.RegisterAsync(request);
        if (response.Success)
        {
            Response.Redirect("/Auth/Login");
            return;
        }
        else
        {
            Message = response.Message ?? "Erreur lors de l'inscription";
            IsError = true;
        }
    }
}
