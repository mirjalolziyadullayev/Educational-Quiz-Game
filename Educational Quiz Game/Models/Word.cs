﻿using Newtonsoft.Json;
namespace Educational_Quiz_Game.Models;
public class Word
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("meaning")]
    public string Meaning { get; set; }
    [JsonProperty("is_learned_status")]
    public bool IsLearnedStatus { get; set; }
}