using NashEquilibriumFinder.Core.Algorithms;
using Rationals;
using Xunit;
using Q = Rationals.Rational;

namespace NashEquilibriumFinder.Algorithms.UnitTests;

public class LemkeHowsonTests
{
    [Fact]
    public void GetNormalizedMatrixes_MatrixesWithSomeNegativeValues_ShouldReturnValidMatrixes()
    {
        int[,] firstPlayerPayoutsMatrix = new int[,]
        {
            {-2, 3, -3 },
            { 4, 2, -1 }
        };
        int[,] secondPlayerPayoutsMatrix = new int[,]
        {
            { 3, -2, 4 },
            {-2,  1, 2 }
        };
        int[,] expectedFirstPlayerPayoutsMatrix = new int[,]
        {
            { 2, 7, 1 },
            { 8, 6, 3 }
        };
        int[,] expectedSecondPlayerPayoutsMatrix = new int[,]
        {
            { 7, 2, 8 },
            { 2, 5, 6 }
        };

        (int[,] FirstPlayerMatrix, int[,] SecondPlayerMatrix) actualMatrixes = LemkeHowson.GetNormalizedMatrixes(
            firstPlayerPayoutsMatrix, secondPlayerPayoutsMatrix);

        Assert.Equal(expectedFirstPlayerPayoutsMatrix, actualMatrixes.FirstPlayerMatrix);
        Assert.Equal(expectedSecondPlayerPayoutsMatrix, actualMatrixes.SecondPlayerMatrix);
    }

    [Fact]
    public void CreateTableaux_ShouldReturnValidTableaux()
    {
        Q[,] actualTableaux;
        var firstPlayerPayoutsMatrix = new int[,]
        {
            { 5, 5,10 },
            { 1, 8, 3 }
        };
        var secondPlayerPayoutsMatrix = new int[,]
        {
            { 2, 8, 2 },
            { 3, 2, 7 }
        };
        var expectedTabluex = new Q[,]
        {
            { 0, 0, 1, 2, 3, 4, 5 },
            {-1, 1, 0, 0,-5,-5,-10 },
            {-2, 1, 0, 0,-1,-8, -3 },
            {-3, 1,-2,-3, 0, 0,  0 },
            {-4, 1,-8,-2, 0, 0,  0 },
            {-5, 1,-2,-7, 0, 0,  0 }
        };

        actualTableaux = LemkeHowson.CreateTableaux(firstPlayerPayoutsMatrix, secondPlayerPayoutsMatrix);

        Assert.Equal(expectedTabluex, actualTableaux);
    }

    [Fact]
    public void GetMinRatioRowVariable_ShouldReturnValidVariable()
    {
        const int EnterBasisVariable = 2;
        const int ExpectedLeaveBasisVariable = -4;
        var tableaux = new Q[,]
        {
            { 0, 0, 1, 2, 3, 4,  5 },
            {-1, 1, 0, 0,-5,-5,-10 },
            {-2, 1, 0, 0,-1,-8, -3 },
            {-3, 1,-2,-3, 0, 0,  0 },
            {-4, 1,-8,-2, 0, 0,  0 },
            {-5, 1,-2,-7, 0, 0,  0 }
        };

        int actualVariable = LemkeHowson.GetMinRatioRowVariable(tableaux, EnterBasisVariable);

        Assert.Equal(ExpectedLeaveBasisVariable, actualVariable);
    }

    [Theory]
    [ClassData(typeof(GameDataGenerator))]
    public void MakePivotStep_ValidTableaux_ShouldMakePivotAndReturnValidIndexToMakeNextPivot(
        int valueToEnterBasis,
        Q[,] tableaux,
        Q[,] expectedTableauxAfterPivot)
    {
        const int FirstPlayerStrategiesCount = 2;

        LemkeHowson.MakePivotStep(
            ref tableaux,
            valueToEnterBasis,
            FirstPlayerStrategiesCount);

        Assert.Equal(expectedTableauxAfterPivot, tableaux);
    }

    [Fact]
    public void GetNashEquilibrium_ValidTableaux_ShouldReturnNashERationaluilibrium()
    {
        const int FirstPlayerStrategiesCount = 2;
        var tableaux = new Q[,]
        {
            { 0, 0, 1, 2, 3, 4, 5  },
            {-1, 1, 0, 0,-5,-5,-10 },
            {-2, 1, 0, 0,-1,-8, -3 },
            {-3, 1,-2,-3, 0, 0,  0 },
            {-4, 1,-8,-2, 0, 0,  0 },
            {-5, 1,-2,-7, 0, 0,  0 }
        };
        List<Q>? expectedSolution = new()
        {
            (Q)5 / 52,
            (Q)3 / 26,
            Q.Zero,
            (Q)7 / 65,
            (Q)3 / 65
        };

        IEnumerable<Q> actualSolution = LemkeHowson.GetNashEquilibrium(tableaux, FirstPlayerStrategiesCount);

        Assert.Equal(expectedSolution, actualSolution);
    }

    [Fact]
    public void GetNormalizedNashEquilibrium_ValidNashERationaluilibrium_ShouldReturnNormalizedNashERationaluilibrium()
    {
        const int firstPlayerStrategiesCount = 2;
        var nashERationaluilibrium = new List<Rational>()
        {
            (Q)5/52,
            (Q)3/26,
                  0,
            (Q)7/65,
            (Q)3/65
        };
        var expectedNormalizedNashERationaluilibrium = new List<Rational>()
        {
            (Q)5/11,
            (Q)6/11,
        };
        var expectedSecondPlayerNashERationaluilibrium = new List<Rational>()
        {
                  0,
            (Q)7/10,
            (Q)3/10
        };

        (List<Q> FirstPlayer, List<Q> SecondPlayer) = LemkeHowson
            .GetNormalizedNashEquilibrium(nashERationaluilibrium, firstPlayerStrategiesCount);

        Assert.Equal(expectedNormalizedNashERationaluilibrium, FirstPlayer);
        Assert.Equal(expectedSecondPlayerNashERationaluilibrium, SecondPlayer);
    }

    [Fact]
    public void GetPlayersGamePayoffs_ValidNashEquilibrium_ShouldReturnCorrectPlayersGamePayoffs()
    {
        var firstPlayerPayoutsMatrix = new int[,]
        {
            { 5, 5,10 },
            { 1, 8, 3 }
        };
        var secondPlayerPayoutsMatrix = new int[,]
        {
            { 2, 8, 2 },
            { 3, 2, 7 }
        };
        var firstPlayerStrategyProbabilities = new List<Q>()
        {
            new Q(5,11),
            new Q(6,11)
        };
        var secondPlayerStrategyProbabilities = new List<Q>()
        {
            new Q(0,1),
            new Q(7, 10),
            new Q(3,10)
        };
        const double expectedFirstPlayerGamePayoff = 6.5;
        const double expectedSecondPlayerGamePayoff = 4.73;

        (double firstPlayersGamePayoff, double secondPlayerGamePayoff) =
            LemkeHowson.GetPlayersGamePayoffs(
                firstPlayerPayoutsMatrix,
                secondPlayerPayoutsMatrix,
                firstPlayerStrategyProbabilities,
                secondPlayerStrategyProbabilities);

        Assert.Equal(expectedFirstPlayerGamePayoff, firstPlayersGamePayoff);
        Assert.Equal(expectedSecondPlayerGamePayoff, secondPlayerGamePayoff);
    }
}
