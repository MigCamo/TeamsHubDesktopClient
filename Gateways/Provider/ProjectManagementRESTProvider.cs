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
        public ProjectManagementRESTProvider() { }

        public async Task<List<ProjectDTO>> GetAllMyProjectsAsync(int studentID)
        {
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var response = await HttpClientSingleton.Instance.GetAsync($"/TeamHub/Projects/MyProjects/{studentID}");
                response.EnsureSuccessStatusCode();
                var projects = await response.Content.ReadFromJsonAsync<List<ProjectDTO>>();
                return projects;
            }
            catch (Exception ex)
            {
                return new List<ProjectDTO>();
            }
        }

        public bool AddProject(ProjectDTO project, int idStudent)
        {
            try
            {
                var request = new { ProjectNew = project, StudentID = idStudent };
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.PostAsJsonAsync($"/TeamHub/Projects/AddProject", request).Result;
                result.EnsureSuccessStatusCode();
                var response = result.Content.ReadFromJsonAsync<Boolean>().Result;
                return response;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ProjectDTO GetProject(int idProject)
        {
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Projects/Project/{idProject}").Result;
                result.EnsureSuccessStatusCode();
                var response = result.Content.ReadFromJsonAsync<ProjectDTO>().Result;
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ProjectDTO> GetProjectsbyDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var startDateFormat = $"{startDate.Year}-{startDate.Month:00}-{startDate.Day:00}";
                var endDateFormat = $"{endDate.Year}-{endDate.Month:00}-{endDate.Day:00}";
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Projects/MyProjects/{startDateFormat}/{endDateFormat}").Result;
                result.EnsureSuccessStatusCode();
                var response = result.Content.ReadFromJsonAsync<List<ProjectDTO>>().Result;
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> RemoveProjectAsync(ProjectDTO project)
        {
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var requestUri = $"/TeamHub/Projects/DeleteProject?idProject={project.IdProject}";
                var response = await HttpClientSingleton.Instance.DeleteAsync(requestUri);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<bool>();

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateProjectAsync(ProjectDTO projectNew)
        {
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var content = JsonContent.Create(projectNew);
                var response = await HttpClientSingleton.Instance.PutAsync("/TeamHub/Projects/UpdateProject", content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<bool>();

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
