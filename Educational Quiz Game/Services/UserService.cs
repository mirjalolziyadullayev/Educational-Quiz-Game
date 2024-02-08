using QuizGameEDU.Interfaces;
using QuizGameEDU.Models;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace QuizGameEDU.Services;
public class UserService : IUserService
{
    public async ValueTask<User> CreateAsync(User user)
    {
        var data = File.ReadAllText(Constants.USERS_PATH);
        var users = JsonConvert.DeserializeObject<List<User>>(data) ?? new List<User>();

        if (users.Count == 0)
        {
            user.Id = 1;
        }
        else
        {
            var lastWord = users.Last();
            user.Id = lastWord.Id + 1;
        }

        users.Add(user);

        data = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(Constants.USERS_PATH, data);
        return user;
    }

    public async ValueTask<User> GetByIdAsync(int id)
    {
        var data = File.ReadAllText(Constants.USERS_PATH);
        var users = JsonConvert.DeserializeObject<List<User>>(data) ?? new List<User>();

        var user = users.FirstOrDefault(u => u.Id == id)
            ?? throw new Exception($"User is not found with id {id}");

        return user;
    }
    public async ValueTask<IEnumerable<User>> GetAllAsync()
    {
        var data = File.ReadAllText(Constants.USERS_PATH);
        var users = JsonConvert.DeserializeObject<List<User>>(data) ?? new List<User>();

        return users;
    }
    public async ValueTask<bool> UpdateAsync(int id, User user)
    {
        var data = File.ReadAllText(Constants.USERS_PATH);
        var users = JsonConvert.DeserializeObject<List<User>>(data) ?? new List<User>();

        var existingUser = users.FirstOrDefault(u => u.Id == id)
            ?? throw new Exception($"User is not found with id {id}");

        existingUser.Name = user.Name;

        existingUser.LearnedWords = new List<Word>();

        existingUser.Scores = 0;

        data = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(Constants.USERS_PATH, data);

        return true;
    }

    public async ValueTask<bool> UpdateScoreAsync(int id, int score)
    {
        var content = await File.ReadAllTextAsync(Constants.USERS_PATH);
        var users = JsonConvert.DeserializeObject<IEnumerable<User>>(content);

        var existingUser = users.FirstOrDefault(u => u.Id == id)
            ?? throw new Exception($"User is not found with id {id}");

        existingUser.Scores += score;

        var res = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(Constants.USERS_PATH, res);

        return true;
    }

    public async ValueTask<bool> DeleteAsync(int id)
    {
        var data = File.ReadAllText(Constants.USERS_PATH);
        var users = JsonConvert.DeserializeObject<List<User>>(data) ?? new List<User>();

        var user = users.FirstOrDefault(u => u.Id == id)
            ?? throw new Exception($"User is not found with id {id}");

        users.Remove(user);

        data = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(Constants.USERS_PATH, data);

        return true;
    }
}