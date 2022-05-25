namespace NashEquilibriumFinder.Core.Domain;

public class NashEquilibrium
{
    public IEnumerable<SimpleFraction> FirstPlayerStrategyProbabilities { get; init; } = new List<SimpleFraction>();
    public IEnumerable<SimpleFraction> SecondPlayerStrategyProbabilities { get; init; } = new List<SimpleFraction>();
    public double FirstPlayerGamePayoff { get; init; }
    public double SecondPlayerGamePayoff { get; init; }
}
