using System.Net.Http.Json;
using System.Text.Json;
using CoworkingFrontend.Models;

namespace CoworkingBlazor.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string email, string password, string role);
        Task LogoutAsync();
        Task<string> GetTokenAsync();
        Task<string> GetRoleAsync();
        Task<string> GetUsernameAsync();
        bool IsAuthenticated { get; }
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private string _token = string.Empty;
        private string _role = string.Empty;
        private string _username = string.Empty;

        public bool IsAuthenticated => !string.IsNullOrEmpty(_token);

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Account/login",
                    new { username, password });

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;

                    _token = root.GetProperty("token").GetString();
                    _role = root.GetProperty("role").GetString();
                    _username = username;

                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        public async Task<bool> RegisterAsync(string username, string email, string password, string role)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Account/registre",
                    new { username, emailAddress = email, password, role });
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public Task LogoutAsync()
        {
            _token = string.Empty;
            _role = string.Empty;
            _username = string.Empty;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return Task.CompletedTask;
        }

        public Task<string> GetTokenAsync() => Task.FromResult(_token);
        public Task<string> GetRoleAsync() => Task.FromResult(_role);
        public Task<string> GetUsernameAsync() => Task.FromResult(_username);
    }
}