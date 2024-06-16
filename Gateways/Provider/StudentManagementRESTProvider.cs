using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsHubDesktopClient.DTOs;
using TeamsHubDesktopClient.SinglentonClasses;
using System.Net.Http.Json;
using System.Security.Cryptography;
using TeamHubServiceProjects.DTOs;
using Microsoft.Extensions.Logging;
using TeamsHubDesktopClient.Resources;

namespace TeamsHubDesktopClient.Gateways.Provider
{
    public class StudentManagementRESTProvider
    {
        private readonly ILogger<StudentManagementRESTProvider> _logger;

        public StudentManagementRESTProvider(ILogger<StudentManagementRESTProvider> logger) 
        {
            _logger = logger;
        }

        public int AddStudent(StudentDTO newStudent)
        {
            int result;

            try
            {
                var response = HttpClientSingleton.Instance.PostAsJsonAsync($"/TeamHub/Users", newStudent).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadFromJsonAsync<Int16>().Result;
            }
            catch (Exception ex)
            {
                result = (int)ServerResponse.ErrorServer;
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public int AddStudentToProject(int idStudent, int idProject)
        {
            int result;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var response = HttpClientSingleton.Instance.PostAsJsonAsync($"/TeamHub/Users/AddToProject/{idProject}/{idStudent}", (object)null).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadFromJsonAsync<Int16>().Result;
            }
            catch (Exception ex)
            {
                result = (int)ServerResponse.ErrorServer;
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public int DeleteStudentToProject(int idStudent, int idProject)
        {
            int result;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var response = HttpClientSingleton.Instance.DeleteAsync($"/TeamHub/Users/RemoveOfProject/{idProject}/{idStudent}").Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadFromJsonAsync<Int16>().Result;
            }
            catch (Exception ex)
            {
                result = (int)ServerResponse.ErrorServer;
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<int> EditStudent(StudentDTO editStudent)
        {
            int result;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var content = JsonContent.Create(editStudent);
                var response = await HttpClientSingleton.Instance.PutAsync("/TeamHub/Users/Edit", content);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadFromJsonAsync<Int16>();
            }
            catch (Exception ex)
            {
                result = (int)ServerResponse.ErrorServer;
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public List<User> GetStudentsByProject(int idProject)
        {
            List<User> response;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Users/ByProject/{idProject}").Result;
                result.EnsureSuccessStatusCode();
                response = result.Content.ReadFromJsonAsync<List<User>>().Result;
            }
            catch (Exception ex)
            {
                response = null;
                _logger.LogError(ex.Message);
            }

            return response;
        }

        public User GetStudentInfo(string student)
        {
            User user;
            List<User> responseList;

            try
            {
                var encodedStudent = Uri.EscapeDataString(student);
                HttpClientSingleton.SetAuthorizationHeader();
                var response = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Users/Search/{encodedStudent}").Result;
                response.EnsureSuccessStatusCode();
                responseList = response.Content.ReadFromJsonAsync<List<User>>().Result;
                user = responseList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                user = null;
                _logger.LogError(ex.Message);
            }

            return user;
        }

        public StudentDTO GetUserPersonalData(int studentID)
        {
            StudentDTO response;

            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Users/GetUserInformation/{studentID}").Result;
                result.EnsureSuccessStatusCode();
                response = result.Content.ReadFromJsonAsync<StudentDTO>().Result;
            }
            catch (Exception ex)
            {
                response = null;
                _logger.LogError(ex.Message);
            }

            return response;
        }
    }
}
