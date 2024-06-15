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
        public UserIdentityManagerRESTProvider(){}

        public UserValidationResponse ValidateUser(SessionLoginRequest sessionLoginRequest)
        {
            try
            {
                byte[] encodedPassword = new UTF8Encoding().GetBytes(sessionLoginRequest.password);
                byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
                string passwordMD5 = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
                //sessionLoginRequest.password = passwordMD5;

                var resultado = HttpClientSingleton.Instance.PostAsJsonAsync<SessionLoginRequest>($"/TeamHub/Sessions/validateUser", sessionLoginRequest).Result;
                resultado.EnsureSuccessStatusCode();
                var respuesta = resultado.Content.ReadFromJsonAsync<UserValidationResponse>().Result;
                return respuesta;
            }
            catch (Exception e)
            {
                return new UserValidationResponse() { IsValid = false };
            }
        }

        public async Task<UserValidationResponse> ValidateUserAsync(SessionLoginRequest sessionLoginRequest)
        {
            try
            {
                byte[] encodedPassword = new UTF8Encoding().GetBytes(sessionLoginRequest.password);
                byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
                string passwordMD5 = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
                //sessionLoginRequest.password = passwordMD5;

                var response = await HttpClientSingleton.Instance.PostAsJsonAsync("/TeamHub/Sessions/validateUser", sessionLoginRequest);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<UserValidationResponse>();
                return result;
            }
            catch (Exception e)
            {
                return new UserValidationResponse { IsValid = false };
            }
        }

        public bool PasswordRecovery(string userEmail)
        {
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Users/RecoveryPassword/{userEmail}").Result;
                result.EnsureSuccessStatusCode();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
