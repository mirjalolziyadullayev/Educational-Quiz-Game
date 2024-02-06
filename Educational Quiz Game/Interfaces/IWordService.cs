using Educational_Quiz_Game.Models;
namespace Educational_Quiz_Game.Interfaces;
public interface IWordService
{
    ValueTask<Word> CreateAsync(Word word, int id);
    ValueTask<Word> GetByIdAsync(int id);
    ValueTask<IEnumerable<Word>> GetAllAsync();
    ValueTask<bool> DeleteAsync(int id);
    ValueTask<bool> UpdateIsLearnedStatusAsync(int id);
}
