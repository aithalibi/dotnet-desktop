using Location_voiture_front_web.Models;
using Location_voiture_front_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Location_voiture_front_web.Pages.Locations;

public class DetailModel : PageModel
{
    private readonly ILocationService _locationService;
    private readonly IPaiementService _paiementService;
    private readonly IAuthService _authService;
    private readonly ILogger<DetailModel> _logger;

    public DetailModel(
        ILocationService locationService,
        IPaiementService paiementService,
        IAuthService authService,
        ILogger<DetailModel> logger)
    {
        _locationService = locationService;
        _paiementService = paiementService;
        _authService = authService;
        _logger = logger;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public LocationDTO? Location { get; set; }
    public List<PaiementDTO>? Paiements { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }

    [BindProperty]
    public decimal MontantPaiement { get; set; }

    [BindProperty]
    public string? ModePaiement { get; set; } = "CARTE_CREDIT";

    public async Task OnGetAsync()
    {
        var user = _authService.GetCurrentUser();
        if (user == null)
        {
            RedirectToPage("/Auth/Login");
            return;
        }

        var response = await _locationService.GetLocationByIdAsync(Id);
        if (response.Success && response.Data != null)
        {
            Location = response.Data;

            // Vérifier que l'utilisateur actuel est propriétaire de la location ou admin
            if (Location.ClientId != user.Id && user.TypeUtilisateur != "ADMINISTRATEUR")
            {
                Message = "Vous n'avez pas accès à cette location";
                IsError = true;
                return;
            }

            var paiementsResponse = await _paiementService.GetPaiementsByLocationAsync(Id);
            if (paiementsResponse.Success)
            {
                Paiements = paiementsResponse.Data ?? new List<PaiementDTO>();
            }

            // Initialiser le montant du paiement
            MontantPaiement = Location.MontantTotal;
        }
        else
        {
            Message = response.Message ?? "Location non trouvée";
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

        // Récupérer la location
        var response = await _locationService.GetLocationByIdAsync(Id);
        if (!response.Success || response.Data == null)
        {
            Message = "Location non trouvée";
            IsError = true;
            return;
        }

        Location = response.Data;

        // Vérifier l'autorisation
        if (Location.ClientId != user.Id && user.TypeUtilisateur != "ADMINISTRATEUR")
        {
            Message = "Vous n'avez pas accès à cette location";
            IsError = true;
            return;
        }

        // Créer le paiement
        var createPaiement = new CreatePaiementRequest
        {
            LocationId = Id,
            Montant = MontantPaiement,
            ModePaiement = ModePaiement!
        };

        var paiementResponse = await _paiementService.CreatePaiementAsync(createPaiement);
        if (paiementResponse.Success)
        {
            Message = "Paiement enregistré avec succès";
            IsError = false;
            // Recharger les paiements
            var paiementsResponse = await _paiementService.GetPaiementsByLocationAsync(Id);
            if (paiementsResponse.Success)
            {
                Paiements = paiementsResponse.Data ?? new List<PaiementDTO>();
            }
        }
        else
        {
            Message = paiementResponse.Message ?? "Erreur lors de l'enregistrement du paiement";
            IsError = true;
        }
    }

    public async Task<IActionResult> OnPostRegenererQRCodeAsync()
    {
        var user = _authService.GetCurrentUser();
        if (user == null)
        {
            return RedirectToPage("/Auth/Login");
        }

        try
        {
            var response = await _locationService.RegenererQRCodeAsync(Id);
            if (response.Success)
            {
                TempData["SuccessMessage"] = "QR Code régénéré avec succès";
            }
            else
            {
                TempData["ErrorMessage"] = response.Message ?? "Erreur lors de la régénération du QR code";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la régénération du QR code");
            TempData["ErrorMessage"] = "Erreur lors de la régénération du QR code";
        }

        return RedirectToPage(new { id = Id });
    }

    public string GetStatusColor(string? statut)
    {
        return statut?.ToUpper() switch
        {
            "EN_ATTENTE" => "warning",
            "CONFIRMEE" => "info",
            "EN_COURS" => "primary",
            "TERMINEE" => "success",
            "ANNULEE" => "danger",
            _ => "secondary"
        };
    }

    public string GetStatusLabel(string? statut)
    {
        return statut?.ToUpper() switch
        {
            "EN_ATTENTE" => "En attente",
            "CONFIRMEE" => "Confirmée",
            "EN_COURS" => "En cours",
            "TERMINEE" => "Terminée",
            "ANNULEE" => "Annulée",
            _ => statut ?? "Inconnu"
        };
    }

    public string GetPaiementStatusColor(string? statut)
    {
        return statut?.ToUpper() switch
        {
            "EN_ATTENTE" => "warning",
            "VALIDE" => "success",
            "ECHOUE" => "danger",
            "REMBOURSE" => "info",
            _ => "secondary"
        };
    }

    public string GetPaiementStatusLabel(string? statut)
    {
        return statut?.ToUpper() switch
        {
            "EN_ATTENTE" => "En attente",
            "VALIDE" => "Validé",
            "ECHOUE" => "Échoué",
            "REMBOURSE" => "Remboursé",
            _ => statut ?? "Inconnu"
        };
    }

    public int GetDays()
    {
        if (Location == null) return 0;
        var days = (Location.DateFin - Location.DateDebut).Days;
        return days > 0 ? days : 1;
    }
}
