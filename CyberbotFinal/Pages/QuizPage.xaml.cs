using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CyberbotFinal.Pages
{
    public partial class QuizPage : Page
    {
        private List<(string question, List<string> options, int correctIndex)> quiz;
        private int currentQuestion = 0;

        public QuizPage()
        {
            InitializeComponent();
            LoadQuiz();
            ShowQuestion();
        }

        private void LoadQuiz()
        {
            quiz = new List<(string, List<string>, int)>
            {
                ("What is phishing?", new List<string> {
                    "A hacking tool", "A fake website scam", "A type of firewall", "Antivirus software"
                }, 1),

                ("What's the safest password?", new List<string> {
                    "123456", "password", "MyBirthday", "Z!6k@1vL"
                }, 3),

                ("What does 2FA stand for?", new List<string> {
                    "Two-Factor Authentication", "Too Fast Access", "Two File Authorization", "Trusted Firewall Agent"
                }, 0),
            };
        }

        private void ShowQuestion()
        {
            if (currentQuestion >= quiz.Count)
            {
                QuestionText.Text = "Quiz complete! Great job!";
                OptionsList.Items.Clear();
                return;
            }

            var (q, options, _) = quiz[currentQuestion];
            QuestionText.Text = q;
            OptionsList.Items.Clear();
            foreach (var option in options)
            {
                OptionsList.Items.Add(option);
            }

            FeedbackText.Text = "";
        }

        private void OptionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OptionsList.SelectedIndex == -1) return;

            var correct = quiz[currentQuestion].correctIndex;
            if (OptionsList.SelectedIndex == correct)
                FeedbackText.Text = "✅ Correct!";
            else
                FeedbackText.Text = "❌ Incorrect!";
        }

        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            currentQuestion++;
            ShowQuestion();
        }
    }
}
