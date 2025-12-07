namespace Location_voiture_front_web.Models;

public class LocationDTO
{
    public int Id { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public int DureeJours { get; set; }
    public decimal MontantTotal { get; set; }
    public string? Statut { get; set; }
    public string? QRCode { get; set; }
    public string? FactureUrl { get; set; }
    public int ClientId { get; set; }
    public int VehiculeId { get; set; }
    public int? EmployeId { get; set; }
    public DateTime DateCreation { get; set; }
    public VehiculeDTO? Vehicule { get; set; }
    public UtilisateurDTO? Client { get; set; }
}

public class CreateLocationRequest
{
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public int VehiculeId { get; set; }
    public int ClientId { get; set; }
}

public class UpdateLocationRequest
{
    public int Id { get; set; }
    public DateTime? DateFin { get; set; }
    public string? Statut { get; set; }
}

public class LocationFiltersDto
{
    public int? ClientId { get; set; }
    public int? VehiculeId { get; set; }
    public string? Statut { get; set; }
    public DateTime? DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
}
