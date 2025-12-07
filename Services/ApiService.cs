using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using Location_voiture_front_web.Models;

namespace Location_voiture_front_web.Services;

public interface IApiService
{
    Task<ApiResponse<T>> GetAsync<T>(string endpoint);
    Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? data = null);
    Task<ApiResponse<T>> PutAsync<T>(string endpoint, object? data = null);
    Task<ApiResponse> DeleteAsync(string endpoint);
    void SetAuthorizationToken(string token);
    void ClearAuthorizationToken();
}

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public void SetAuthorizationToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearAuthorizationToken()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling GET {Endpoint}", endpoint);
            return new ApiResponse<T>
            {
                Success = false,
                Message = ex.Message,
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? data = null)
    {
        try
        {
            HttpContent? content = null;
            if (data != null)
            {
                var json = JsonSerializer.Serialize(data);
                content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.PostAsync(endpoint, content);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling POST {Endpoint}", endpoint);
            return new ApiResponse<T>
            {
                Success = false,
                Message = ex.Message,
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object? data = null)
    {
        try
        {
            HttpContent? content = null;
            if (data != null)
            {
                var json = JsonSerializer.Serialize(data);
                content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.PutAsync(endpoint, content);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling PUT {Endpoint}", endpoint);
            return new ApiResponse<T>
            {
                Success = false,
                Message = ex.Message,
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse> DeleteAsync(string endpoint)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse { Success = true };
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(content);
            
            return new ApiResponse
            {
                Success = false,
                Message = jsonResponse.GetProperty("message").GetString() ?? "Erreur lors de la suppression",
                Errors = new List<string> { "Erreur: " + response.StatusCode }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling DELETE {Endpoint}", endpoint);
            return new ApiResponse
            {
                Success = false,
                Message = ex.Message,
                Errors = new List<string> { ex.Message }
            };
        }
    }

    private async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                };
                
                // First, try to deserialize as direct T (for API responses that don't wrap in ApiResponse)
                var directData = JsonSerializer.Deserialize<T>(content, options);
                if (directData != null)
                {
                    return new ApiResponse<T>
                    {
                        Success = true,
                        Data = directData
                    };
                }
                
                // If that fails, try ApiResponse<T>
                var jsonResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, options);
                return jsonResponse ?? new ApiResponse<T> { Success = true, Data = directData };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing successful response: {Content}", content);
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = $"Erreur de désérialisation: {ex.Message}"
                };
            }
        }

        try
        {
            var options = new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            };
            
            // Check if content is empty or whitespace
            if (string.IsNullOrWhiteSpace(content))
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = $"Erreur: {response.StatusCode}"
                };
            }
            
            var errorResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, options);
            if (errorResponse != null)
            {
                return errorResponse;
            }

            // Try to extract message from plain object response
            using (JsonDocument doc = JsonDocument.Parse(content))
            {
                var root = doc.RootElement;
                if (root.TryGetProperty("message", out var messageProp))
                {
                    return new ApiResponse<T>
                    {
                        Success = false,
                        Message = messageProp.GetString()
                    };
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing error response: {Content}", content);
        }

        return new ApiResponse<T>
        {
            Success = false,
            Message = $"Erreur: {response.StatusCode}",
            Errors = new List<string> { content }
        };
    }
}
