using Educational_Quiz_Game.Interfaces;
using Educational_Quiz_Game.Models;
using Spectre.Console;
namespace Educational_Quiz_Game.Menu;
public class UserMenu
{
    private readonly IUserService userService;

    public UserMenu(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task Show()
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(10)
                    .Title("Choose an option:")
                    .AddChoices("Create User", "Update User", "Delete User", "View User", "View All Users", "Exit"));

            switch (choice)
            {
                case "Create User":
                    AnsiConsole.Clear();
                    await Create();
                    break;
                case "Update User":
                    AnsiConsole.Clear();
                    await Update();
                    break;
                case "Delete User":
                    AnsiConsole.Clear();
                    await Delete();
                    break;
                case "View User":
                    AnsiConsole.Clear();
                    await View();
                    break;
                case "View All Users":
                    AnsiConsole.Clear();
                    await ViewAll();
                    break;
                case "Exit":
                    AnsiConsole.Clear();
                    Console.WriteLine("Exit...");
                    return;
                default:
                    AnsiConsole.Clear();
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private async Task Create()
    {
        AnsiConsole.MarkupLine("[bold green]CREATE USER[/]");
        var name = AnsiConsole.Ask<string>("Enter user name: ");

        var newUser = new User
        {
            Name = name,
            LearnedWords = new List<Word>(),
            Scores = 0
        };
        try
        {
            var createdUser = await userService.CreateAsync(newUser);
            AnsiConsole.MarkupLine("[green]Successfully added...[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
    }

    private async Task Update()
    {
        AnsiConsole.MarkupLine("[bold green]UPDATE USER[/]");
        var userId = AnsiConsole.Ask<int>("Enter user ID to update: ");

        var newName = AnsiConsole.Ask<string>("Enter new user name: ");
        var user = new User()
        {
            Name = newName,
        };
        try
        {
            var updatedUser = await userService.UpdateAsync(userId, user);
            AnsiConsole.MarkupLine("[green]Successfully updated...[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
    }

    private async Task Delete()
    {
        AnsiConsole.MarkupLine("[bold green]DELETE USER[/]");
        var userId = AnsiConsole.Ask<int>("Enter user ID to delete: ");
        try
        {
            var deleted = await userService.DeleteAsync(userId);
            AnsiConsole.MarkupLine("[green]Successfully deleted...[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
    }

    private async Task View()
    {
        AnsiConsole.MarkupLine("[bold green]VIEW USER[/]");
        var userId = AnsiConsole.Ask<int>("Enter user ID to view: ");

        var user = await userService.GetByIdAsync(userId);

        try
        {
            await Display(user);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
    }
    private async Task ViewAll()
    {
        AnsiConsole.MarkupLine("[bold green]VIEW ALL USERS[/]");
        var allUsers = await userService.GetAllAsync();

        try
        {
            foreach (var user in allUsers)
            {
                await Display(user);
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

    }

    private async Task Display(User user)
    {
        AnsiConsole.MarkupLine($"[bold]User ID:[/][yellow] {user.Id}[/]");
        AnsiConsole.MarkupLine($"[bold]Name:[/][yellow] {user.Name}[/]");
        AnsiConsole.MarkupLine($"[bold]Scores:[/][yellow] {user.Scores}[/]");

        if (user.LearnedWords.Count() > 0)
        {
            foreach (var word in user.LearnedWords)
            {
                await DisplayWords(word);
            }
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]No learnt words found[/]");
        }

        AnsiConsole.WriteLine();
    }

    private async Task DisplayWords(Word word)
    {
        var panel = new Panel($"{word.Name} - {word.Meaning}");
        panel.Header = new PanelHeader("Your learned words: ");
        panel.Expand = true;
        AnsiConsole.Write(panel);
    }
}
