using NashEquilibriumFinder.Core.Domain;

namespace NashEquilibriumFinder.Core.Helpers;

public static class ChartDataConverter
{
    public static IEnumerable<PayoutCoordinate> SortPointsCounterclockwise(
        int[,] firstPlayerMatrix,
        int[,] secondPlayerMatrix)
    {
        if (firstPlayerMatrix.GetLength(0) != secondPlayerMatrix.GetLength(0) ||
            firstPlayerMatrix.GetLength(1) != secondPlayerMatrix.GetLength(1))
        {
            throw new Exception("The matrixes are not of the same size.");
        }

        int firstPlayerStrategiesCount = firstPlayerMatrix.GetLength(0);
        int secondPlayerStrategiesCount = secondPlayerMatrix.GetLength(1);
        int cleanStrategiesCombinationNumber = firstPlayerStrategiesCount * secondPlayerStrategiesCount;

        var polygonNodes = new List<PayoutCoordinate>();

        const char firstPlayerFirstStrategyName = 'A';
        char secondPlayerFirstStrategyName = (char)((int)firstPlayerFirstStrategyName + firstPlayerStrategiesCount);

        // Add unsorted points to list
        for (int i = 0; i < firstPlayerMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < firstPlayerMatrix.GetLength(1); j++)
            {
                polygonNodes.Add(new PayoutCoordinate
                {
                    FirstPlayerStrategyName = (char)((int)firstPlayerFirstStrategyName + i),
                    SecondPlayerStrategyName = (char)((int)secondPlayerFirstStrategyName + j),
                    FirstPlayerCleanStrategyPayoff = firstPlayerMatrix[i, j],
                    SecondPlayerCleanStrategyPayoff = secondPlayerMatrix[i, j]
                });
            }
        }
        double xReferencePoint = GetAverageOfElements(firstPlayerMatrix);
        double yReferencePoint = GetAverageOfElements(secondPlayerMatrix);

        List<PayoutCoordinate> sortedCoordinates = polygonNodes
            .OrderBy(node => Math.Atan2( //TODO: How this works?
                node.SecondPlayerCleanStrategyPayoff - yReferencePoint,
                node.FirstPlayerCleanStrategyPayoff - xReferencePoint))
            .ToList();

        return sortedCoordinates;
    }

    public static double GetAverageOfElements(int[,] array)
    {
        int yDimension = array.GetLength(0);
        int xDimension = array.GetLength(1);

        int elementsCount = xDimension * yDimension;

        double elementsSum = 0;

        for (int i = 0; i < yDimension; i++)
        {
            for (int j = 0; j < xDimension; j++)
            {
                elementsSum += array[i, j];
            }
        }

        return elementsSum / elementsCount;
    }

    public static double GetAverageOfElements(int[][] array)
    {
        int yDimension = array.GetLength(0);
        int xDimension = array.GetLength(1);

        int elementsCount = xDimension * yDimension;

        double elementsSum = 0;

        for (int i = 0; i < yDimension; i++)
        {
            for (int j = 0; j < xDimension; j++)
            {
                elementsSum += array[i][j];
            }
        }

        return elementsSum / elementsCount;
    }
}
