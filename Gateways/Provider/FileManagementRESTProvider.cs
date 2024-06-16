using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TeamsHubDesktopClient.DTOs;
using TeamsHubDesktopClient.SinglentonClasses;

namespace TeamsHubDesktopClient.Gateways.Provider
{
    public class FileManagementRESTProvider
    {
        private readonly ILogger<FileManagementRESTProvider> _logger;

        public FileManagementRESTProvider(ILogger<FileManagementRESTProvider> logger) 
        {
            _logger = logger;
        }
        
        public async Task<List<DocumentDTO>>? GetAllFileByProjectAsync(int idProject)
        {
            List<DocumentDTO> documents;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = await HttpClientSingleton.Instance.GetAsync($"/TeamHub/File/{idProject}");
                result.EnsureSuccessStatusCode();
                documents = result.Content.ReadFromJsonAsync<List<DocumentDTO>>().Result;
            } 
            catch (Exception ex) 
            {
                documents = null;
                _logger.LogError(ex.Message);
            }

            return documents;
        }

    }
}
