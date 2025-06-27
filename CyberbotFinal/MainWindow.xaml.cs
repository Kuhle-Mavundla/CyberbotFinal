using System; // Imports the System namespace, which contains fundamental classes and base types.
using System.Collections.Generic; // Imports the namespace for generic collections like List.
using System.Globalization; // Imports the namespace for culture-specific information, used for text manipulation.
using System.IO; // Imports the namespace for input/output operations, like file handling.
using System.Linq; // Imports the namespace for Language Integrated Query (LINQ), providing methods for querying collections.
using System.Media; // Imports the namespace for playing system sounds and WAV files.
using System.Speech.Synthesis; // Imports the namespace for speech synthesis (text-to-speech).
using System.Text.RegularExpressions; // Imports the namespace for regular expressions, used for pattern matching in strings.
using System.Threading; // Imports the namespace for multithreading, though not directly used for new threads here, it's often related to async operations.
using System.Windows; // Imports the namespace for Windows Presentation Foundation (WPF) core elements, like Window.
using System.Windows.Threading; // Imports the namespace for Dispatcher objects, used for managing UI thread operations.
using CyberbotFinal.Pages; // Imports the namespace for custom page classes within the CyberbotFinal project.

namespace CyberbotFinal // Defines the namespace for the application.
{
    public partial class MainWindow : Window // Declares the MainWindow class, which is a partial class inheriting from Window (WPF window).
    {
        // Memory and state
        List<string> topicsDiscussed = new List<string>(); // Declares a list to store topics the user has discussed.
        string userName = ""; // Declares a string to store the user's name, initialized as empty.
        string userInterest = ""; // Declares a string to store the user's stated interest, initialized as empty.
        SpeechSynthesizer synthesizer = new SpeechSynthesizer(); // Creates an instance of SpeechSynthesizer for text-to-speech.

        List<string> activityLog = new List<string>(); // Declares a list to store a log of user and bot activities.
        List<(string Title, string Description, DateTime? Reminder)> tasks = new List<(string, string, DateTime?)>(); // Declares a list to store tasks, each with a title, description, and optional reminder date.

        private List<(string Question, string[] Options, int AnswerIndex, string Explanation)> quizQuestions; // Declares a list to store quiz questions, each with a question, options, correct answer index, and explanation.
        private int currentQuizIndex = 0; // Initializes an integer to track the current quiz question index.
        private int quizScore = 0; // Initializes an integer to track the user's quiz score.
        private bool quizInProgress = false; // Initializes a boolean to indicate if a quiz is currently in progress.

        // For NLP: track if chatbot is awaiting reminder input
        private bool awaitingReminderForTask = false; // Initializes a boolean to indicate if the bot is waiting for reminder input for a task.
        private string lastAddedTaskTitle = ""; // Declares a string to store the title of the last added task, used for setting reminders.

        public MainWindow() // Constructor for the MainWindow class.
        {
            InitializeComponent(); // Initializes the components defined in the XAML file for the window.
            AsciiHeader.Text = @"
   ______     __  __ _____     _____     ______      _____    _____    _________
  /\  ___\   /\ \/ / \ == \   /\  ___\  /\  == \     \ == \  /     \  /\__    __\
  \ \ \____  \ \  /   \  /___ \ \  ___\ \ \  __<      \   /__\      \ \/__\   \_/
   \ \_____\  \ \_\    \  == \ \ \_____\ \ \_\ \_\     \  == \\      \     \   \
    \/_____/   \/_/     \____/  \/_____/  \/_/ /_/      \____/ \_____/      \___\
"; // Sets the Text property of an AsciiHeader control (presumably a TextBlock) to a multi-line ASCII art string.

            PlayGreetingAudio("cyber_greeting.wav"); // Calls a method to play a greeting audio file.

            Dispatcher.InvokeAsync(() => // Executes the following code asynchronously on the UI thread.
            {
                AddToChat("CyberBot: Like I have mentioned, my name is CyberBot and I am your digital assistant for cybersecurity."); // Adds a message from CyberBot to the chat display.
                AddToChat("CyberBot: What is your name?"); // Adds another message asking for the user's name.
            });

            LoadQuizQuestions(); // Calls a method to load the quiz questions.
        }

