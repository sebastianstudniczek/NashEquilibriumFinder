using Rationals;

namespace NashEquilibriumFinder.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static Rational Sum(this IEnumerable<Rational> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            Rational sum = Rational.Zero;

            foreach (Rational number in source)
            {
                sum += number;
            }

            return sum;
        }
    }
}
