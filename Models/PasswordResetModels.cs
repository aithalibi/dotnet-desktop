namespace Location_voiture_front_web.Models;

public class ForgotPasswordRequest
{
    public string? Email { get; set; }
}

public class VerifyResetCodeRequest
{
    public string? Email { get; set; }
    public string? Code { get; set; }
}

public class ResetPasswordRequest
{
    public string? Email { get; set; }
    public string? Code { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }
}

public class ForgotPasswordResponse
{
    public string? Message { get; set; }
}

public class VerifyCodeResponse
{
    public string? Message { get; set; }
    public bool Valid { get; set; }
}
