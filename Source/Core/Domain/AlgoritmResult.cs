namespace NashEquilibriumFinder.Core.Domain;
public class AlgoritmResult
{
    // TODO: Refactor this
    public NashEquilibrium NashEquilibrium { get; init; } = null!;
    public IEnumerable<PayoutCoordinate> SortedPayouts { get; init; } = new List<PayoutCoordinate>();
    public int[][] ParetoFront { get; set; } = Array.Empty<int[]>();
    public int? ElapsedMilliseconds { get; set; }
}
