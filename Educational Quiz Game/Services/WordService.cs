using Educational_Quiz_Game.Interfaces;
using Educational_Quiz_Game.Models;
using Newtonsoft.Json;
namespace Educational_Quiz_Game.Services;
public class WordService : IWordService
{
    public async ValueTask<Word> CreateAsync(Word word, int userId)
    {

        var data = File.ReadAllText(Constants.WORDS_PATH);
        var words = JsonConvert.DeserializeObject<List<Word>>(data) ?? new List<Word>();

        if (words.Count == 0)
        {
            word.Id = 1;
        }
        else
        {
            var lastWord = words.Last();
            word.Id = lastWord.Id + 1;
        }

        words.Add(word);

        data = JsonConvert.SerializeObject(words, Formatting.Indented);
        File.WriteAllText(Constants.WORDS_PATH, data);

        var userData = File.ReadAllText(Constants.USERS_PATH);
        var users = JsonConvert.DeserializeObject<List<User>>(userData) ?? new List<User>();

        var user = users.FirstOrDefault(u => u.Id == userId)
            ?? throw new Exception($"User is not found with id {userId}");

        user.LearnedWords.Add(word);

        var updatedUserData = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(Constants.USERS_PATH, updatedUserData);


        return word;

    }
    public async ValueTask<bool> UpdateIsLearnedStatusAsync(int id)
    {
        var data = File.ReadAllText(Constants.WORDS_PATH);
        var words = JsonConvert.DeserializeObject<List<Word>>(data) ?? new List<Word>();
        var word = words.FirstOrDefault(w => w.Id == id)
            ?? throw new Exception($"Word is not found with id {id}");
        word.IsLearnedStatus = true;

        var userData = File.ReadAllText(Constants.USERS_PATH);
        var users = JsonConvert.DeserializeObject<List<User>>(userData) ?? new List<User>();

        foreach (var user in users)
        {
            var learntWord = user.LearnedWords.FirstOrDefault(w => w.Id == id);
            if (learntWord != null)
            {
                learntWord.IsLearnedStatus = true;
            }
        }

        var updatedContent = JsonConvert.SerializeObject(words, Formatting.Indented);
        await File.WriteAllTextAsync(Constants.WORDS_PATH, updatedContent);

        var updatedUserData = JsonConvert.SerializeObject(users, Formatting.Indented);
        await File.WriteAllTextAsync(Constants.USERS_PATH, updatedUserData);

        return true;
    }


    public async ValueTask<Word> GetByIdAsync(int id)
    {
        var data = File.ReadAllText(Constants.WORDS_PATH);
        var words = JsonConvert.DeserializeObject<List<Word>>(data) ?? new List<Word>();

        var word = words.FirstOrDefault(w => w.Id == id)
            ?? throw new Exception($"User is not found with id {id}");

        return word;
    }

    public async ValueTask<IEnumerable<Word>> GetAllAsync()
    {
        var data = File.ReadAllText(Constants.WORDS_PATH);
        var words = JsonConvert.DeserializeObject<List<Word>>(data) ?? new List<Word>();

        return words;
    }

    public async ValueTask<bool> DeleteAsync(int id)
    {
        var data = File.ReadAllText(Constants.WORDS_PATH);
        var words = JsonConvert.DeserializeObject<List<Word>>(data) ?? new List<Word>();

        var word = words.FirstOrDefault(x => x.Id == id)
            ?? throw new Exception($"User is not found with id {id}");

        words.Remove(word);

        data = JsonConvert.SerializeObject(words, Formatting.Indented);
        File.WriteAllText(Constants.WORDS_PATH, data);

        return true;
    }
}