        private void LoadQuizQuestions() // Defines a private method to load quiz questions.
        {
            quizQuestions = new List<(string, string[], int, string)> // Initializes the quizQuestions list with predefined questions.
            {
                ("What should you do if you receive an email asking for your password?", new[]{"Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it"}, 2, "Correct! Reporting phishing emails helps prevent scams."), // First quiz question.
                ("What makes a strong password?", new[]{"Your birthdate", "123456", "A mix of letters, numbers, and symbols", "Your pet's name"}, 2, "Correct! Strong passwords use a mix of characters."), // Second quiz question.
                ("What does 2FA stand for?", new[]{"Two-Factor Authentication", "Two-Firewall Access", "Two File Agreements", "Technical File Authorization"}, 0, "Correct! 2FA adds an extra layer of security."), // Third quiz question.
                ("What is malware?", new[]{"A strong password", "A software update", "A type of cyber threat", "A security program"}, 2, "Correct! Malware is harmful software."), // Fourth quiz question.
                ("Is it safe to click links in unknown emails?", new[]{"Yes", "Only if they look safe", "Never", "Sometimes"}, 2, "Correct! Avoid clicking unknown links."), // Fifth quiz question.
                ("What is a VPN used for?", new[]{"Speeding up internet", "Securing online traffic", "Downloading files", "Gaming"}, 1, "Correct! VPNs encrypt and secure online traffic."), // Sixth quiz question.
                ("How often should you update your passwords?", new[]{"Once a year", "Never", "Regularly", "Every 10 years"}, 2, "Correct! Update passwords regularly."), // Seventh quiz question.
                ("Which of these is a sign of a phishing scam?", new[]{"Spelling errors", "Urgency", "Unfamiliar links", "All of the above"}, 3, "Correct! All listed are signs of phishing."), // Eighth quiz question.
                ("What is ransomware?", new[]{"Free software", "Malware that locks data", "Anti-virus tool", "Spam email"}, 1, "Correct! Ransomware encrypts and locks your data."), // Ninth quiz question.
                ("Which of these is a good cybersecurity habit?", new[]{"Sharing passwords", "Using public Wi-Fi for banking", "Clicking pop-up ads", "Using antivirus software"}, 3, "Correct! Antivirus helps protect your system."), // Tenth quiz question.
                ("Should you use the same password for multiple accounts?", new[]{"Yes", "No"}, 1, "Correct! Always use unique passwords for each account."), // Eleventh quiz question.
                ("What does a firewall do?", new[]{"Cooks food", "Blocks unauthorized access", "Increases speed", "Stores files"}, 1, "Correct! A firewall blocks unauthorized access."), // Twelfth quiz question.
                ("Is it okay to ignore software updates?", new[]{"Yes", "Only on weekends", "No"}, 2, "Correct! Updates fix security vulnerabilities."), // Thirteenth quiz question.
                ("What’s a common goal of social engineering?", new[]{"Entertainment", "Stealing info", "Improving security", "Fixing bugs"}, 1, "Correct! Social engineering tricks people into giving info.") // Fourteenth quiz question.
            };
        }

        private void GoToLog_Click(object sender, RoutedEventArgs e) // Event handler for a click on the "GoToLog" button.
        {
            MainFrame.Navigate(new ActivityLogPage()); // Navigates the MainFrame (presumably a Frame control) to a new instance of ActivityLogPage.
        }

        private void GoToTask_Click(object sender, RoutedEventArgs e) // Event handler for a click on the "GoToTask" button.
        {
            MainFrame.Navigate(new TaskAssistantPage()); // Navigates the MainFrame to a new instance of TaskAssistantPage.
            LogAction("User sent a message."); // Logs the action "User sent a message." (This log message seems misplaced here, as it's tied to navigation, not message sending).
        }

