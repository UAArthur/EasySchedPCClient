using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging; // Ensure this for controls like ListBox and ComboBox
using Newtonsoft.Json;

namespace EasySchedl
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();

            String QRCodePath = "C:\\Users\\Arthur\\Desktop\\Projektarbeit UNTIS\\SampleQRCode.png";
            QRImage.Source = new BitmapImage(new Uri(QRCodePath));
            
            //Event handler for AutoCompleteBox
            ACBSchools.KeyUp += ACBSchools_OnKeyUp;
        }
       
        private async void ACBSchools_OnKeyUp(object sender, KeyEventArgs e)
        {
            var textBox = sender as AutoCompleteBox;
            if (textBox != null && textBox.Text.Length > 2)
            {
                var query = textBox.Text;
                Console.WriteLine($"Text changed: {query}"); // Debug statement
                await querySchools(query);
                textBox.IsDropDownOpen = true;
            }
        }

        
        private async Task querySchools(string query)
        {
            var url = "http://localhost:8080/api/schools/search";

            var requestBody = new { query = query };
            var jsonContent = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // Debug statement to check request being sent
                Console.WriteLine($"Sending request to: {url}");
                Console.WriteLine($"Request body: {jsonContent}");

                var response = await client.PostAsync(url, content);

                // Debug statement to check response status
                Console.WriteLine($"Response status: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                // Debug statement to check response content
                Console.WriteLine($"Response content: {responseContent}");

                var result = JsonConvert.DeserializeObject<SchoolSearchResult>(responseContent);

                var schoolNames = new List<string>();
                foreach (var school in result.Schools)
                {
                    schoolNames.Add(school.Name);
                }

                // Debug statement to check list of school names
                Console.WriteLine("School names:");
                foreach (var name in schoolNames)
                {
                    Console.WriteLine(name);
                }

                // Assuming ACBSchools is a ListBox or ComboBox
                ACBSchools.ItemsSource = schoolNames;
            }
            catch (HttpRequestException ex)
            {
                // Handle request errors (e.g., show an error message)
                MessageBox.Show($"Request error: {ex.Message}");
                // Debug statement for error
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other possible errors
                MessageBox.Show($"An unexpected error occurred: {ex.Message}");
                // Debug statement for unexpected error
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }

    public class SchoolSearchResult
    {
        public int Status { get; set; }
        public School[] Schools { get; set; }
    }

    public class School
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Id { get; set; }
    }
}
