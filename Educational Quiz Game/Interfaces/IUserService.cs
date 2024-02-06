using Educational_Quiz_Game.Models;

namespace Educational_Quiz_Game.Interfaces;
public interface IUserService
{
    ValueTask<User> CreateAsync(User user);
    ValueTask<User> GetByIdAsync(int id);
    ValueTask<IEnumerable<User>> GetAllAsync();
    ValueTask<bool> UpdateAsync(int id, User user);
    ValueTask<bool> DeleteAsync(int id);
    ValueTask<bool> UpdateScoreAsync(int id, int score);
}
