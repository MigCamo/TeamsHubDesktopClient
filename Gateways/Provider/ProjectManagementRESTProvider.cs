using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TeamHubServiceProjects.DTOs;
using TeamsHubDesktopClient.DTOs;
using TeamsHubDesktopClient.SinglentonClasses;

namespace TeamsHubDesktopClient.Gateways.Provider
{
    public class ProjectManagementRESTProvider
    {
        private readonly ILogger<ProjectManagementRESTProvider> _logger;

        public ProjectManagementRESTProvider(ILogger<ProjectManagementRESTProvider> logger) 
        {
            _logger = logger;
        }

        public async Task<List<ProjectDTO>> GetAllMyProjectsAsync(int studentID)
        {
            List<ProjectDTO> projects;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var response = await HttpClientSingleton.Instance.GetAsync($"/TeamHub/Projects/MyProjects/{studentID}");
                response.EnsureSuccessStatusCode();
                projects = await response.Content.ReadFromJsonAsync<List<ProjectDTO>>();
            }
            catch (Exception ex)
            {
                projects = null;
                _logger.LogError(ex.Message);
            }

            return projects;
        }

        public bool AddProject(ProjectDTO project, int idStudent)
        {
            bool result;

            try
            {
                var request = new { ProjectNew = project, StudentID = idStudent };
                HttpClientSingleton.SetAuthorizationHeader();
                var response = HttpClientSingleton.Instance.PostAsJsonAsync($"/TeamHub/Projects/AddProject", request).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadFromJsonAsync<Boolean>().Result;
            }
            catch (Exception ex)
            {
                result = false;
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public ProjectDTO GetProject(int idProject)
        {
            ProjectDTO projectDTO;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Projects/Project/{idProject}").Result;
                result.EnsureSuccessStatusCode();
                projectDTO = result.Content.ReadFromJsonAsync<ProjectDTO>().Result;
            }
            catch (Exception ex)
            {
                projectDTO = null;
                _logger.LogError(ex.Message);
            }

            return projectDTO;
        }

        public List<ProjectDTO> GetProjectsbyDate(DateTime startDate, DateTime endDate)
        {
            List<ProjectDTO> projects;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var startDateFormat = $"{startDate.Year}-{startDate.Month:00}-{startDate.Day:00}";
                var endDateFormat = $"{endDate.Year}-{endDate.Month:00}-{endDate.Day:00}";
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Projects/MyProjects/{startDateFormat}/{endDateFormat}").Result;
                result.EnsureSuccessStatusCode();
                projects = result.Content.ReadFromJsonAsync<List<ProjectDTO>>().Result;
            }
            catch (Exception ex)
            {
                projects = null;
                _logger.LogError(ex.Message);
            }

            return projects;
        }

        public async Task<bool> RemoveProjectAsync(ProjectDTO project)
        {
            bool result;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var requestUri = $"/TeamHub/Projects/DeleteProject?idProject={project.IdProject}";
                var response = await HttpClientSingleton.Instance.DeleteAsync(requestUri);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                result = false;
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<bool> UpdateProjectAsync(ProjectDTO projectNew)
        {
            bool result;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var content = JsonContent.Create(projectNew);
                var response = await HttpClientSingleton.Instance.PutAsync("/TeamHub/Projects/UpdateProject", content);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                result = false;
                _logger.LogError(ex.Message);
            }

            return result;
        }
    }
}
