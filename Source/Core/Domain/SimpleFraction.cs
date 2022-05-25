namespace NashEquilibriumFinder.Core.Domain;
public struct SimpleFraction
{
    public static readonly SimpleFraction Zero = new(0);
    public SimpleFraction(long numerator, long denominator = 1)
    {
        Numerator = numerator;
        Denominator = denominator;
    }

    public long Numerator { get; set; }
    public long Denominator { get; set; }

    public static implicit operator SimpleFraction(int number) => new(number);
    public static implicit operator SimpleFraction(long number) => new(number);

    public override string ToString() {
        return $"{Numerator}/{Denominator}";
    }
}
