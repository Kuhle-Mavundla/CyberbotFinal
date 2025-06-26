using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CyberbotFinal.Pages
{
    public partial class TaskAssistantPage : Page
    {
        private DispatcherTimer reminderTimer;
        private List<Reminder> reminders = new List<Reminder>();
        private Reminder selectedReminder = null;

        public TaskAssistantPage()
        {
            InitializeComponent();
            reminderTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            reminderTimer.Tick += ReminderTimer_Tick;
            reminderTimer.Start();
        }

        private void AddReminder_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetReminderInput(out string description, out DateTime dueTime)) return;

            Reminder newReminder = new Reminder
            {
                Description = description,
                DueTime = dueTime
            };

            reminders.Add(newReminder);
            RefreshList();
            ClearInput();
        }

        private void UpdateReminder_Click(object sender, RoutedEventArgs e)
        {
            if (selectedReminder == null)
            {
                MessageBox.Show("Select a reminder to update.");
                return;
            }

            if (!TryGetReminderInput(out string newText, out DateTime newTime)) return;

            selectedReminder.Description = newText;
            selectedReminder.DueTime = newTime;
            selectedReminder.IsTriggered = false;

            RefreshList();
            ClearInput();
        }

        private void DeleteReminder_Click(object sender, RoutedEventArgs e)
        {
            if (selectedReminder == null)
            {
                MessageBox.Show("Select a reminder to delete.");
                return;
            }

            reminders.Remove(selectedReminder);
            selectedReminder = null;
            RefreshList();
            ClearInput();
        }

        private void ReminderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReminderList.SelectedIndex == -1) return;

            selectedReminder = reminders[ReminderList.SelectedIndex];
            ReminderInput.Text = selectedReminder.Description;
            DatePicker.SelectedDate = selectedReminder.DueTime.Date;
            TimeInput.Text = selectedReminder.DueTime.ToString("HH:mm");
        }

        private void ReminderTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            foreach (var r in reminders.Where(r => r.DueTime <= now && !r.IsTriggered))
            {
                r.IsTriggered = true;
                MessageBox.Show($"🔔 Reminder: {r.Description}", "Task Due", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshList()
        {
            ReminderList.Items.Clear();
            foreach (var r in reminders)
            {
                ReminderList.Items.Add($"{r.Description} at {r.DueTime:HH:mm dd MMM}");
            }
        }

        private void ClearInput()
        {
            ReminderInput.Clear();
            TimeInput.Text = "";
            DatePicker.SelectedDate = null;
            ReminderList.SelectedIndex = -1;
            selectedReminder = null;
        }

        private bool TryGetReminderInput(out string description, out DateTime dueTime)
        {
            description = ReminderInput.Text.Trim();
            dueTime = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please enter a reminder description.");
                return false;
            }

            if (!DatePicker.SelectedDate.HasValue || string.IsNullOrWhiteSpace(TimeInput.Text))
            {
                MessageBox.Show("Please select both a date and time.");
                return false;
            }

            if (!TimeSpan.TryParse(TimeInput.Text, out TimeSpan time))
            {
                MessageBox.Show("Invalid time format. Use HH:mm.");
                return false;
            }

            dueTime = DatePicker.SelectedDate.Value.Date + time;
            if (dueTime <= DateTime.Now)
            {
                MessageBox.Show("Time must be in the future.");
                return false;
            }

            return true;
        }

        private class Reminder
        {
            public string Description { get; set; }
            public DateTime DueTime { get; set; }
            public bool IsTriggered { get; set; } = false;
        }
    }
}
