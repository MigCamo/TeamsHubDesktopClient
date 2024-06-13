using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TeamsHubDesktopClient.SinglentonClasses
{
    public sealed class HttpClientSingleton
    {
        private static readonly Lazy<HttpClient> lazyHttpClient = new Lazy<HttpClient>(() =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8081/")
            };
            return client;
        });

        public static HttpClient Instance => lazyHttpClient.Value;

        public static string UserToken { get; set; }

        private HttpClientSingleton() { }

        public static void SetAuthorizationHeader()
        {
            if (!string.IsNullOrWhiteSpace(UserToken))
            {
                Instance.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserToken);
            }
        }
    }
}
