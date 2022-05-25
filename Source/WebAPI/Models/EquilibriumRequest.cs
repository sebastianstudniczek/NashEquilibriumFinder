using NashEquilibriumFinder.Core.Domain;

namespace NashEquilbriumFinder.WebAPI.Models;

public class EquilibriumRequest
{
    public string? Title { get; set; }
    public Player FirstPlayer { get; set; } = null!;
    public Player SecondPlayer { get; set; } = null!;
}
