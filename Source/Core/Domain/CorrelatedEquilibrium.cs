namespace NashEquilibriumFinder.Core.Domain;
public class CorrelatedEquilibrium
{
    public double FirstPlayerGamePayoff { get; init; }
    public double SecondPlayerGamePayoff { get; init; }
    public IEnumerable<SimpleFraction> StrategyProfilesProbabilities { get; init; } = new List<SimpleFraction>();
}
