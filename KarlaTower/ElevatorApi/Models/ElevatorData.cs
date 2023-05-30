using System.Text.Json.Serialization;

namespace KarlaTower.Models;

public class ElevatorData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("currentFloor")]
    public int CurrentFloor { get; set; }
    
    [JsonPropertyName("targetFloor")]
    public int TargetFloor { get; set; }
    
    [JsonPropertyName("direction")]
    public Direction Direction { get; set; }
    
    [JsonPropertyName("isMoving")]
    public bool IsMoving { get; set; }
    
    [JsonPropertyName("occupants")]
    public int Occupants { get; set; }
    
    [JsonPropertyName("maxOccupants")]
    public int MaxOccupants { get; set; }
    
    [JsonPropertyName("currentSong")]
    public string CurrentSong { get; set; } = string.Empty;
}