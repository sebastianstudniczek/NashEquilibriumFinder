namespace NashEquilibriumFinder.Core.Domain;

public class Game
{
    public string? Title { get; set; }
    public Player FirstPlayer { get; set; } = null!;
    public Player SecondPlayer { get; set; } = null!;
}