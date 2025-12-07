using System.Net.Http.Headers;

namespace Location_voiture_front_web.Handlers;

public class AuthorizationHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthorizationHeaderHandler> _logger;

    public AuthorizationHeaderHandler(IHttpContextAccessor httpContextAccessor, ILogger<AuthorizationHeaderHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Get token from session directly
        var session = _httpContextAccessor.HttpContext?.Session;
        var token = session?.GetString("auth_token");
        
        _logger.LogInformation($"AuthHandler: Session={session != null}, Token={(!string.IsNullOrEmpty(token) ? "exists" : "null")}, Request={request.RequestUri?.PathAndQuery}");
        
        if (!string.IsNullOrEmpty(token))
        {
            // Add Authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _logger.LogInformation("AuthHandler: Token added to Authorization header");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
