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

        public async Task<bool> AddTask(TaskDTO newTask)
        {
            try
            {
                var json = JsonSerializer.Serialize(newTask);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, "/TeamHub/Task/")
                {
                    Content = content
                };

                HttpClientSingleton.SetAuthorizationHeader();
                var result = await HttpClientSingleton.Instance.SendAsync(request);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {result.StatusCode}, Content: {responseContent}");
                    return false;
                }
                var responseContentSuccess = await result.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContentSuccess}");

                bool response;
                if (bool.TryParse(responseContentSuccess, out response))
                {
                    return response;
                }
                else
                {
                    Console.WriteLine("La respuesta no se pudo convertir a booleano.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateTask(TaskDTO task)
        {
            try
            {
                var json = JsonSerializer.Serialize(task);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClientSingleton.SetAuthorizationHeader();
                var request = new HttpRequestMessage(HttpMethod.Post, "/TeamHub/Task/up")
                {
                    Content = content
                };
                var result = await HttpClientSingleton.Instance.SendAsync(request);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {result.StatusCode}, Content: {responseContent}");
                    return false;
                }
                var responseContentSuccess = await result.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContentSuccess}");

                bool response;
                if (bool.TryParse(responseContentSuccess, out response))
                {
                    return response;
                }
                else
                {
                    Console.WriteLine("La respuesta no se pudo convertir a booleano.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public List<TaskDTO> GetAllTaskByProject(int projectID)
        {
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Task/{projectID}").Result;
                result.EnsureSuccessStatusCode();
                var response = result.Content.ReadFromJsonAsync<List<TaskDTO>>().Result;
                return response;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<TaskDTO> GetTaskbyDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                var startDateFormat = $"{startDate.Year}-{startDate.Month:00}-{startDate.Day:00}";
                var endDateFormat = $"{endDate.Year}-{endDate.Month:00}-{endDate.Day:00}";
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Task/{startDateFormat}/{endDateFormat}").Result;
                result.EnsureSuccessStatusCode();
                var response = result.Content.ReadFromJsonAsync<List<TaskDTO>>().Result;
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        public bool RemoveTask(int taskID)
        {
            return true;
        }
    }
}
