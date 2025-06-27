using System.Collections.Generic; // Imports the namespace for generic collections like Queue.
using System.Windows.Controls; // Imports the namespace for WPF controls like Page.

namespace CyberbotFinal.Pages // Defines the namespace for the pages within the CyberbotFinal application.
{
    public partial class ActivityLogPage : Page // Declares the ActivityLogPage class, which is a partial class inheriting from Page.
    {
        // Declares a static Queue to store activity log messages.
        // It's static so that all instances of ActivityLogPage (and other parts of the application) share the same log.
        // Initialized with a capacity of 10, implying it will store up to 10 recent activities.
        private static Queue<string> activityQueue = new Queue<string>(10); // FIFO structure (First-In, First-Out)

        // Declares a static field to hold a reference to the current instance of ActivityLogPage.
        // This allows static methods (like LogAction) to interact with the UI elements of the page.
        private static ActivityLogPage _instance;

        public ActivityLogPage() // Constructor for the ActivityLogPage class.
        {
            InitializeComponent(); // Initializes the components defined in the XAML file for this page.
            _instance = this; // Sets the static _instance field to the current instance of the page.
            RefreshLog(); // Calls RefreshLog to display any existing log entries when the page is loaded.
        }

        // Static method to add a new action message to the activity log.
        // It can be called from anywhere in the application without needing an instance of ActivityLogPage.
        public static void LogAction(string message)
        {
            if (activityQueue.Count >= 10) // Checks if the queue has reached its maximum capacity (10 entries).
            {
                activityQueue.Dequeue(); // If full, removes the oldest message from the front of the queue.
            }

            activityQueue.Enqueue(message); // Adds the new message to the end of the queue.

            // If an instance of ActivityLogPage exists (_instance is not null),
            // it calls the RefreshLog method on that instance to update the UI.
            _instance?.RefreshLog();
        }

        // Private method to clear and repopulate the ActivityList ListBox with current log entries.
        private void RefreshLog()
        {
            ActivityList.Items.Clear(); // Clears all existing items from the ListBox.
            foreach (var action in activityQueue) // Iterates through each message in the activityQueue.
            {
                ActivityList.Items.Add(action); // Adds each message as a new item to the ListBox.
            }
        }
    }
}
