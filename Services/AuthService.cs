using Location_voiture_front_web.Models;

namespace Location_voiture_front_web.Services;

public interface IAuthService
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<UtilisateurDTO>> RegisterAsync(RegisterRequest request);
    Task<ApiResponse> LogoutAsync();
    string? GetToken();
    void SetToken(string token);
    void ClearToken();
    UtilisateurDTO? GetCurrentUser();
    void SetCurrentUser(UtilisateurDTO user);
    
    // Password Reset
    Task<ApiResponse<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<ApiResponse<VerifyCodeResponse>> VerifyResetCodeAsync(VerifyResetCodeRequest request);
    Task<ApiResponse> ResetPasswordAsync(ResetPasswordRequest request);
}

public class AuthService : IAuthService
{
    private readonly IApiService _apiService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string TokenSessionKey = "auth_token";
    private const string UserSessionKey = "current_user";

    public AuthService(IApiService apiService, IHttpContextAccessor httpContextAccessor)
    {
        _apiService = apiService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var response = await _apiService.PostAsync<LoginResponse>("/api/Auth/login", request);
        
        if (response.Success && response.Data != null)
        {
            // Save token to session - AuthorizationHeaderHandler will pick it up automatically
            SetToken(response.Data.Token ?? string.Empty);
        }

        return response;
    }

    public async Task<ApiResponse<UtilisateurDTO>> RegisterAsync(RegisterRequest request)
    {
        return await _apiService.PostAsync<UtilisateurDTO>("/api/Auth/register", request);
    }

    public async Task<ApiResponse> LogoutAsync()
    {
        ClearToken();
        _apiService.ClearAuthorizationToken();
        return new ApiResponse { Success = true };
    }

    public string? GetToken()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        return session?.GetString(TokenSessionKey);
    }

    public void SetToken(string token)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session != null)
        {
            session.SetString(TokenSessionKey, token);
        }
    }

    public void ClearToken()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.Remove(TokenSessionKey);
        session?.Remove(UserSessionKey);
    }

    public UtilisateurDTO? GetCurrentUser()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var userJson = session?.GetString(UserSessionKey);
        
        if (string.IsNullOrEmpty(userJson))
            return null;

        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<UtilisateurDTO>(userJson, 
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch
        {
            return null;
        }
    }

    public void SetCurrentUser(UtilisateurDTO user)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session != null)
        {
            var userJson = System.Text.Json.JsonSerializer.Serialize(user);
            session.SetString(UserSessionKey, userJson);
        }
    }

    public async Task<ApiResponse<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        return await _apiService.PostAsync<ForgotPasswordResponse>("/api/Auth/forgot-password", request);
    }

    public async Task<ApiResponse<VerifyCodeResponse>> VerifyResetCodeAsync(VerifyResetCodeRequest request)
    {
        return await _apiService.PostAsync<VerifyCodeResponse>("/api/Auth/verify-reset-code", request);
    }

    public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var response = await _apiService.PostAsync<object>("/api/Auth/reset-password", request);
        return new ApiResponse
        {
            Success = response.Success,
            Message = response.Message,
            Errors = response.Errors
        };
    }
}
