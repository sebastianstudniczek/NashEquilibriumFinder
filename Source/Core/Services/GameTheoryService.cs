using NashEquilibriumFinder.Core.Algorithms;
using NashEquilibriumFinder.Core.Contracts;
using NashEquilibriumFinder.Core.Domain;
using NashEquilibriumFinder.Core.Extensions;
using NashEquilibriumFinder.Core.Helpers;
using System.Diagnostics;
using Q = Rationals.Rational;

namespace NashEquilibriumFinder.Core.Services;

public class GameTheoryService : IGameTheoryService
{
    public NashEquilibrium GetNashEquilibriumFromLemkeHowson(Game twoPersonGame)
    {
        int[,] firstPlayerPayouts = twoPersonGame.FirstPlayer.Payouts.To2dArray<int>();
        int[,] secondPlayerPayouts = twoPersonGame.SecondPlayer.Payouts.To2dArray<int>();

        (int[,], int[,]) normalizedPlayersMatrixes =
            LemkeHowson.GetNormalizedMatrixes(firstPlayerPayouts, secondPlayerPayouts);

        int[,] firstPlayerNormalizedMatrix = normalizedPlayersMatrixes.Item1;
        int[,] secondPlayerNormalizedMatrix = normalizedPlayersMatrixes.Item2;
        Q[,] tableaux = LemkeHowson.CreateTableaux(firstPlayerNormalizedMatrix, secondPlayerNormalizedMatrix);

        var nashEquilibrium = LemkeHowson
            .GetNashEquilibrium(tableaux, firstPlayerPayouts.GetLength(0))
            .ToList();

        int firstPlayersStrategiesCount = firstPlayerPayouts.GetLength(0);

        (List<Q> firstPlayerNormalizedNashEuqilibrium, List<Q> secondPlayerNormalizedNashEquilibrium) = LemkeHowson
            .GetNormalizedNashEquilibrium(nashEquilibrium, firstPlayersStrategiesCount);

        (double firstPlayersGamePayoff, double secondPlayerGamePayoff) = LemkeHowson.GetPlayersGamePayoffs(
            twoPersonGame.FirstPlayer.Payouts.To2dArray<int>(),
            twoPersonGame.SecondPlayer.Payouts.To2dArray<int>(),
            firstPlayerNormalizedNashEuqilibrium,
            secondPlayerNormalizedNashEquilibrium);

        // TODO: Add dependency injections instead of static classes
        return new NashEquilibrium
        {
            FirstPlayerStrategyProbabilities = firstPlayerNormalizedNashEuqilibrium
                .ConvertAll(x => new SimpleFraction((long)x.Numerator, (long)x.Denominator)),
            SecondPlayerStrategyProbabilities = secondPlayerNormalizedNashEquilibrium
                .ConvertAll(x => new SimpleFraction((long)x.Numerator, (long)x.Denominator)),
            FirstPlayerGamePayoff = firstPlayersGamePayoff,
            SecondPlayerGamePayoff = secondPlayerGamePayoff
        };
    }

    public int[][] GetParetoFront(int[,] firstPlayerMatrix, int[,] secondPlayerMatrix)
    {
        // TODO: Refactor this shit
        var polygonNodes = new List<PayoutCoordinate>();

        // Add unsorted points to list
        for (int i = 0; i < firstPlayerMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < firstPlayerMatrix.GetLength(1); j++)
            {
                polygonNodes.Add(new PayoutCoordinate
                {
                    FirstPlayerCleanStrategyPayoff = firstPlayerMatrix[i, j],
                    SecondPlayerCleanStrategyPayoff = secondPlayerMatrix[i, j]
                });
            }
        }

        // TODO: Use ChartDataConverter somehow
        double xReferencePoint = ChartDataConverter.GetAverageOfElements(firstPlayerMatrix);
        double yReferencePoint = ChartDataConverter.GetAverageOfElements(secondPlayerMatrix);

        List<PayoutCoordinate> paretoOptimalPayoffs = polygonNodes
            .Where(x => IsParetoOptimal((x.FirstPlayerCleanStrategyPayoff, x.SecondPlayerCleanStrategyPayoff), firstPlayerMatrix, secondPlayerMatrix))
            .ToList();

        var output = paretoOptimalPayoffs
            .OrderBy(node => Math.Atan2(
                node.SecondPlayerCleanStrategyPayoff - yReferencePoint,
                node.FirstPlayerCleanStrategyPayoff - xReferencePoint))
            .ToList();

