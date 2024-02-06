using Educational_Quiz_Game.Interfaces;
using Spectre.Console;
namespace Educational_Quiz_Game.Menu;
public class ProgressTrackerMenu
{
    private readonly IUserService userService;

    public ProgressTrackerMenu(IUserService userService)
    {
        this.userService = userService;
    }

    public void TrackProgress()
    {
        AnsiConsole.Status()
        .Start("Use...", ctx =>
        {
            AnsiConsole.MarkupLine("Tracking User Progress...");
            Thread.Sleep(1000);
        });
        AnsiConsole.Clear();
        AnsiConsole.Write("Result...\n");
        var users = userService.GetAllAsync().Result;
        var sortedUsers = users.OrderByDescending(u => u.Scores).ToList();

        if (sortedUsers.Any())
        {
            var table = new Table();
            table.AddColumn("Rank");
            table.AddColumn("Name");
            table.AddColumn("Scores");
            table.AddColumn("Medal");

            for (int i = 0; i < sortedUsers.Count; i++)
            {
                var user = sortedUsers[i];
                var rank = i + 1;
                var sticker = GetMedal(rank);

                table.AddRow(rank.ToString(), user.Name, user.Scores.ToString(), sticker);
            }

            AnsiConsole.Write(table);
        }
        else
        {
            Console.WriteLine("No users found.");
        }
    }

    private string GetMedal(int rank)
    {
        switch (rank)
        {
            case 1:
                return "Gold Medal";
            case 2:
                return "Silver Medal";
            case 3:
                return "Bronze Medal";
            default:
                return "no medal";
        }
    }

}