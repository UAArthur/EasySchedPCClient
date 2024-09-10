using System.Windows;

namespace EasySchedl.Windows
{
    public partial class mainWindow : Window
    {
        public mainWindow()
        {
            InitializeComponent();
            // Set the data context of the window to the view model
            NavSchoolname.Text = (AppConfig.School == null) ? "School Name" : AppConfig.School.Name;
            
        }
    }
}