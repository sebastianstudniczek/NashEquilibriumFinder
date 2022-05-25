namespace NashEquilibriumFinder.Core.Domain;

public class Player
{
    public string? Name { get; set; }

    public int[][] Payouts { get; set; } = Array.Empty<int[]>();
}
