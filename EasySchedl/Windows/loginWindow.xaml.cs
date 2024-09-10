using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using EasySchedl.Models;
using EasySchedl.Windows;
using Newtonsoft.Json;

namespace EasySchedl
{
    public partial class loginWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly SchoolSearchService _schoolSearchService;

        public loginWindow()
        {
            InitializeComponent();
            _schoolSearchService = new SchoolSearchService();
            //Set position of the window to the center of the screen
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            String QRCodePath = "C:\\Users\\Arthur\\Desktop\\Projektarbeit UNTIS\\SampleQRCode.png";
            QRImage.Source = new BitmapImage(new Uri(QRCodePath));

            // Event handler for AutoCompleteBox
            ACBSchools.KeyUp += ACBSchools_OnKeyUp;
            BtnNext.Click += BtnNext_Click;
            // Set custom item filter
            ACBSchools.ItemFilter = SchoolFilter;
            
            
            //Login Window
            BtnLogin.Click += BtnLogin_Click;
        }
        private async void ACBSchools_OnKeyUp(object sender, KeyEventArgs e)
        {
            var textBox = sender as AutoCompleteBox;
            if (textBox != null && textBox.Text.Length > 2)
            {
                var query = textBox.Text;
                var schools = await _schoolSearchService.QuerySchools(query);
                if (schools != null && schools.Count > 0)
                {
                    textBox.ItemsSource = schools.Select(s => s.Name).ToList();
                    textBox.IsDropDownOpen = true;
                }
            }
        }

        private bool SchoolFilter(string search, object item)
        {
            var schoolName = item as string;
            return schoolName != null && schoolName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private async void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            var textBox = ACBSchools as AutoCompleteBox;
            if (textBox != null && textBox.Text.Length > 2)
            {
                var query = textBox.Text;
                var school = await _schoolSearchService.QuerySchools(query);
                if (school != null && school.Count > 0)
                {
                    AppConfig.School = school.First();
                    CSelectSchool.Visibility = Visibility.Collapsed;
                    CLogin.Visibility = Visibility.Visible;
                    Line1.Stroke = new SolidColorBrush(Color.FromRgb(242, 108, 108));
                    Ellipse2.Fill = new SolidColorBrush(Color.FromRgb(242, 108, 108));
                    txtSchoolname.Text = school.First().Name;
                }
            }
        }
        
        //////////////////////////// LOGIN WINDOW ////////////////////////////
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            /*var username = TBUser.Text;
            var password = TBPassword.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your username and password.");
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.");
                return;
            }*/
            
            
            // Load the mainWindow content
            var mainWindowContent = new mainWindow().Content;

            // Set the content of loginWindow to the content of mainWindow
            this.Width = 1200;
            this.Height = 700;
            this.Content = mainWindowContent;

            // Center the window on the screen
            CenterWindowOnScreen();
        }
        
        private void CenterWindowOnScreen()
        {
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            var windowLeft = (screenWidth - this.Width) / 2;
            var windowTop = (screenHeight - this.Height) / 2;
            this.Left = windowLeft;
            this.Top = windowTop;
        }
    }
}

//Sites used for this page:
//https://www.color-hex.com/color/f26c6c to get the rgb color of the Hex color
//https://stackoverflow.com/questions/22415136/in-a-wpf-program-i-want-to-change-the-stroke-color-on-all-the-lines-on-a-can