namespace Location_voiture_front_web.Models;

public class EntretienDTO
{
    public int Id { get; set; }
    public DateTime DateEntretien { get; set; }
    public string? TypeEntretien { get; set; }
    public string? Description { get; set; }
    public decimal Cout { get; set; }
    public DateTime? ProchainEntretien { get; set; }
    public bool EstUrgent { get; set; }
    public string? Statut { get; set; }
    public int VehiculeId { get; set; }
    public int? EmployeId { get; set; }
    public DateTime DateCreation { get; set; }
    public VehiculeDTO? Vehicule { get; set; }
}

public class CreateEntretienRequest
{
    public DateTime DateEntretien { get; set; }
    public string? TypeEntretien { get; set; }
    public string? Description { get; set; }
    public decimal Cout { get; set; }
    public DateTime? ProchainEntretien { get; set; }
    public bool EstUrgent { get; set; }
    public int VehiculeId { get; set; }
}

public class UpdateEntretienRequest
{
    public int Id { get; set; }
    public string? Statut { get; set; }
    public DateTime? DateEntretien { get; set; }
    public DateTime? ProchainEntretien { get; set; }
}