        private void GoToTips_Click(object sender, RoutedEventArgs e) // Event handler for a click on the "GoToTips" button.
        {
            MainFrame.Navigate(new CyberTipsPage()); // Navigates the MainFrame to a new instance of CyberTipsPage.
            LogAction("User navigated to Cyber Tips page."); // Logs the action "User navigated to Cyber Tips page."
        }

        private void GoToQuiz_Click(object sender, RoutedEventArgs e) // Event handler for a click on the "GoToQuiz" button.
        {
            MainFrame.Navigate(new QuizPage()); // Navigates the MainFrame to a new instance of QuizPage.
            LogAction("User navigated to Quiz page."); // Logs the action "User navigated to Quiz page."
        }

        private void GoToMainChat_Click(object sender, RoutedEventArgs e) // Event handler for a click on the "GoToMainChat" button.
        {
            MainFrame.Content = null; // Clears the content of the MainFrame, effectively returning to the main chat view.
            LogAction("User navigated back to Main Chat."); // Logs the action "User navigated back to Main Chat."
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) // Event handler for a click on the "SendButton".
        {
            string userInput = UserInput.Text.Trim(); // Gets the text from the UserInput control (presumably a TextBox) and removes leading/trailing whitespace.
            if (string.IsNullOrEmpty(userInput)) return; // If the user input is empty or null, exit the method.

            AddToChat($"You: {userInput}"); // Adds the user's input to the chat display, prefixed with "You: ".
            UserInput.Clear(); // Clears the text from the UserInput control.
            LogAction("User sent message"); // Logs the action "User sent message".

            if (awaitingReminderForTask) // Checks if the bot is currently awaiting a reminder input for a task.
            {
                ProcessReminderResponse(userInput); // Calls a method to process the user's reminder response.
                return; // Exit the method after processing the reminder response.
            }

            if (quizInProgress) // Checks if a quiz is currently in progress.
            {
                ProcessQuizAnswer(userInput); // Calls a method to process the user's quiz answer.
                return; // Exit the method after processing the quiz answer.
            }

            if (string.IsNullOrEmpty(userName)) // Checks if the user's name has not yet been set.
            {
                userName = userInput; // Sets the userName to the current user input.
                LogAction("User wrote their name"); // Logs the action "User wrote their name".
                AddToChat($"\nWelcome, {userName}!"); // Adds a welcome message to the chat display, including the user's name.
                AddToChat("You can ask me about topics like:\n- 'phishing'\n- 'strong passwords'\n- 'malware'\n- 'ransomware'\n- 'firewall'"); // Informs the user about available topics.
                AddToChat("Or say things like 'I'm worried about scams' or 'I'm interested in privacy'."); // Provides examples of other types of queries.
                LoadQuizQuestions(); // Reloads quiz questions (redundant if already loaded in constructor, but ensures they are available).

                // Inform the user about added features
                AddToChat("Here’s what you can do:"); // Informs the user about additional features.
                AddToChat("👉 Type 'start quiz' to begin the cybersecurity quiz"); // Explains how to start the quiz.
                AddToChat("👉 Type 'add task to...' to track a security task"); // Explains how to add a task.
                AddToChat("👉 Type 'remind me in X days' to set a reminder"); // Explains how to set a reminder.
                AddToChat("👉 Type 'show activity log' to see what I’ve done"); // Explains how to view the activity log.
                AddToChat("👉 Type 'help' to get a list of supported topics"); // Explains how to get help.

                AddToChat("Type 'exit' anytime to quit."); // Informs the user how to exit.
                return; // Exit the method after initial setup.
            }

            userInput = userInput.ToLower(); // Converts the user's input to lowercase for case-insensitive matching.

            if (userInput == "exit") // Checks if the user input is "exit".
            {
                HandleExit(); // Calls a method to handle the exit process.
                LogAction("User exited"); // Logs the action "User exited".
                return; // Exit the method.
            }

            // NLP keyword recognition for tasks and reminders
            if (userInput.Contains("add task") || userInput.StartsWith("add a task to") || userInput.StartsWith("add task to")) // Checks if the user input contains phrases related to adding a task.
            {
                string taskTitle = ExtractTaskTitle(userInput); // Extracts the task title from the user input.
                if (!string.IsNullOrEmpty(taskTitle)) // Checks if a task title was successfully extracted.
                {
                    AddTask(taskTitle, $"Cybersecurity task: {taskTitle}", null); // Adds the task with the extracted title and a generic description, no initial reminder.
                    AddToChat($"Task added: '{taskTitle}'. Would you like to set a reminder for this task?"); // Confirms task addition and asks about setting a reminder.
                    awaitingReminderForTask = true; // Sets the flag to true, indicating the bot is awaiting reminder input.
                    lastAddedTaskTitle = taskTitle; // Stores the title of the last added task.
                    return; // Exit the method.
                }
            }

            if (userInput.StartsWith("remind me") || userInput.StartsWith("set a reminder") || userInput.Contains("reminder")) // Checks if the user input contains phrases related to setting a reminder.
            {
                // Basic parse reminder command if not awaiting a reminder
                ParseAndSetReminder(userInput); // Calls a method to parse and set a reminder.
                return; // Exit the method.
            }

            if (userInput == "start quiz" || userInput == "quiz") // Checks if the user input is "start quiz" or "quiz".
            {
                StartQuiz(); // Calls a method to start the quiz.
                return; // Exit the method.
            }

            if (userInput == "show activity log" || userInput == "what have you done for me") // Checks if the user input is "show activity log" or "what have you done for me".
            {
                ShowActivityLog(); // Calls a method to display the activity log.
                return; // Exit the method.
            }

            if (userInput.Contains("worried") || userInput.Contains("scared") || userInput.Contains("anxious")) // Checks if the user expresses worry, fear, or anxiety.
            {
                string supportMsg = "It's completely understandable to feel that way. Scammers can be very convincing. Let me share some tips to help you stay safe."; // Defines a supportive message.
                Respond(supportMsg); // Responds with the supportive message.
                ShareCyberTips(); // Calls a method to share general cybersecurity tips.
                return; // Exit the method.
            }

            if (userInput.StartsWith("i'm interested in")) // Checks if the user expresses interest in a topic.
            {
                userInterest = userInput.Replace("i'm interested in", "").Trim(); // Extracts the user's interest by removing the introductory phrase.
                string msg = $"Great! I'll remember that you're interested in {userInterest}. It's a crucial part of staying safe online."; // Forms a confirmation message.
                Respond(msg); // Responds with the confirmation message.
                return; // Exit the method.
            }

            if (userInput.Contains("tell me more") || userInput.Contains("more about")) // Checks if the user wants more information about something.
            {
                string[] words = userInput.Split(' '); // Splits the user input into individual words.
                string keyword = words.Last(); // Gets the last word as the keyword for the query.
                HandleUserQuery(keyword); // Calls a method to handle the user's query based on the keyword.
                return; // Exit the method.
            }

            HandleUserQuery(userInput); // If no specific command is matched, calls a general method to handle the user's query.
        }

