using System.Windows;
using System.Windows.Controls;

namespace CyberbotFinal.Pages
{
    public partial class TaskAssistantPage : Page
    {
        public TaskAssistantPage()
        {
            InitializeComponent();
        }

        private void AddReminder_Click(object sender, RoutedEventArgs e)
        {
            string task = ReminderInput.Text.Trim();
            if (!string.IsNullOrEmpty(task))
            {
                ReminderList.Items.Add(task);
                ReminderInput.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a task reminder.");
            }
        }
    }
}
