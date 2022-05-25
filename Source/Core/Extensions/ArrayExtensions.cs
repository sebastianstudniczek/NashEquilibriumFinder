using Rationals;

namespace NashEquilibriumFinder.Core.Extensions;

public static class ArrayExtensions
{
    public static Rational[,] To2dArray(this int[][] array)
    {
        int arrayHeight = array.Length;
        int arrayLength = array[0].Length;

        var resultArray = new Rational[arrayHeight, arrayLength];

        for (int i = 0; i < arrayHeight; i++)
        {
            for (int j = 0; j < arrayLength; j++)
            {
                resultArray[i, j] = array[i][j];
            }
        }

        return resultArray;
    }

    public static T[,] To2dArray<T>(this T[][] array)
    {
        int arrayHeight = array.Length;
        int arrayLength = array[0].Length;

        var resultArray = new T[arrayHeight, arrayLength];

        for (int i = 0; i < arrayHeight; i++)
        {
            for (int j = 0; j < arrayLength; j++)
            {
                resultArray[i, j] = array[i][j];
            }
        }

        return resultArray;
    }

    /// <summary>
    /// Returns a transposed array.
    /// </summary>
    /// <param name="arrayToTranspose"></param>
    /// <returns>A two dimensional transposed integer array.</returns>
    public static int[,] GetTranposed(this int[,] arrayToTranspose)
    {
        var transposedArray = new int[arrayToTranspose.GetLength(1), arrayToTranspose.GetLength(0)];

        for (int i = 0; i < transposedArray.GetLength(0); i++)
        {
            for (int j = 0; j < transposedArray.GetLength(1); j++)
            {
                transposedArray[i, j] = arrayToTranspose[j, i];
            }
        }

        return transposedArray;
    }

    public static void InitializeWithZero(this Rational[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = Rational.Zero;
        }
    }

    public static void InitializeWithZero(this Rational[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                array[i, j] = Rational.Zero;
            }
        }
    }

    public static int GetBaseColumnIndex<T>(this T[,] array)
    {
        return array.GetLength(1) - 1;
    }

    public static int GetCjColumnIndex<T>(this T[,] array)
    {
        return array.GetLength(1) - 1 - 1;
    }
}