        private void ProcessReminderResponse(string userInput) // Defines a private method to process the user's response to a reminder prompt.
        {
            // Expect something like "yes, remind me in 3 days" or "no"
            if (userInput.ToLower().Contains("yes")) // Checks if the user's response contains "yes" (case-insensitive).
            {
                // Extract days or date from input
                var match = Regex.Match(userInput.ToLower(), @"in (\d+) days?"); // Uses a regular expression to find a pattern like "in X days".
                if (match.Success) // Checks if the pattern was found.
                {
                    int days = int.Parse(match.Groups[1].Value); // Extracts the number of days from the matched group.
                    DateTime reminderDate = DateTime.Now.AddDays(days); // Calculates the reminder date by adding the specified number of days to the current date.
                    SetReminderForLastTask(reminderDate); // Calls a method to set the reminder for the last added task.
                    AddToChat($"Got it! I'll remind you in {days} days."); // Confirms the reminder has been set.
                    LogAction($"Reminder set for task '{lastAddedTaskTitle}' in {days} days."); // Logs the reminder setting action.
                }
                else
                {
                    AddToChat("Please specify the number of days for the reminder, e.g., 'Remind me in 3 days'."); // Prompts the user for a valid reminder format.
                    return; // still awaiting correct input // Exits the method, keeping awaitingReminderForTask true.
                }
            }
            else if (userInput.ToLower().Contains("no")) // Checks if the user's response contains "no" (case-insensitive).
            {
                AddToChat("Okay, no reminder set."); // Informs the user that no reminder will be set.
                LogAction($"No reminder set for task '{lastAddedTaskTitle}'."); // Logs that no reminder was set.
            }
            else
            {
                AddToChat("Please answer 'yes' or 'no'. Would you like to set a reminder?"); // Prompts the user to answer "yes" or "no".
                return; // still awaiting valid input // Exits the method, keeping awaitingReminderForTask true.
            }
            awaitingReminderForTask = false; // Resets the flag, as the reminder process is complete (either set or declined).
            lastAddedTaskTitle = ""; // Clears the last added task title.
        }

