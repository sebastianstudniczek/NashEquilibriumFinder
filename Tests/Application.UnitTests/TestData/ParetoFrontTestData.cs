using Xunit;

namespace NashEquilibriumFinder.Application.UnitTests.TestData;

internal class ParetoFrontTestData : TheoryData<int[,], int[,], int[][]>
{
    public ParetoFrontTestData()
    {
        Add(_game1.firstPlayerPayouts, _game1.secondPlayerPayouts, _game1.expectedParetoCoordinates);
        Add(_game2.firstPlayerPayouts, _game2.secondPlayerPayouts, _game2.expectedParetoCoordinates);
        Add(_game3.firstPlayerPayouts, _game3.secondPlayerPayouts, _game3.expectedParetoCoordinates);
        Add(_game4.firstPlayerPayouts, _game4.secondPlayerPayouts, _game4.expectedParetoCoordinates);
    }

    private readonly (int[,] firstPlayerPayouts, int[,] secondPlayerPayouts, int[][] expectedParetoCoordinates) _game1 = (
        new int[,]
        {
            { 2, 3 },
            { 1, 0 }
        },
        new int[,]
        {
            { 3, 2 },
            { 0, 1 }
        },
        new int[][]
        {
            new int[] { 3, 2},
            new int[] { 2, 3}
        });

    private readonly (int[,] firstPlayerPayouts, int[,] secondPlayerPayouts, int[][] expectedParetoCoordinates) _game2 = (
        new int[,]
        {
            { 2, 1 },
            { 3, 0 }
        },
        new int[,]
        {
            { 4, 0 },
            { 1 ,4 }
        },
        new int[][]
        {
            new int[] { 3, 1 },
            new int[] { 2, 4 }
        });

    private readonly (int[,] firstPlayerPayouts, int[,] secondPlayerPayouts, int[][] expectedParetoCoordinates) _game3 = (
        new int[,]
        {
            { 1,  2 },
            { 5, -1 }
        },
        new int[,]
        {
            { 1,  5 },
            { 2, -1 }
        },
        new int[][]
        {
            new int[] { 5, 2},
            new int[] { 2, 5}
        });

    private readonly (int[,] firstPlayerPayouts, int[,] secondPlayerPayouts, int[][] expectedParetoCoordinates) _game4 = (
        new int[,]
        {
            { 3, -1 },
            { 5, 0 }
        },
        new int[,]
        {
            {  3, 5 },
            { -1, 0 }
        },
        new int[][]
        {
            new int[] { 5, -1},
            new int[] { 3,  3},
            new int[] { -1, 5}
        });
}