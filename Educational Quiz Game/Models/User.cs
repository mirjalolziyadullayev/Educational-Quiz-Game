using Newtonsoft.Json;
namespace Educational_Quiz_Game.Models;
public class User
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("learned_words")]
    public List<Word> LearnedWords { get; set; }
    [JsonProperty("scores")]
    public int Scores { get; set; }
}