        private void SetReminderForLastTask(DateTime reminderDate) // Defines a private method to set a reminder for the last added task.
        {
            for (int i = 0; i < tasks.Count; i++) // Loops through all existing tasks.
            {
                if (tasks[i].Title == lastAddedTaskTitle) // Finds the task that matches the last added task title.
                {
                    tasks[i] = (tasks[i].Title, tasks[i].Description, reminderDate); // Updates the reminder date for that specific task.
                    break; // Exits the loop once the task is found and updated.
                }
            }
        }

        private void AddTask(string title, string description, DateTime? reminder) // Defines a private method to add a new task.
        {
            tasks.Add((title, description, reminder)); // Adds a new tuple (task) to the tasks list.
            LogAction($"Task added: '{title}'" + (reminder.HasValue ? $" (Reminder set for {reminder.Value.ToShortDateString()})" : " (no reminder set)")); // Logs the task addition, including reminder info if present.
        }

        private void ParseAndSetReminder(string userInput) // Defines a private method to parse user input and set a reminder.
        {
            // Basic pattern: "remind me in 3 days" or "remind me on 2025-07-01"
            var daysMatch = Regex.Match(userInput.ToLower(), @"remind me in (\d+) days?"); // Tries to match "remind me in X days".
            var dateMatch = Regex.Match(userInput.ToLower(), @"remind me on (\d{4}-\d{2}-\d{2})"); // Tries to match "remind me on YYYY-MM-DD".

            if (daysMatch.Success) // If "remind me in X days" pattern is found.
            {
                int days = int.Parse(daysMatch.Groups[1].Value); // Extracts the number of days.
                DateTime reminderDate = DateTime.Now.AddDays(days); // Calculates the reminder date.
                AddToChat($"Reminder set for {reminderDate.ToShortDateString()}."); // Confirms the reminder setting.
                LogAction($"Reminder set for {reminderDate.ToShortDateString()} via NLP command."); // Logs the action.
            }
            else if (dateMatch.Success) // If "remind me on YYYY-MM-DD" pattern is found.
            {
                if (DateTime.TryParse(dateMatch.Groups[1].Value, out DateTime reminderDate)) // Tries to parse the extracted date string into a DateTime object.
                {
                    AddToChat($"Reminder set for {reminderDate.ToShortDateString()}."); // Confirms the reminder setting.
                    LogAction($"Reminder set for {reminderDate.ToShortDateString()} via NLP command."); // Logs the action.
                }
                else
                {
                    AddToChat("Invalid date format for reminder."); // Informs the user about an invalid date format.
                }
            }
            else // If neither pattern is matched.
            {
                AddToChat("Sorry, I couldn't understand the reminder date. Please specify like 'Remind me in 3 days' or 'Remind me on 2025-07-01'."); // Prompts the user for correct format.
            }
        }

