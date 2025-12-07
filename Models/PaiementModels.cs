namespace Location_voiture_front_web.Models;

public class PaiementDTO
{
    public int Id { get; set; }
    public decimal Montant { get; set; }
    public DateTime DatePaiement { get; set; }
    public string? ModePaiement { get; set; }
    public string? Statut { get; set; }
    public string? Reference { get; set; }
    public int LocationId { get; set; }
    public DateTime DateCreation { get; set; }
}

public class CreatePaiementRequest
{
    public decimal Montant { get; set; }
    public string? ModePaiement { get; set; }
    public int LocationId { get; set; }
}

public class UpdatePaiementRequest
{
    public int Id { get; set; }
    public string? Statut { get; set; }
}
