using System; // Imports the System namespace, providing fundamental classes and base types like DateTime, TimeSpan.
using System.Collections.Generic; // Imports the namespace for generic collections like List.
using System.Linq; // Imports the namespace for Language Integrated Query (LINQ), used for querying collections.
using System.Windows; // Imports the namespace for Windows Presentation Foundation (WPF) core elements, like MessageBox, RoutedEventArgs.
using System.Windows.Controls; // Imports the namespace for WPF controls like Page, ListBox, TextBox, DatePicker, Button.
using System.Windows.Threading; // Imports the namespace for DispatcherTimer, used for UI-thread-bound timers.

namespace CyberbotFinal.Pages // Defines the namespace for the pages within the CyberbotFinal application.
{
    public partial class TaskAssistantPage : Page // Declares the TaskAssistantPage class, which is a partial class inheriting from Page.
    {
        private DispatcherTimer reminderTimer; // Declares a DispatcherTimer object to periodically check for due reminders.
        private List<Reminder> reminders = new List<Reminder>(); // Declares a list to store Reminder objects.
        private Reminder selectedReminder = null; // Declares a variable to hold the currently selected reminder in the ListBox.

        public TaskAssistantPage() // Constructor for the TaskAssistantPage class.
        {
            InitializeComponent(); // Initializes the components defined in the XAML file for this page.
            reminderTimer = new DispatcherTimer // Creates a new instance of DispatcherTimer.
            {
                Interval = TimeSpan.FromSeconds(10) // Sets the timer to tick every 10 seconds.
            };
            reminderTimer.Tick += ReminderTimer_Tick; // Subscribes the ReminderTimer_Tick method to the timer's Tick event.
            reminderTimer.Start(); // Starts the timer.
        }

        private void AddReminder_Click(object sender, RoutedEventArgs e) // Event handler for the "Add Reminder" button click.
        {
            // Tries to get the reminder description and due time from the input fields.
            // If input is invalid, it returns false and the method exits.
            if (!TryGetReminderInput(out string description, out DateTime dueTime)) return;

            Reminder newReminder = new Reminder // Creates a new Reminder object.
            {
                Description = description, // Sets the description from user input.
                DueTime = dueTime // Sets the due time from user input.
            };

            reminders.Add(newReminder); // Adds the new reminder to the list of reminders.
            RefreshList(); // Refreshes the display of reminders in the ListBox.
            ClearInput(); // Clears the input fields.
        }

        private void UpdateReminder_Click(object sender, RoutedEventArgs e) // Event handler for the "Update Reminder" button click.
        {
            if (selectedReminder == null) // Checks if a reminder is selected in the ListBox.
            {
                MessageBox.Show("Select a reminder to update."); // Shows a message if no reminder is selected.
                return; // Exits the method.
            }

            // Tries to get the new reminder text and time from the input fields.
            // If input is invalid, it returns false and the method exits.
            if (!TryGetReminderInput(out string newText, out DateTime newTime)) return;

            selectedReminder.Description = newText; // Updates the description of the selected reminder.
            selectedReminder.DueTime = newTime; // Updates the due time of the selected reminder.
            selectedReminder.IsTriggered = false; // Resets the triggered status, in case it was previously triggered.

            RefreshList(); // Refreshes the display of reminders in the ListBox.
            ClearInput(); // Clears the input fields.
        }

        private void DeleteReminder_Click(object sender, RoutedEventArgs e) // Event handler for the "Delete Reminder" button click.
        {
            if (selectedReminder == null) // Checks if a reminder is selected in the ListBox.
            {
                MessageBox.Show("Select a reminder to delete."); // Shows a message if no reminder is selected.
                return; // Exits the method.
            }

            reminders.Remove(selectedReminder); // Removes the selected reminder from the list.
            selectedReminder = null; // Clears the selected reminder reference.
            RefreshList(); // Refreshes the display of reminders in the ListBox.
            ClearInput(); // Clears the input fields.
        }

