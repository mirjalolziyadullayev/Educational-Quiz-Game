using Educational_Quiz_Game.Services;
using Spectre.Console;
namespace Educational_Quiz_Game.Menu;
public class MainMenu
{
    private readonly UserService userService;
    private readonly WordService wordService;

    private readonly ProgressTrackerMenu progressTracker;
    private readonly QuizMenu quizMenu;
    private readonly UserMenu userMenu;
    private readonly WordMenu wordMenu;

    public MainMenu()
    {
        userService = new UserService();
        wordService = new WordService();

        progressTracker = new ProgressTrackerMenu(userService);
        quizMenu = new QuizMenu(userService, wordService);
        userMenu = new UserMenu(userService);
        wordMenu = new WordMenu(wordService, userService);
    }

    public async void Show()
    {
        while (true)
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Main selection menu")
                    .AddChoices("Manage Users", "Manage Words", "Progress Tracker", "Quiz Game", "Exit"));

            AnsiConsole.Clear();

            switch (selection)
            {
                case "Progress Tracker":
                    AnsiConsole.Clear();
                    progressTracker.TrackProgress();
                    break;
                case "Quiz":
                    AnsiConsole.Clear();
                    quizMenu.StartQuizMenu();
                    break;
                case "User":
                    AnsiConsole.Clear();
                    await userMenu.Show();
                    break;
                case "Word":
                    AnsiConsole.Clear();
                    await wordMenu.Show();
                    break;
                case "Exit":
                    AnsiConsole.Clear();
                    Console.WriteLine("Exit main menu");
                    return;
                default:
                    AnsiConsole.Clear();
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}
