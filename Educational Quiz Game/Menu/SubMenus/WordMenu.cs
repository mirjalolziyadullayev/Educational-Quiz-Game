using QuizGameEDU.Interfaces;
using QuizGameEDU.Models;
using QuizGameEDU.Services;
using Spectre.Console;
namespace Educational_Quiz_Game.Menu.SubMenus;
public class WordMenu
{
    private readonly IWordService wordService;
    private readonly UserService userService;

    public WordMenu(IWordService wordService, UserService userService)
    {
        this.wordService = wordService;
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
                    .AddChoices("Create Word", "View Word", "View all Words", "Delete Word", "Back"));

            switch (choice)
            {
                case "Create Word":
                    AnsiConsole.Clear();
                    await Create();
                    break;
                case "View Word":
                    AnsiConsole.Clear();
                    await View();
                    break;
                case "View all Words":
                    AnsiConsole.Clear();
                    await ViewAll();
                    break;
                case "Delete Word":
                    AnsiConsole.Clear();
                    await Delete();
                    break;
                case "Back":
                    AnsiConsole.Clear();
                    return;
            }
        }
    }

    private async Task Create()
    {
        AnsiConsole.MarkupLine("[bold green]CREATE WORD[/]");
        var userId = AnsiConsole.Ask<int>("Enter user id: ");
        var name = AnsiConsole.Ask<string>("Enter word name: ");
        var meaning = AnsiConsole.Ask<string>("Enter word meaning: ");

        var newWord = new Word
        {
            Name = name,
            Meaning = meaning,
            IsLearnedStatus = false
        };
        try
        {
            var user = await userService.GetByIdAsync(userId);
            var createdWord = await wordService.CreateAsync(newWord, user.Id);
            AnsiConsole.MarkupLine($"[bold]Word created successfully![/] Word ID: [yellow]{createdWord.Id}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");
        }
    }

    private async Task View()
    {
        AnsiConsole.MarkupLine("[bold green]VIEW WORD[/]");

        var wordId = AnsiConsole.Ask<int>("Enter word ID: ");

        try
        {
            var word = await wordService.GetByIdAsync(wordId);
            AnsiConsole.MarkupLine($"[bold]Word Details:[/]");
            AnsiConsole.MarkupLine($"ID: [yellow]{word.Id}[/]");
            AnsiConsole.MarkupLine($"Name: [yellow]{word.Name}[/]");
            AnsiConsole.MarkupLine($"Meaning: [yellow]{word.Meaning}[/]");
            AnsiConsole.MarkupLine($"Is Learned: [yellow]{word.IsLearnedStatus}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");
        }

    }

    private async Task ViewAll()
    {
        try
        {
            AnsiConsole.MarkupLine("[bold green]VIEW ALL WORDS[/]");
            var allWords = await wordService.GetAllAsync();
            foreach (var word in allWords)
            {
                AnsiConsole.MarkupLine($"[bold]Word ID:[/] [yellow]{word.Id}[/] - [bold]Name:[/] [yellow]{word.Name}[/]");
                AnsiConsole.MarkupLine($"[bold]Meaning:[/] [yellow]{word.Meaning}[/] - [bold]Is Learned:[/] [yellow]{word.IsLearnedStatus}[/]");
                AnsiConsole.WriteLine();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");
        }

    }

    private async Task Delete()
    {
        AnsiConsole.MarkupLine("[bold green]DELETE WORD[/]");

        var wordId = AnsiConsole.Ask<int>("Enter word ID: ");

        try
        {
            await wordService.DeleteAsync(wordId);
            AnsiConsole.MarkupLine("[bold]Word deleted successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");
        }
    }
}