using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TeamsHubDesktopClient.DTOs;
using TeamsHubDesktopClient.SinglentonClasses;

namespace TeamsHubDesktopClient.Gateways.Provider
{
    public class TaskManagementRESTProvider
    {

        ILogger<TaskManagementRESTProvider> _logger;
        public TaskManagementRESTProvider(ILogger<TaskManagementRESTProvider> logger) 
        {
            _logger = logger;
        }

        public async Task<bool> AddTaskAsync(TaskDTO newTask)
        {
            bool response;

            try
            {
                var json = JsonSerializer.Serialize(newTask);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClientSingleton.SetAuthorizationHeader();
                var request = new HttpRequestMessage(HttpMethod.Post, "/TeamHub/Task/") { Content = content };
                var result = await HttpClientSingleton.Instance.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    response = true;
                }
                else
                {
                    response = false;
                }
            }
            catch (Exception ex)
            {
                response = false;
                _logger.LogError(ex.Message);
            }

            return response;
        }

        public async Task<bool> UpdateTaskAsync(TaskDTO task)
        {
            bool response;

            try
            {
                var json = JsonSerializer.Serialize(task);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClientSingleton.SetAuthorizationHeader();
                var request = new HttpRequestMessage(HttpMethod.Post, "/TeamHub/Task/up") { Content = content };
                var result = await HttpClientSingleton.Instance.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    response = true;
                }
                else
                {
                    response = false;
                }
            }
            catch (Exception ex)
            {
                response = false;
                _logger.LogError(ex.Message);
            }

            return response;
        }

        public List<TaskDTO> GetAllTaskByProject(int projectID)
        {
            List<TaskDTO> response;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Task/{projectID}").Result;
                result.EnsureSuccessStatusCode();
                response = result.Content.ReadFromJsonAsync<List<TaskDTO>>().Result;
            }
            catch (Exception ex)
            {
                response = null;
                _logger.LogError(ex.Message);
            }

            return response;
        }

        public List<TaskDTO> GetTaskbyDate(DateTime startDate, DateTime endDate)
        {
            List<TaskDTO> response;

            try
            {
                var startDateFormat = $"{startDate.Year}-{startDate.Month:00}-{startDate.Day:00}";
                var endDateFormat = $"{endDate.Year}-{endDate.Month:00}-{endDate.Day:00}";
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Task/{startDateFormat}/{endDateFormat}").Result;
                result.EnsureSuccessStatusCode();
                response = result.Content.ReadFromJsonAsync<List<TaskDTO>>().Result;
            }
            catch (Exception ex)
            {
                response = null;
                _logger.LogError(ex.Message);
            }

            return response;
        }

        public async Task<bool> RemoveTaskAsync(int taskID)
        {
            bool result;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var requestUri = $"/TeamHub/Task/{taskID}";
                var response = await HttpClientSingleton.Instance.DeleteAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (HttpRequestException httpEx)
            {
                result = false;
                _logger.LogError(httpEx.Message);
            }
            catch (JsonException jsonEx)
            {
                result = false;
                _logger.LogError(jsonEx.Message);

            }
            catch (Exception ex)
            {
                result = false;
                _logger.LogError(ex.Message);

            }

            return result;
        }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
    }
}
