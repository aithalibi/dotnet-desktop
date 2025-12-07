namespace Location_voiture_front_web.Models;

public class TypeVehiculeDTO
{
    public int Id { get; set; }
    public string? Nom { get; set; }
    public string? Description { get; set; }
    public string? Categorie { get; set; }
    public decimal PrixBaseJournalier { get; set; }
    public DateTime DateCreation { get; set; }
}

public class VehiculeDTO
{
    public int Id { get; set; }
    public string? Marque { get; set; }
    public string? Modele { get; set; }
    public string? Immatriculation { get; set; }
    public int Annee { get; set; }
    public string? Couleur { get; set; }
    public decimal PrixJournalier { get; set; }
    public bool EstDisponible { get; set; }
    public int Kilometrage { get; set; }
    public string? ImagePrincipaleUrl { get; set; }
    public List<string>? ImagesUrls { get; set; }
    public int TypeVehiculeId { get; set; }
    public TypeVehiculeDTO? TypeVehicule { get; set; }
    public DateTime DateAjout { get; set; }
}

public class CreateVehiculeRequest
{
    public string? Marque { get; set; }
    public string? Modele { get; set; }
    public string? Immatriculation { get; set; }
    public int Annee { get; set; }
    public string? Couleur { get; set; }
    public decimal PrixJournalier { get; set; }
    public int Kilometrage { get; set; }
    public string? ImagePrincipaleUrl { get; set; }
    public List<string>? ImagesUrls { get; set; }
    public int TypeVehiculeId { get; set; }
}

public class UpdateVehiculeRequest
{
    public int Id { get; set; }
    public string? Marque { get; set; }
    public string? Modele { get; set; }
    public string? Couleur { get; set; }
    public decimal PrixJournalier { get; set; }
    public int Kilometrage { get; set; }
    public bool EstDisponible { get; set; }
}
