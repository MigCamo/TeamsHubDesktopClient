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
        public FileManagementRESTProvider() { }

        public bool AddFile() 
        { 
            return true; 
        }

        public bool DeleteFile() 
        { 
            return true; 
        }
        
        public async Task<List<DocumentDTO>>? GetAllFileByProjectAsync(int idProject)
        {
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = await HttpClientSingleton.Instance.GetAsync($"/TeamHub/File/{idProject}");
                result.EnsureSuccessStatusCode();
                var response = result.Content.ReadFromJsonAsync<List<DocumentDTO>>().Result;
                return response;
            } 
            catch (Exception ex) 
            {
                return new List<DocumentDTO>();
            }
        }
        
        public bool GetFileInfo(int idFile) 
        {
            return true; 
        }

        public bool GetFile(string path)
        {
            return true;
        }


    }
}
