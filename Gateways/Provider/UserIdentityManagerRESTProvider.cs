using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeamsHubDesktopClient.DTOs;
using TeamsHubDesktopClient.SinglentonClasses;

namespace TeamsHubDesktopClient.Gateways.Provider
{
    public class UserIdentityManagerRESTProvider
    {
        ILogger<UserIdentityManagerRESTProvider> _logger;

        public UserIdentityManagerRESTProvider(ILogger<UserIdentityManagerRESTProvider> logger)
        {
            _logger = logger;
        }

        public async Task<UserValidationResponse> ValidateUserAsync(SessionLoginRequest sessionLoginRequest)
        {
            UserValidationResponse userValidationResponse;

            try
            {
                sessionLoginRequest.password = EncryptPassword(sessionLoginRequest.password);
                var response = await HttpClientSingleton.Instance.PostAsJsonAsync("/TeamHub/Sessions/validateUser", sessionLoginRequest);
                response.EnsureSuccessStatusCode();
                userValidationResponse = await response.Content.ReadFromJsonAsync<UserValidationResponse>();
                _logger.LogInformation("login exitoso");
            }
            catch (Exception e)
            {
                userValidationResponse = null;
            }

            return userValidationResponse;
        }

        public bool PasswordRecovery(string userEmail)
        {
            bool result;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var response = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Users/RecoveryPassword/{userEmail}").Result;
                response.EnsureSuccessStatusCode();
                result = true;
            }
            catch (System.Exception)
            {
                result = false;
            }

            return result;
        }

        private String EncryptPassword(String password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string passwordMD5 = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            
            return passwordMD5;
        }
    }
}
