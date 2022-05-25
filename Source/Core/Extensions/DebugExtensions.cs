using Rationals;
using System.Diagnostics;

namespace NashEquilibriumFinder.Core.Extensions;

public static class DebugExtensions
{
    public static string Test2D(this Rational[,] source, int pad = 3)
    {
        string result = "";
        for (int i = source.GetLowerBound(0); i <= source.GetUpperBound(0); i++)
        {
            for (int j = source.GetLowerBound(1); j <= source.GetUpperBound(1); j++)
            {
                result += source.GetValue(i, j).ToString().PadLeft(pad);
                result += " |";
            }
            result += "\n";
        }
        return result;
    }

    public static void Print2D(Rational[,] source)
    {
        string result = "";
        for (int i = source.GetLowerBound(0); i <= source.GetUpperBound(0); i++)
        {
            result = "";
            for (int j = source.GetLowerBound(1); j <= source.GetUpperBound(1); j++)
            {
                if (!source[i,j].IsNaN) {
                    //result += Math.Round(((double)source[i, j]), 2).ToString().PadLeft(6);
                    result += source[i, j].CanonicalForm.ToString().PadLeft(6);
                } else {
                    result += "NaN";
                }

                result += "|";
            }
            Debug.Print(result);
        }
    }
}