        private string ExtractTaskTitle(string input) // Defines a private method to extract a task title from user input.
        {
            // Simple heuristic to extract task title from user input
            // Remove phrases like "add task" and trim
            string title = input.ToLower(); // Converts the input to lowercase.
            title = Regex.Replace(title, @"add (a )?task( to)?", "").Trim(); // Removes "add task", "add a task", "add task to", "add a task to" and trims whitespace.
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title); // Converts the first letter of each word in the title to uppercase (title case).
        }

        private void StartQuiz() // Defines a private method to start the quiz.
        {
            quizInProgress = true; // Sets the flag to true, indicating a quiz is in progress.
            currentQuizIndex = 0; // Resets the current quiz question index to the beginning.
            quizScore = 0; // Resets the quiz score to zero.
            AskQuizQuestion(); // Calls a method to ask the first quiz question.
        }

        private void AskQuizQuestion() // Defines a private method to ask a quiz question.
        {
            if (currentQuizIndex >= quizQuestions.Count) // Checks if all quiz questions have been asked.
            {
                quizInProgress = false; // Sets the flag to false, indicating the quiz is over.
                AddToChat($"Quiz complete! Your score: {quizScore} out of {quizQuestions.Count}"); // Displays the final quiz score.
                LogAction($"Quiz ended with score {quizScore}/{quizQuestions.Count}"); // Logs the quiz completion and score.
                return; // Exit the method.
            }

            var q = quizQuestions[currentQuizIndex]; // Gets the current quiz question from the list.
            string questionText = $"Question {currentQuizIndex + 1}: {q.Question}\nOptions:"; // Formats the question text.
            for (int i = 0; i < q.Options.Length; i++) // Loops through the options for the current question.
            {
                questionText += $"\n  {i + 1}. {q.Options[i]}"; // Adds each option to the question text with a number.
            }

            AddToChat(questionText); // Displays the formatted question and options in the chat.
            AddToChat("Please type the option number for your answer."); // Prompts the user to enter their answer.
        }

        private void ProcessQuizAnswer(string userInput) // Defines a private method to process the user's quiz answer.
        {
            if (!int.TryParse(userInput, out int answerNum)) // Tries to parse the user's input as an integer.
            {
                AddToChat("Please enter a valid option number."); // If parsing fails, prompts for a valid number.
                return; // Exit the method.
            }

            var q = quizQuestions[currentQuizIndex]; // Gets the current quiz question.
            if (answerNum - 1 == q.AnswerIndex) // Checks if the user's answer (adjusted for 0-based index) matches the correct answer index.
            {
                quizScore++; // Increments the quiz score if the answer is correct.
                AddToChat(q.Explanation); // Displays the explanation for the correct answer.
            }
            else
            {
                AddToChat($"Incorrect. {q.Explanation}"); // Informs the user their answer was incorrect and provides the explanation.
            }
            currentQuizIndex++; // Moves to the next quiz question.
            AskQuizQuestion(); // Calls the method to ask the next question.
        }

        void AddToChat(string message) // Defines a method to add a message to the chat display.
        {
            ChatBox.Items.Add(message); // Adds the given message as a new item to the ChatBox control (presumably a ListBox or similar).
        }

        void Respond(string message) // Defines a method for the CyberBot to respond to the user.
        {
            AddToChat("CyberBot: " + message); // Adds the CyberBot's message to the chat display, prefixed with "CyberBot: ".
            synthesizer.SpeakAsync(message); // Uses the SpeechSynthesizer to speak the message asynchronously.
        }

