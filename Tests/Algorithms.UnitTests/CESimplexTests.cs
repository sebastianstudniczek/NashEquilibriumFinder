using NashEquilibriumFinder.Core.Algorithms;
using Xunit;
using Q = Rationals.Rational;

namespace NashEquilibriumFinder.Algorithms.UnitTests;

public class CESimplexTests
{
    [Theory]
    [ClassData(typeof(ContraintsTestData))]
    public void GenerateContraints_ShouldReturnValidConstraintsForPlayer(int playerNumber, Q[,] playerPayouts, IEnumerable<Q[]> expectedContraints)
    {
        // Already converted from greather than to less than.
        IEnumerable<Q[]> actualPlyerContraints = CESimplex.GenerateConstraintsForPlayer(playerPayouts, playerNumber);

        Assert.Equal(expectedContraints, actualPlyerContraints);
    }

    [Fact]
    public void CreateTableauxForFirstPhase_ShouldCreateValidTableaux()
    {
        const int expectedVerticalSize = 15;
        const int expectedHorizontalSize = 25;
        var firstPlayerPayouts = new Q[,]
        {
            { 0, 0, 2 }, // A
            { 0, 2, 1 }, // B
            { 2, 1, 1 }  // C
        };
        var secondPlayerPayouts = new Q[,]
        {
            //D  E  F
            {-1, 2, 3 },
            { 0, 1,-1 },
            { 2, 4,-1 }
        };
        var expectedTableaux = new Q[expectedVerticalSize, expectedHorizontalSize]
        {
            //z p1 p2 p3 p4 p5 p6 p7 p8 p9 s1 s2 s3 s4 s5 s6 s7 s8 s9s10s11s12 a1 Cj Base
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18,19,20,21,22, 0, 0 }, //1 indexes
            { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1, 0 }, //2 objective function
            { 0, 0, 2,-1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,10 }, //3 s1
            { 0, 2, 1,-1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,11 }, //4 s2
            { 0, 0, 0, 0, 0,-2, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,12 }, //5 s3
            { 0, 0, 0, 0, 2,-1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,13 }, //6 s4
            { 0, 0, 0, 0, 0, 0, 0,-2,-1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,14 }, //7 s5
            { 0, 0, 0, 0, 0, 0, 0,-2, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,15 }, //8 s6
            { 0, 3, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,16 }, //9 s7
            { 0, 4, 0, 0,-1, 0, 0,-3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,17 }, //10 s8
            { 0, 0,-3, 0, 0,-1, 0, 0,-2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,18 }, //11 s9
            { 0, 0, 1, 0, 0,-2, 0, 0,-5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,19 }, //12 s10
            { 0, 0, 0,-4, 0, 0, 1, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,20 }, //13 s11
            { 0, 0, 0,-1, 0, 0, 2, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,21 }, //14 s12
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,22 }, //15 a1
        };

        Q[,] actualTableaux = CESimplex.CreateTablueaxForFirstPhase(firstPlayerPayouts, secondPlayerPayouts);

        Assert.Equal(expectedHorizontalSize, actualTableaux.GetLength(1));
        Assert.Equal(expectedVerticalSize, actualTableaux.GetLength(0));
        Assert.Equal(expectedTableaux, actualTableaux);
    }

    [Fact]
    public void GetVariableIndexForEnteringBase_ShouldReturnTrueAndVariableIndexWithSmallestValue()
    {
        const int expectedIndex = 6; // Variable with -3 value
        var tableaux = new Q[,]
        {
            { 0, 1, 2, 3, 4, 5, 0, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 4,-1,-3, 2, 0, 0,-1, 0 },
            { 0, 2, 1, 1, 0, 0,20, 3, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 1, 0,18, 4, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0, 1, 8, 5, 0, 0, 0, 0 }
        };

        bool wasFound = CESimplex.TryGetVariableToEnterBasis(tableaux, out int actualIndex);

        Assert.True(wasFound);
        Assert.Equal(expectedIndex, actualIndex);
    }

    [Fact]
    public void FindVariableToEnterBasis_ShouldReturnFalse_IfThereAreNoNegativeVariables()
    {
        var tableaux = new Q[,]
        {
            { 0, 1, 2, 3, 4, 5, 0, 0, 0 },
            { 1, 0, 0, 2, 4, 5, 6, 7, 0 },
            { 0, 2, 1, 1, 0, 0,20, 3, 0 },
            { 0, 1, 1, 0, 1, 0,18, 4, 0 },
            { 0, 1, 0, 0, 0, 1, 8, 5, 0 }
        };

        bool wasFound = CESimplex.TryGetVariableToEnterBasis(tableaux, out _);

        Assert.False(wasFound);
    }

    [Fact]
    public void FindVariableToEnterBasis_ShouldReturnFalse_IfCjValueIsTheOnlyNegativeInRow()
    {
        var tableaux = new Q[,]
        {
            { 0, 1, 2, 3, 4, 5, 0, 0, 0 },
            { 1, 0, 0, 2, 4, 5, 6,-2, 0 },
            { 0, 2, 1, 1, 0, 0,20, 3, 0 },
            { 0, 1, 1, 0, 1, 0,18, 4, 0 },
            { 0, 1, 0, 0, 0, 1, 8, 5, 0 }
        };

        bool wasFound = CESimplex.TryGetVariableToEnterBasis(tableaux, out _);

        Assert.False(wasFound);
    }

    [Fact]
    public void FindVariableToLeaveBasis_ShouldReturnValidRow_AndIgnoreNonPositiveValuesInEnterVariableColumn()
    {
        const int expectedRowIndex = 4;
        const int varToEnterBasis = 1;
        var tableaux = new Q[,]
        {
            { 0, 1, 2, 3, 4, 5, 0, 0, 0 },
            { 1,-7, 0, 2, 4, 5, 6, 0, 0 },
            { 0,-2, 1, 1, 0, 0,20,20, 0 },
            { 0, 1, 1, 0, 1, 0,18,18, 0 },
            { 0, 1, 0, 0, 0, 1, 8, 8, 0 }
        };

        int leaveVarRowIndex = CESimplex.FindVariableToLeaveBasis(tableaux, varToEnterBasis);

        Assert.Equal(expectedRowIndex, leaveVarRowIndex);
    }

    [Fact]
    public void Pivot_ShouldCorrectlyTransformTablueax()
    {
        var initialTablueax = new Q[,]
        {
            { 0, 1, 2, 3, 4, 5, 0, 0 },
            { 1,-7,-4, 0, 0, 0, 0, 0 },
            { 0, 2, 1, 1, 0, 0,20, 3 },
            { 0, 1, 1, 0, 1, 0,18, 4 },
            { 0, 1, 0, 0, 0, 1, 8, 5 }
        };

        var expectedTableauxAfterPivot = new Q[,]
        {
            { 0, 1, 2, 3, 4, 5, 0, 0 },
            { 1, 0,-4, 0, 0, 7,56, 0 },
            { 0, 0, 1, 1, 0,-2, 4, 3 },
            { 0, 0, 1, 0, 1,-1,10, 4 },
            { 0, 1, 0, 0, 0, 1, 8, 1 }
        };
        const int varToEnterBasisColumnIndex = 1;
        const int varToLeaveBasisRowIndex = 4;

        CESimplex.Pivot(ref initialTablueax, varToEnterBasisColumnIndex, varToLeaveBasisRowIndex);

        Assert.Equal(expectedTableauxAfterPivot, initialTablueax);
    }

    [Fact]
    public void GetResult_ShouldReadResultFromTablueax()
    {
        const int strategyProfilesCount = 4;
        var finishedTablueax = new Q[,]
        {
            { 0,  1,  2,  3,     4,     5,  6,     7,  8,     0,  0 },
            { 1,  0,  0,  0,  20/3,   2/3,  0,   2/3,  0,  20/3,  0 },
            { 0,  1,  0,  0,   1/3,   1/3,  0,   1/3,  0,   1/3,  1 },
            { 0,  0,  0,  0,   4/3,   1/3,  1,  -2/3,  0,   1/3,  6 },
            { 0,  0,  1,  0,   1/3,  -2/3,  0,   1/3,  0,   1/3,  2 },
            { 0,  0,  0,  0,   4/3,  -2/3,  0,   1/3,  1,   1/3,  8 },
            { 0,  0,  0,  1,   1/3,   1/3,  0,  -2/3,  0,   1/3,  3 }
        };
        var expectedSolution = new Q[] { 1 / 3, 1 / 3, 1 / 3, 0 };

        Q[] solution = CESimplex.ReadSolution(finishedTablueax, strategyProfilesCount);

        Assert.Equal(expectedSolution.Length, solution.Length);
        Assert.Equal(expectedSolution, solution);
    }

    [Fact]
    public void CreateTableauxForSecondPhase_ShouldCreateValidTableaux()
    {
        const int artificialContraintsCount = 2;
        var originalObjectiveFunction = new Q[] { 1, -6, -1 };
        var tableauxAfterFirstPhaseTablueax = new Q[,]
        {
            { 0, 1, 2, 3, 4, 5, 6, 0, 0 }, // Index row
            { 1, 0, 0, 0, 0, 1, 1, 0, 0 }, // Objective function row
            { 0, 0, 0, 1, 0 ,1, 0,12, 3 },
            { 0, 0,-4, 0, 1, 1,-1, 5, 4 },
            { 0, 1,-3, 0, 0, 1, 0, 6, 1 }
        };
        var expectedTablueax = new Q[,]
        {
            { 0, 1,  2, 3, 4, 0, 0 }, // Index row
            { 1, 0,-19, 0, 0,36, 0 }, // Objective function row
            { 0, 0,  0, 1, 0,12, 3 },
            { 0, 0, -4, 0, 1, 5, 4 },
            { 0, 1, -3, 0, 0, 6, 1 }
        };

        Q[,] actualTablueax = CESimplex.CreateTableauxForSecondPhase(tableauxAfterFirstPhaseTablueax, originalObjectiveFunction, artificialContraintsCount);

        Assert.Equal(expectedTablueax, actualTablueax);
    }

    [Fact]
    public void GetObjectiveFunctionRow_SholdReturnRowWithCorrectValues()
    {
        const int tableauxHorizontalSizeWithoutArtificialContraints = 11;
        var expectedObjectiveFunctionRow = new Q[]
        {
            1, -8, -6, -6, 0, 0, 0, 0, 0, 0, 0
        };
        var firstPlayerPayouts = new Q[,]
        {
            { 4, 1 },
            { 5, 0 }
        };
        var secondPlayerPayouts = new Q[,]
        {
            { 4, 5 },
            { 1, 0 }
        };

        Q[] objectiveFunctionRow = CESimplex.GenerateObjectiveFunctionRow(firstPlayerPayouts, secondPlayerPayouts, tableauxHorizontalSizeWithoutArtificialContraints);

        Assert.Equal(expectedObjectiveFunctionRow, objectiveFunctionRow);
    }
}