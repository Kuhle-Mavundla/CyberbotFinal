CyberBot Guide

This ReadMe will guide you on how to use this chatbot easily I doubt instructions are needed to show how the buttons work for similar functions
since if it has buttons it is easy to use and gives a great user experience compared to these required commands anyway this is how it works: 

1.Getting Started

The ‘MainWindow’ is the first thing that gets booted up and it does the following when the CyberBot application starts:

 GreetingPlays an audio greeting and prints an ASCII art header.

Name Prompt:Demands your name right away.

To begin interaction:

Put your name in the input box below and hit Enter or click on "Send."

Core Chatbot Interaction

Once you have given a name, the chatbot will reply with a greeting and a selection of initial features.

2.1. Asking About Cybersecurity Topics

You may inquire to CyberBot for certain topics The chatbot has predefined responses for common terms.
Supported Topics:
       `phishing`
      `strong passwords`
       `malware`
       `ransomware`
       `firewall`
How to Ask: Simply type the topic name (e.g., `phishing`) into the input box and send.
Fuzzy Matching: If you misspell a topic, CyberBot might suggest a correction (e.g., typing `phising` might prompt "Did you mean 'phishing'?").

2.2. Support & General Inquiries
 Help: To obtain a list of supported topics, type `help`.
 Emotional Support: CyberBot will provide comfort and basic cybersecurity advice if you voice concerns (e.g., `I'm worried about scams`, `I'm scared`).

 Expressing Interest: You can use phrases like "I'm interested in privacy" to let CyberBot know what interests you.  The bot will keep this in mind and may modify its recommendations in the future.
 More facts: You can frequently write `tell me more` or `more about [topic]` to obtain more facts if CyberBot just gives a brief response.
 Try asking "how are you" or "what's your purpose" in a casual conversation.

 3. Interactive Elements

 A number of interactive elements that can be accessed with particular commands are integrated into the `MainWindow` class.
 3.1. Test of Cybersecurity

Take a brief cybersecurity quiz to see how much you know.
 Launch the quiz:  Enter `quiz` or `start quiz`.
 Answering Questions: Enter the number that corresponds to the choice you selected for each question (e.g., `1`, `2`, `3`).

 Quiz Flow: Following each response, CyberBot will instantly provide feedback (right or incorrect) and an explanation before moving on to the next question.  After you have responded to every question and seen your score, the quiz is over.

 3.2. Reminders & Task Management
 Tasks linked to cybersecurity can be added, and reminders can be made.

Enter `add task to [your task description]` to add a task (for example, `add task to update my email password`).
       After verifying the task addition, CyberBot will you whether you would like to create a reminder.

       After adding a task, set a reminder by saying, "Yes, remind me in X days" (for example, "yes, remind me in 7 days") when asked.
       An alternative is to use a specific date to establish a reminder directly: `yes, remind me on YYYY-MM-DD` (for example, `yes, remind me on 2024-12-31`).
       Use `no` to indicate that you do not want a reminder.
       Creating a Standalone Reminder:
       You can also type `remind me in X days` or `remind me on YYYY-MM-DD` to set a reminder without first adding a task.
 3.3. Log of Activities

Examine your previous exchanges with CyberBot.
 Examine the log:  Enter `what have you done for me` or `show activity log`.
 A timestamped list of messages and actions will be shown in the log.


 4. Closing the Program

 Exit Command: Enter `exit` to finish your CyberBot session politely.
 Exit Summary: Prior to closing, CyberBot will give you a rundown of the subjects covered and any interests you mentioned, as well as links to other resources for additional education.

 5. UI Buttons for Navigation

 Additionally, the `MainWindow` has navigation buttons that take users to specific sites for specific features:
GoToLog: To observe previous interactions, navigate to the `ActivityLogPage`.
 GoToTask: Opens the `TaskAssistantPage`, which has a specialized user interface for handling tasks and reminders.
 GoToTips: Views general cybersecurity advice by navigating to the `CyberTipsPage`.
 Demonstration Youtube video:https://www.youtube.com/watch?v=dBq151yt_rA

 GoToQuiz: Opens the cybersecurity test through a specialized user interface by navigating to the `QuizPage`.
 GoToMainChat: From any browsed page, this function takes you back to the main chat interface.
By following these instructions, you can easily use this chatbot named CyberBot
