using NashEquilibriumFinder.Application.UnitTests.TestData;
using NashEquilibriumFinder.Core.Contracts;
using NashEquilibriumFinder.Core.Domain;
using NashEquilibriumFinder.Core.Services;
using Xunit;

namespace NashEquilibriumFinder.Application.UnitTests;

public class GameTheoryServiceTests
{
    private readonly IGameTheoryService _service;

    public GameTheoryServiceTests()
    {
        _service = new GameTheoryService();
    }

    [Theory]
    [ClassData(typeof(NashEquilibriumTestData))]
    public void GetFromLemkeHowson_ShouldReturnValidResult(Game gameData, NashEquilibrium expectedEquilibrium)
    {
        NashEquilibrium result = _service.GetNashEquilibriumFromLemkeHowson(gameData);

        // TODO: Implement players game payoffs to the rest of games
        Assert.Equal(expectedEquilibrium.FirstPlayerStrategyProbabilities, result.FirstPlayerStrategyProbabilities);
        Assert.Equal(expectedEquilibrium.SecondPlayerStrategyProbabilities, result.SecondPlayerStrategyProbabilities);
    }

    [Theory]
    [ClassData(typeof(ParetoFrontTestData))]
    public void GetFromLemkeHowson_ShouldReturnValidParetoFront(
        int[,] firstPlayerMatrix,
        int[,] secondPlayerMatrix,
        int[][] expectedParetoFront)
    {
        int[][] result = _service.GetParetoFront(firstPlayerMatrix, secondPlayerMatrix);

        Assert.Equal(expectedParetoFront, result);
    }

    [Theory]
    [ClassData(typeof(CorrelatedEquilibriumTestData))]
    public void GetCorrelatedEquilirbiumFromSimplex_ShouldReturnValidResult(Game gameData, CorrelatedEquilibrium expectedEquilibrium)
    {
        CorrelatedEquilibrium result = _service.GetCorrelatedEquilibriumFromSimplex(gameData);

        Assert.Equal(expectedEquilibrium.FirstPlayerGamePayoff, result.FirstPlayerGamePayoff);
        Assert.Equal(expectedEquilibrium.SecondPlayerGamePayoff, result.SecondPlayerGamePayoff);
        Assert.Equal(expectedEquilibrium.StrategyProfilesProbabilities.ToArray(), result.StrategyProfilesProbabilities);
    }
}
