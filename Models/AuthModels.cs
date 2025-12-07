namespace Location_voiture_front_web.Models;

public class LoginRequest
{
    public string? Email { get; set; }
    public string? MotDePasse { get; set; }
}

public class LoginResponse
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
    public UserInfoDto? User { get; set; }
}

public class UserInfoDto
{
    public int Id { get; set; }
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Email { get; set; }
    public string? TypeUtilisateur { get; set; }
}

public class RegisterRequest
{
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Email { get; set; }
    public string? MotDePasse { get; set; }
    public string? MotDePasseConfirm { get; set; }
    public string? Telephone { get; set; }
    public string? Adresse { get; set; }
    public string? NumeroPermis { get; set; }
}

public class UtilisateurDTO
{
    public int Id { get; set; }
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Email { get; set; }
    public string? TypeUtilisateur { get; set; }
    public bool EstActif { get; set; }
    public DateTime DateCreation { get; set; }
}
