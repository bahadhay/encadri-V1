using Azure.Communication.Identity;
using Azure.Communication;
using Azure;

namespace Encadri_Backend.Services
{
    /// <summary>
    /// Service for Azure Communication Services (Video calls, chat, etc.)
    /// </summary>
    public interface IAzureCommunicationService
    {
        Task<string> CreateUserAndGetToken();
        Task<string> GetTokenForUser(string userId);
    }

    public class AzureCommunicationService : IAzureCommunicationService
    {
        private readonly CommunicationIdentityClient? _client;
        private readonly bool _isConfigured;

        public AzureCommunicationService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureCommunication:ConnectionString"]
                ?? Environment.GetEnvironmentVariable("AZURE_COMMUNICATION_CONNECTION_STRING");

            // Check if connection string is valid (not a placeholder)
            if (string.IsNullOrEmpty(connectionString) ||
                connectionString.Contains("YOUR_RESOURCE_NAME") ||
                connectionString.Contains("YOUR_ACCESS_KEY"))
            {
                Console.WriteLine("Warning: Azure Communication Services is not configured. Video calling will not be available.");
                _isConfigured = false;
                _client = null;
            }
            else
            {
                try
                {
                    _client = new CommunicationIdentityClient(connectionString);
                    _isConfigured = true;
                    Console.WriteLine("Azure Communication Services initialized successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Failed to initialize Azure Communication Services: {ex.Message}");
                    _isConfigured = false;
                    _client = null;
                }
            }
        }

        /// <summary>
        /// Create a new ACS user and generate an access token for video calling
        /// </summary>
        public async Task<string> CreateUserAndGetToken()
        {
            if (!_isConfigured || _client == null)
            {
                throw new InvalidOperationException(
                    "Azure Communication Services is not configured. " +
                    "Please set up ACS connection string to enable video calling."
                );
            }

            try
            {
                // Create a new user
                var userResponse = await _client.CreateUserAsync();
                var user = userResponse.Value;

                // Generate token with VoIP scope (for video/audio calls)
                var tokenResponse = await _client.GetTokenAsync(
                    user,
                    scopes: new[] { CommunicationTokenScope.VoIP }
                );

                // Return token (frontend will use this to join calls)
                return tokenResponse.Value.Token;
            }
            catch (RequestFailedException ex)
            {
                throw new Exception($"Failed to create ACS user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get a new token for an existing ACS user
        /// </summary>
        public async Task<string> GetTokenForUser(string userId)
        {
            if (!_isConfigured || _client == null)
            {
                throw new InvalidOperationException(
                    "Azure Communication Services is not configured. " +
                    "Please set up ACS connection string to enable video calling."
                );
            }

            try
            {
                var user = new CommunicationUserIdentifier(userId);
                var tokenResponse = await _client.GetTokenAsync(
                    user,
                    scopes: new[] { CommunicationTokenScope.VoIP }
                );

                return tokenResponse.Value.Token;
            }
            catch (RequestFailedException ex)
            {
                throw new Exception($"Failed to get token for user {userId}: {ex.Message}", ex);
            }
        }
    }
}
