namespace NashEquilibriumFinder.Core.Domain;

public struct PayoutCoordinate
{
    public int FirstPlayerCleanStrategyPayoff { get; set; }
    public int SecondPlayerCleanStrategyPayoff { get; set; }
    public char? FirstPlayerStrategyName { get; set; }
    public char? SecondPlayerStrategyName { get; set; }
}