        // TODO: How to consider equilibrium which are inside a polygon?
        var sortedPayoutsPoints = new int[output.Count][];
        for (int i = 0; i < output.Count; i++)
        {
            sortedPayoutsPoints[i] = new int[2]
            {
                output[i].FirstPlayerCleanStrategyPayoff,
                output[i].SecondPlayerCleanStrategyPayoff
            };
        }

        return sortedPayoutsPoints;
    }

    private static bool IsParetoOptimal((int, int) toCheck, int[,] firstPlayerMatrix, int[,] secondPlayerMatrix)
    {
        for (int i = 0; i < firstPlayerMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < firstPlayerMatrix.GetLength(1); j++)
            {
                if (toCheck == (firstPlayerMatrix[i, j], secondPlayerMatrix[i, j]))
                {
                    break;
                }
                if (firstPlayerMatrix[i, j] >= toCheck.Item1 && secondPlayerMatrix[i, j] >= toCheck.Item2)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public CorrelatedEquilibrium GetCorrelatedEquilibriumFromSimplex(Game twoPersonGameData)
    {
        Q[,] firstPlayerPayouts = twoPersonGameData.FirstPlayer.Payouts.To2dArray();
        Q[,] secondPlayerPayouts = twoPersonGameData.SecondPlayer.Payouts.To2dArray();
        Q[,] firstPhaseTablueax = CESimplex.CreateTablueaxForFirstPhase(firstPlayerPayouts, secondPlayerPayouts);

        // TODO: Refactor code to use Array.Initialize to initialize empty matrix Array.Initialize
        int variableToEnterBasis;
        int variableToLeaveBasis;

        while (CESimplex.TryGetVariableToEnterBasis(firstPhaseTablueax, out variableToEnterBasis))
        {
            variableToLeaveBasis = CESimplex.FindVariableToLeaveBasis(firstPhaseTablueax, variableToEnterBasis);
            CESimplex.Pivot(ref firstPhaseTablueax, variableToEnterBasis, variableToLeaveBasis);
        }

        Debug.Print("Second phase");
        // Second phase
        const int aritificialContraintsCount = 1;
        int horizontalSizeWithoutArtificialContraint = firstPhaseTablueax.GetLength(1) - aritificialContraintsCount;
        Q[] objectiveFunctionRow = CESimplex.GenerateObjectiveFunctionRow(firstPlayerPayouts, secondPlayerPayouts, horizontalSizeWithoutArtificialContraint);
        Q[,] secondPhaseTablueax = CESimplex.CreateTableauxForSecondPhase(firstPhaseTablueax, objectiveFunctionRow, aritificialContraintsCount);

        while (CESimplex.TryGetVariableToEnterBasis(secondPhaseTablueax, out variableToEnterBasis))
        {
            variableToLeaveBasis = CESimplex.FindVariableToLeaveBasis(secondPhaseTablueax, variableToEnterBasis);
            CESimplex.Pivot(ref secondPhaseTablueax, variableToEnterBasis, variableToLeaveBasis);
        }
        // Get correlated equilibrium
        // Probabily need some trying instead of just reading the solution
        int strategyProfilesCount = firstPlayerPayouts.GetLength(0) * firstPlayerPayouts.GetLength(1);
        Q[] solution = CESimplex.ReadSolution(firstPhaseTablueax, strategyProfilesCount);

        List<int> firstPlayerPayoutsTest = twoPersonGameData.FirstPlayer.Payouts
            .SelectMany(x => x)
            .Select(x => x)
            .ToList();
        List<int> secondPlayerPayoutsTest = twoPersonGameData.SecondPlayer.Payouts
            .SelectMany(x => x)
            .Select(x => x)
            .ToList();

        Q fpPayout = 0;
        Q scPayout = 0;
        for (int i = 0; i < strategyProfilesCount; i++)
        {
            fpPayout += firstPlayerPayoutsTest[i] * solution[i];
            scPayout += secondPlayerPayoutsTest[i] * solution[i];
        }

        return new()
        {
            FirstPlayerGamePayoff = Math.Round((double)fpPayout.CanonicalForm, 2),
            SecondPlayerGamePayoff = Math.Round((double)scPayout.CanonicalForm, 2),
            StrategyProfilesProbabilities = solution.Select(x => new SimpleFraction((long)x.Numerator, (long)x.Denominator)).ToArray()
        };
    }
}
