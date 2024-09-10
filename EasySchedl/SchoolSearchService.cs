using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using EasySchedl.Models;

namespace EasySchedl
{
    public class SchoolSearchService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<School>> QuerySchools(string query)
        {
            var url = "http://localhost:8089/api/schools/search";
            var requestBody = new { query = query };
            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<SchoolSearchResult>(responseContent);
                var schools = new List<School>(result.Schools);

                // Debug output
                Console.WriteLine("School details:");
                foreach (var school in schools)
                {
                    Console.WriteLine($"Name: {school.Name}, Address: {school.Address}, Id: {school.Id}, IP: {school.Ip}");
                }

                return schools;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}");
                return new List<School>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}");
                return new List<School>();
            }
        }
    }
}