        private void ReminderList_SelectionChanged(object sender, SelectionChangedEventArgs e) // Event handler for when the selection in the ReminderList changes.
        {
            if (ReminderList.SelectedIndex == -1) return; // If no item is selected (e.g., selection cleared), exit the method.

            selectedReminder = reminders[ReminderList.SelectedIndex]; // Sets the selectedReminder to the Reminder object corresponding to the selected item's index.
            ReminderInput.Text = selectedReminder.Description; // Populates the ReminderInput TextBox with the selected reminder's description.
            DatePicker.SelectedDate = selectedReminder.DueTime.Date; // Sets the DatePicker to the date of the selected reminder's due time.
            TimeInput.Text = selectedReminder.DueTime.ToString("HH:mm"); // Populates the TimeInput TextBox with the time of the selected reminder's due time in HH:mm format.
        }

        private void ReminderTimer_Tick(object sender, EventArgs e) // Event handler for the DispatcherTimer's Tick event.
        {
            var now = DateTime.Now; // Gets the current date and time.
            // Iterates through reminders that are due (DueTime is less than or equal to now) and have not yet been triggered.
            foreach (var r in reminders.Where(r => r.DueTime <= now && !r.IsTriggered))
            {
                r.IsTriggered = true; // Marks the reminder as triggered.
                // Displays a message box notification for the due reminder.
                MessageBox.Show($"🔔 Reminder: {r.Description}", "Task Due", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshList() // Private method to clear and repopulate the ReminderList ListBox.
        {
            ReminderList.Items.Clear(); // Clears all existing items from the ListBox.
            foreach (var r in reminders) // Iterates through each Reminder object in the 'reminders' list.
            {
                // Adds a formatted string for each reminder to the ListBox, showing description and due time.
                ReminderList.Items.Add($"{r.Description} at {r.DueTime:HH:mm dd MMM}");
            }
        }

        private void ClearInput() // Private method to clear the input fields and reset selection.
        {
            ReminderInput.Clear(); // Clears the text in the ReminderInput TextBox.
            TimeInput.Text = ""; // Clears the text in the TimeInput TextBox.
            DatePicker.SelectedDate = null; // Clears the selected date in the DatePicker.
            ReminderList.SelectedIndex = -1; // Deselects any item in the ReminderList.
            selectedReminder = null; // Clears the reference to the selected reminder.
        }

        private bool TryGetReminderInput(out string description, out DateTime dueTime) // Private method to validate and parse user input for a reminder.
        {
            description = ReminderInput.Text.Trim(); // Gets the description from the ReminderInput TextBox and trims whitespace.
            dueTime = DateTime.MinValue; // Initializes dueTime to its minimum value.

            if (string.IsNullOrWhiteSpace(description)) // Checks if the description is empty or just whitespace.
            {
                MessageBox.Show("Please enter a reminder description."); // Prompts the user to enter a description.
                return false; // Returns false, indicating invalid input.
            }

            if (!DatePicker.SelectedDate.HasValue || string.IsNullOrWhiteSpace(TimeInput.Text)) // Checks if a date is selected and time input is not empty.
            {
                MessageBox.Show("Please select both a date and time."); // Prompts the user to select both date and time.
                return false; // Returns false.
            }

            if (!TimeSpan.TryParse(TimeInput.Text, out TimeSpan time)) // Tries to parse the time input string into a TimeSpan object.
            {
                MessageBox.Show("Invalid time format. Use HH:mm."); // Prompts for correct time format if parsing fails.
                return false; // Returns false.
            }

            dueTime = DatePicker.SelectedDate.Value.Date + time; // Combines the selected date (only date part) with the parsed time to get the full due time.
            if (dueTime <= DateTime.Now) // Checks if the calculated due time is in the past or present.
            {
                MessageBox.Show("Time must be in the future."); // Prompts the user that the time must be in the future.
                return false; // Returns false.
            }

            return true; // Returns true, indicating valid input.
        }

        private class Reminder // Defines a private nested class to represent a single reminder.
        {
            public string Description { get; set; } // Property to store the reminder's description.
            public DateTime DueTime { get; set; } // Property to store the reminder's due date and time.
            public bool IsTriggered { get; set; } = false; // Property to track if the reminder has been triggered, initialized to false.
        }
    }
}