        void PlayGreetingAudio(string filePath) // Defines a method to play an audio file.
        {
            try // Starts a try block to handle potential exceptions.
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", filePath); // Constructs the full path to the audio file within the application's Resources folder.
                if (File.Exists(fullPath)) // Checks if the audio file exists at the specified path.
                {
                    SoundPlayer player = new SoundPlayer(fullPath); // Creates a new SoundPlayer instance with the audio file path.
                    player.PlaySync(); // Plays the audio file synchronously (waits for it to finish).
                }
                else
                {
                    AddToChat($"[Error] File not found: {fullPath}"); // If the file doesn't exist, adds an error message to the chat.
                }
            }
            catch (Exception ex) // Catches any exception that occurs within the try block.
            {
                AddToChat($"[Error] Playing audio: {ex.Message}"); // Adds an error message to the chat, including the exception message.
            }
        }

        void HandleUserQuery(string input) // Defines a method to handle general user queries.
        {
            Dictionary<string, List<string>> responses = new Dictionary<string, List<string>> // Creates a dictionary to store predefined responses for various keywords.
            {
                {"help", new List<string> { "You can ask about: phishing, strong passwords, malware, ransomware, or firewall." }}, // Responses for "help".
                {"phishing", new List<string> {
                    "Be cautious of emails asking for personal information.",
                    "Always verify the sender's email before clicking links.",
                    "Look for spelling errors in scam emails."
                }}, // Responses for "phishing".
                {"strong passwords", new List<string> {
                    "Use a mix of characters. Avoid personal details.",
                    "Consider using a password manager.",
                    "Change passwords often and avoid reusing them."
                }}, // Responses for "strong passwords".
                {"malware", new List<string> {
                    "Malware is harmful software. Avoid unknown sources.",
                    "Keep antivirus software updated.",
                    "Be cautious with email attachments and downloads."
                }}, // Responses for "malware".
                {"ransomware", new List<string> {
                    "Ransomware locks your data. Keep backups!",
                    "Don't pay the ransom—it’s not guaranteed.",
                    "Update software and avoid suspicious files."
                }}, // Responses for "ransomware".
                {"firewall", new List<string> {
                    "A firewall protects you from unauthorized access.",
                    "Keep it enabled to block malicious traffic.",
                    "Use both hardware and software firewalls."
                }}, // Responses for "firewall".
                {"how are you", new List<string> { "I'm always functioning at peak performance—no emotions, just security!" }}, // Responses for "how are you".
                {"what's your purpose", new List<string> { "I'm here to help you stay safe online." }}, // Responses for "what's your purpose".
                {"what can i ask you about", new List<string> { "Topics like phishing, malware, passwords, firewalls, and tips!" }} // Responses for "what can I ask you about".
            };

            if (responses.ContainsKey(input)) // Checks if the exact user input is a key in the responses dictionary.
            {
                string response = GetRandomResponse(responses[input]); // Gets a random response from the list associated with the input.
                Respond(response); // Responds with the chosen message.

                if (!topicsDiscussed.Contains(input) && input != "help") // If the topic hasn't been discussed before and isn't "help".
                    topicsDiscussed.Add(input); // Adds the topic to the list of discussed topics.

                if (!string.IsNullOrEmpty(userInterest)) // Checks if the user has previously stated an interest.
                {
                    string tip = $"Since you're interested in {userInterest}, you might want to check related security practices."; // Forms a tip related to the user's interest.
                    Respond(tip); // Responds with the interest-based tip.
                }
            }
            else // If the exact user input is not found in the responses.
            {
                string closestMatch = FindClosestMatch(input, responses.Keys.ToList()); // Finds the closest matching keyword using Levenshtein distance.
                if (closestMatch != null) // If a closest match is found.
                {
                    string response = GetRandomResponse(responses[closestMatch]); // Gets a random response for the closest match.
                    Respond($"Did you mean '{closestMatch}'? {response}"); // Asks if the user meant the closest match and provides the response.
                }
                else // If no close match is found.
                {
                    AddToChat($"CyberBot: Hmm, I’m not sure about that, {userName}. Want some general cybersecurity tips? (yes/no)"); // Informs the user that the query is not understood and offers general tips.
                }
            }
        }

        string GetRandomResponse(List<string> responses) // Defines a method to get a random response from a list of strings.
        {
            Random random = new Random(); // Creates a new Random object.
            return responses[random.Next(responses.Count)]; // Returns a random string from the provided list.
        }

        string FindClosestMatch(string input, List<string> options) // Defines a method to find the closest string match in a list.
        {
            return options.OrderBy(option => LevenshteinDistance(input, option)).FirstOrDefault(); // Orders the options by their Levenshtein distance to the input and returns the first (closest) one.
        }

        int LevenshteinDistance(string s, string t) // Defines a method to calculate the Levenshtein distance between two strings.
        {
            int n = s.Length, m = t.Length; // Gets the lengths of the two strings.
            var d = new int[n + 1, m + 1]; // Creates a 2D array to store the distances.

            for (int i = 0; i <= n; i++) d[i, 0] = i; // Initializes the first column of the distance matrix.
            for (int j = 0; j <= m; j++) d[0, j] = j; // Initializes the first row of the distance matrix.

            for (int i = 1; i <= n; i++) // Iterates through the first string.
            {
                for (int j = 1; j <= m; j++) // Iterates through the second string.
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1; // Calculates the cost of substitution (0 if characters are same, 1 if different).
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost); // Calculates the minimum of deletion, insertion, and substitution costs.
                }
            }
            return d[n, m]; // Returns the final Levenshtein distance.
        }

        void ShareCyberTips() // Defines a method to share general cybersecurity tips.
        {
            Respond("Here are some general cybersecurity tips:"); // Introduces the tips.
            Respond("1️ Use strong, unique passwords."); // First tip.
            Respond("2️ Enable two-factor authentication."); // Second tip.
            Respond("3️ Be cautious of unexpected emails or links."); // Third tip.
            Respond("4️ Keep your software and antivirus updated."); // Fourth tip.
            Respond("5️ Back up your data regularly."); // Fifth tip.
        }

        void HandleExit() // Defines a method to handle the application exit process.
        {
            if (topicsDiscussed.Count == 0) // Checks if no cybersecurity topics were discussed.
            {
                Respond("You didn’t even ask me a single cybersecurity question. Keep in mind that online safety starts with awareness."); // Provides a message if no topics were discussed.
            }
            else // If topics were discussed.
            {
                Respond("You showed interest in:"); // Introduces the list of discussed topics.
                foreach (var topic in topicsDiscussed) // Iterates through the discussed topics.
                {
                    AddToChat($" - {topic}"); // Adds each discussed topic to the chat.
                }

                if (!string.IsNullOrEmpty(userInterest)) // Checks if the user had a stated interest.
                {
                    Respond($"And I remember you're interested in: {userInterest}"); // Reminds the user of their stated interest.
                }

                Respond("To learn more, check out these resources:"); // Suggests external resources.
                Respond("https://staysafeonline.org"); // First resource link.
                Respond("https://www.cyber.gov.au"); // Second resource link.
                Respond("https://www.cisa.gov"); // Third resource link.
            }

            Respond("Goodbye! Stay safe and stay alert online."); // Final farewell message.
        }

        void LogAction(string action) // Defines a method to log an action.
        {
            string logEntry = $"{DateTime.Now}: {action}"; // Creates a log entry with the current timestamp and the action description.
            activityLog.Add(logEntry); // Adds the log entry to the activity log list.
        }

        private void ShowActivityLog() // Defines a method to display the activity log.
        {
            if (activityLog.Count == 0) // Checks if the activity log is empty.
            {
                AddToChat("Activity log is empty."); // Informs the user that the log is empty.
                return; // Exit the method.
            }

            AddToChat("Activity Log:"); // Introduces the activity log.
            foreach (string entry in activityLog) // Iterates through each entry in the activity log.
            {
                AddToChat(entry); // Adds each log entry to the chat display.
            }
        }
    }
}
