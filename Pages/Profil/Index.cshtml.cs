using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Profil;

public class IndexModel : PageModel
{
    private readonly IClientService _clientService;
    private readonly IAuthService _authService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IClientService clientService, IAuthService authService, ILogger<IndexModel> logger)
    {
        _clientService = clientService;
        _authService = authService;
        _logger = logger;
    }

    public ClientProfileDto? ClientProfile { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }
    public bool IsEditing { get; set; }

    [BindProperty]
    public string? Nom { get; set; }

    [BindProperty]
    public string? Prenom { get; set; }

    [BindProperty]
    public string? Telephone { get; set; }

    [BindProperty]
    public string? Adresse { get; set; }

    [BindProperty]
    public string? NumeroPermis { get; set; }

    public async Task OnGetAsync(bool edit = false)
    {
        var user = _authService.GetCurrentUser();
        
        if (user == null)
        {
            RedirectToPage("/Auth/Login");
            return;
        }

        IsEditing = edit;

        var response = await _clientService.GetCurrentClientProfileAsync();
        if (response.Success && response.Data != null)
        {
            ClientProfile = response.Data;
            Nom = ClientProfile.Nom;
            Prenom = ClientProfile.Prenom;
            Telephone = ClientProfile.Telephone;
            Adresse = ClientProfile.Adresse;
            NumeroPermis = ClientProfile.NumeroPermis;
        }
        else
        {
            Message = response.Message ?? "Erreur lors du chargement du profil";
            IsError = true;
        }
    }

    public async Task OnPostAsync()
    {
        var user = _authService.GetCurrentUser();
        
        if (user == null)
        {
            RedirectToPage("/Auth/Login");
            return;
        }

        if (string.IsNullOrEmpty(Nom) || string.IsNullOrEmpty(Prenom) || 
            string.IsNullOrEmpty(Telephone) || string.IsNullOrEmpty(Adresse) ||
            string.IsNullOrEmpty(NumeroPermis))
        {
            Message = "Tous les champs sont obligatoires";
            IsError = true;
            return;
        }

        var updateDto = new UpdateClientProfileDto
        {
            Nom = Nom,
            Prenom = Prenom,
            Telephone = Telephone,
            Adresse = Adresse,
            NumeroPermis = NumeroPermis
        };

        var response = await _clientService.UpdateClientProfileAsync(updateDto);
        
        if (response.Success)
        {
            Message = "Profil mis à jour avec succès";
            IsError = false;
            // Recharger les données
            await OnGetAsync();
        }
        else
        {
            Message = response.Message ?? "Erreur lors de la mise à jour du profil";
            IsError = true;
        }
    }
}
