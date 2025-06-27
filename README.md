CyberBot Guide

This ReadMe will guide you on how to use this chatbot easily I doubt instructions are needed to show how the buttons work for similar functions
since if it has buttons it is easy to use and gives a great user experience compared to these required commands anyway this is how it works: 

1. Getting Started

Upon launching the CyberBot application, the `MainWindow` initializes and performs the following:
Greeting:Plays a greeting audio and displays an ASCII art header.
Name Prompt:Immediately asks for your name.
To begin interaction:
Type your name into the input box and press Enter or click the "Send" button.

2. Core Chatbot Interaction**

After providing your name, the chatbot will welcome you and provide a list of initial capabilities.
2.1. Asking About Cybersecurity Topics
You can ask CyberBot about various cybersecurity topics. The chatbot has predefined responses for common terms.
Supported Topics:**
       `phishing`
      `strong passwords`
       `malware`
       `ransomware`
       `firewall`
How to Ask: Simply type the topic name (e.g., `phishing`) into the input box and send.
Fuzzy Matching: If you misspell a topic, CyberBot might suggest a correction (e.g., typing `phising` might prompt "Did you mean 'phishing'?").

2.2. General Queries & Support
Help: Type `help` to get a list of supported topics.
Emotional Support: If you express concern (e.g., `I'm worried about scams`, `I'm scared`), CyberBot will offer reassurance and share general cybersecurity tips.
Expressing Interest: You can tell CyberBot your interests (e.g., `I'm interested in privacy`). The bot will remember this and might tailor future tips.
More Information: If CyberBot provides a short answer, you can often type `tell me more` or `more about [topic]` to get additional details.
Casual Conversation: Try `how are you` or `what's your purpose`.

3. Interactive Features

The `MainWindow` class integrates several interactive features accessible via specific commands.
3.1. Cybersecurity Quiz

Test your knowledge with a quick cybersecurity quiz.
Start Quiz: Type `start quiz` or `quiz`.
Answering Questions:** For each question, type the number corresponding to your chosen option (e.g., `1`, `2`, `3`).
Quiz Flow: CyberBot will provide immediate feedback (correct/incorrect) and an explanation after each answer, then move to the next question. The quiz ends when all questions are answered, and your score is displayed.

3.2. Task Management & Reminders
You can add cybersecurity-related tasks and set reminders for them.

    Adding a Task:
      Type `add task to [your task description]` (e.g., `add task to change my email password`).
      CyberBot will confirm the task addition and ask if you want to set a reminder.
      Setting a Reminder (after adding a task):
      If prompted, respond with `yes, remind me in X days` (e.g., `yes, remind me in 7 days`).
      Alternatively, you can set a reminder directly using a specific date: `yes, remind me on YYYY-MM-DD` (e.g., `yes, remind me on 2024-12-31`).
      If you don't want a reminder, respond with `no`.
      Setting a Reminder (standalone):
      You can also set a reminder without first adding a task by typing `remind me in X days` or `remind me on YYYY-MM-DD`.
3.3. Activity Log

Review your past interactions with CyberBot.
View Log: Type `show activity log` or `what have you done for me`.
The log will display a timestamped list of actions and messages.

4. Exiting the Application

Exit Command: Type `exit` to gracefully end your session with CyberBot.
Exit Summary: Before closing, CyberBot will provide a summary of topics discussed and any interests you've expressed, along with external resources for further learning.

5. Navigation (UI Buttons)

The `MainWindow` also features navigation buttons that lead to dedicated pages for certain functionalities:

GoToLog: Navigates to the `ActivityLogPage` to view past interactions.
GoToTask: Navigates to the `TaskAssistantPage` for managing tasks and reminders through a dedicated UI.
GoToTips: Navigates to the `CyberTipsPage` to view general cybersecurity tips.
GoToQuiz: Navigates to the `QuizPage` to start the cybersecurity quiz via a dedicated UI.
GoToMainChat: Returns to the main chat interface from any navigated page.

By following these instructions, you can easily use this chatbot named CyberBot
Demonstration video:https://youtu.be/dBq151yt_rA
