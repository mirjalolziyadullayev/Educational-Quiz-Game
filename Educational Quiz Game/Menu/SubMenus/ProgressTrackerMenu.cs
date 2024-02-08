using QuizGameEDU.Interfaces;
using QuizGameEDU.Models;
using Spectre.Console;

namespace Educational_Quiz_Game.Menu.SubMenus
{
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
                .Start("Tracking Progress...", ctx =>
                {
                    Thread.Sleep(1000);
                });

            AnsiConsole.Clear();
            AnsiConsole.WriteLine("Result...\n");

            var users = userService.GetAllAsync().Result;
            var sortedUsers = users.OrderByDescending(u => u.Scores).ToList();

            if (sortedUsers.Any())
            {
                var positionMap = GetPositionMap(sortedUsers);

                var table = new Table();
                table.AddColumn("Rank");
                table.AddColumn("Name");
                table.AddColumn("Score");
                table.AddColumn("Position");

                foreach (var user in sortedUsers)
                {
                    var rank = positionMap[user];
                    var position = GetMedal(rank);

                    table.AddRow(rank.ToString(), user.Name, user.Scores.ToString(), position);
                }

                AnsiConsole.Render(table);
            }
            else
            {
                Console.WriteLine("No users found.");
            }
        }

        private Dictionary<User, int> GetPositionMap(List<User> sortedUsers)
        {
            var positionMap = new Dictionary<User, int>();
            int currentPosition = 1;
            int previousScore = sortedUsers[0].Scores;

            for (int i = 0; i < sortedUsers.Count; i++)
            {
                if (sortedUsers[i].Scores < previousScore)
                {
                    currentPosition = i + 1;
                    previousScore = sortedUsers[i].Scores;
                }
                positionMap.Add(sortedUsers[i], currentPosition);
            }

            return positionMap;
        }

        private string GetMedal(int rank)
        {
            switch (rank)
            {
                case 1:
                    return "1st";
                case 2:
                    return "2nd";
                case 3:
                    return "3rd";
                default:
                    return $"{rank}th";
            }
        }
    }
}
