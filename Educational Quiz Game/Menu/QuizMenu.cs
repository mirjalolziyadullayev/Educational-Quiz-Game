using Educational_Quiz_Game.Interfaces;
using Educational_Quiz_Game.Models;
using Spectre.Console;
namespace Educational_Quiz_Game.Menu;
public class QuizMenu
{
    private readonly IUserService userService;
    private readonly IWordService wordService;

    public QuizMenu(IUserService userService, IWordService wordService)
    {
        this.userService = userService;
        this.wordService = wordService;
    }

    public void StartQuizMenu()
    {
        Console.Write("Enter user ID to start the quiz: ");
        int userId;
        while (!int.TryParse(Console.ReadLine(), out userId))
        {
            Console.Write("Invalid input. Enter a valid user ID: ");
        }
        try
        {
            var users = userService.GetAllAsync().Result;
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine("User not found");
                return;
            }
            if (user.LearnedWords.Count == 0)
            {
                Console.WriteLine("There is no any word to start quiz!");
                Console.WriteLine();
                return;
            }
            Console.WriteLine("Starting Quiz...");

            var wordsList = wordService.GetAllAsync().Result;
            var selectedWords = wordsList.Take(5);
            var notLearnedWords = new List<Word>();
            Console.WriteLine("Quiz Instructions:");
            Console.WriteLine("Write the meaning for each word.");

            int totalScore = 0;

            var updateTasks = new List<Task>();

            foreach (var word in selectedWords)
            {
                Console.Write($"Word: {word.Name} - Your Answer: ");
                string userAnswer = Console.ReadLine()?.Trim();

                if (userAnswer != null && userAnswer.ToLower() == word.Meaning.ToLower())
                {
                    updateTasks.Add(Task.Run(() => wordService.UpdateIsLearnedStatusAsync(word.Id).GetAwaiter().GetResult()));

                    totalScore += 10;
                    Console.WriteLine($"Correct! +10 scores {Emoji.Known.SmilingFaceWithSunglasses}");
                }
                else
                {
                    notLearnedWords.Add(word);
                    Console.WriteLine($"Incorrect. {Emoji.Known.AnguishedFace} The correct meaning is: {word.Meaning}");
                }
            }

            user = userService.GetByIdAsync(userId).Result;

            updateTasks.Add(Task.Run(() => userService.UpdateScoreAsync(userId, user.Scores + totalScore).GetAwaiter().GetResult()));

            Task.WaitAll(updateTasks.ToArray());

            Console.WriteLine($"Quiz completed! Total Score: {totalScore}");

            Console.WriteLine("**************");
            Console.WriteLine("You should revise these words: ");
            foreach (var word in notLearnedWords)
            {
                DisplayWords(word);
                wordService.UpdateIsLearnedStatusAsync(word.Id).GetAwaiter();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public void DisplayWords(Word word)
    {
        var table = new Table();
        table.AddColumn("Word");
        table.AddColumn("Meaning");

        table.AddRow(word.Name, word.Meaning);

        AnsiConsole.Write(table);
    }
}
