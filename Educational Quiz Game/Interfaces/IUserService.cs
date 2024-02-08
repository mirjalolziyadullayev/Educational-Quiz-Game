using QuizGameEDU.Models;
namespace QuizGameEDU.Interfaces;
public interface IUserService
{
    ValueTask<User> CreateAsync(User user);
    ValueTask<User> GetByIdAsync(int id);
    ValueTask<IEnumerable<User>> GetAllAsync();
    ValueTask<bool> UpdateAsync(int id, User user);
    ValueTask<bool> DeleteAsync(int id);
    ValueTask<bool> UpdateScoreAsync(int id, int score);
}
