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

namespace TeamsHubDesktopClient.Gateways.Provider
{
    public class StudentManagementRESTProvider
    {
        public StudentManagementRESTProvider() { }

        public bool AddStudent(StudentDTO newStudent)
        {
            try
            {
                var result = HttpClientSingleton.Instance.PostAsJsonAsync($"/TeamHub/Users", newStudent).Result;
                result.EnsureSuccessStatusCode();
                var response = result.Content.ReadFromJsonAsync<Boolean>().Result;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddStudentToProject(int idStudent, int idProject)
        {
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.PostAsJsonAsync($"/TeamHub/Users/AddToProject/{idProject}/{idStudent}", (object)null).Result;
                result.EnsureSuccessStatusCode();
                var response = result.Content.ReadFromJsonAsync<Boolean>().Result;
                return response;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteStudentToProject(int idStudent, int idProject)
        {
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.DeleteAsync($"/TeamHub/Users/RemoveOfProject/{idProject}/{idStudent}").Result;
                result.EnsureSuccessStatusCode();
                var response = result.Content.ReadFromJsonAsync<Boolean>().Result;
                return response;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> EditStudent(StudentDTO editStudent)
        {
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var content = JsonContent.Create(editStudent);
                var response = await HttpClientSingleton.Instance.PutAsync("/TeamHub/Users/Edit", content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<bool>();

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<User> GetStudentsByProject(int idProject)
        {
            List<User> response = null;
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Users/ByProject/{idProject}").Result;
                result.EnsureSuccessStatusCode();
                response = result.Content.ReadFromJsonAsync<List<User>>().Result;
            }
            catch (System.Exception)
            {

                throw;
            }
            return response;
        }

        public User GetStudentInfo(string student)
        {
            User response = null;
            List<User> responseList = null;
            try
            {
                var encodedStudent = Uri.EscapeDataString(student);
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Users/Search/{encodedStudent}").Result;
                result.EnsureSuccessStatusCode();
                responseList = result.Content.ReadFromJsonAsync<List<User>>().Result;
                response = responseList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
            return response;
        }

        public StudentDTO GetUserPersonalData(int studentID)
        {
            StudentDTO response = null;
            try
            {
                HttpClientSingleton.SetAuthorizationHeader();
                var result = HttpClientSingleton.Instance.GetAsync($"/TeamHub/Users/GetUserInformation/{studentID}").Result;
                result.EnsureSuccessStatusCode();
                response = result.Content.ReadFromJsonAsync<StudentDTO>().Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
            return response;
        }
    }
}
