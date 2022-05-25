using NashEquilibriumFinder.Core.Extensions;
using Rationals;

namespace NashEquilibriumFinder.Core.Algorithms;

public static class LemkeHowson
{
    public static IEnumerable<Rational> GetNashEquilibrium(Rational[,] tableaux, int firstPlayerStrategiesCount)
    {
        // index = 1
        const int AdditionalColumnsNumber = 2;
        const int FirstCoefficientColumnIndex = 0;
        int minRatioRowVariable = -1;
        int nextColumnIndex = FirstCoefficientColumnIndex + AdditionalColumnsNumber;

        var nashEquilibrium = new List<Rational>();
        const int InitBasisVar = 1;

        int leftBasisVar = MakePivotStep(ref tableaux, InitBasisVar, firstPlayerStrategiesCount);

        // Do while minRatioRowIndex will be the same as the first column index
        while (Math.Abs(leftBasisVar) != InitBasisVar)
        {
            leftBasisVar = MakePivotStep(ref tableaux, -leftBasisVar, firstPlayerStrategiesCount);
        }

        var nashEquilibriumDictionary = new Dictionary<int, Rational>();

        int basisVariableIndex;
        Rational strategyProbability;

        for (int i = 1; i < tableaux.GetLength(0); i++)
        {
            basisVariableIndex = (int)tableaux[i, 0];
            strategyProbability = basisVariableIndex < 0
                ? 0
                : tableaux[i, 1];

            nashEquilibriumDictionary.Add(Math.Abs(basisVariableIndex), strategyProbability);
        }

        var variablesIndexes = nashEquilibriumDictionary.Keys.ToList();
        variablesIndexes.Sort();

        foreach (var index in variablesIndexes)
        {
            nashEquilibrium.Add(nashEquilibriumDictionary[index]);
        }

        return nashEquilibrium;
    }

    public static Rational[,] CreateTableaux(int[,] firstPlayerPayoutsMatrix, int[,] secondPlayerPayoutsMatrix)
    {
        var payoutsMatrix = new Rational[firstPlayerPayoutsMatrix.GetLength(0), secondPlayerPayoutsMatrix.GetLength(1)];
        int[,] transposedSecondPlayerPayoutsMatrix = secondPlayerPayoutsMatrix.GetTranposed();
        const int AdditionalColumnsNumber = 2; // indexes + slack variables
        int firstPlayerStrategiesCount = firstPlayerPayoutsMatrix.GetLength(0);

        int baseTablueaxDimension =
            firstPlayerPayoutsMatrix.GetLength(0) +
            secondPlayerPayoutsMatrix.GetLength(1);

        int tableauxHorizontalDimension = baseTablueaxDimension + 2;

        var tableaux = new Rational[baseTablueaxDimension + 1, tableauxHorizontalDimension];
        // Initializing with 0
        for (int i = 0; i < tableaux.GetLength(0); i++)
        {
            for (int j = 0; j < tableaux.GetLength(1); j++)
            {
                tableaux[i, j] = 0;
            }
        }

        // Adding first players strategies
        int offset = AdditionalColumnsNumber + firstPlayerStrategiesCount;
        for (int i = 0; i < firstPlayerPayoutsMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < firstPlayerPayoutsMatrix.GetLength(1); j++)
            {
                tableaux[i + 1, j + offset] = -firstPlayerPayoutsMatrix[i, j];
            }
        }

        // Adding second player startegies
        for (int i = 0; i < transposedSecondPlayerPayoutsMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < transposedSecondPlayerPayoutsMatrix.GetLength(1); j++)
            {
                tableaux[i + 1 + firstPlayerStrategiesCount, j + AdditionalColumnsNumber] = -transposedSecondPlayerPayoutsMatrix[i, j];
            }
        }

        // Adding indexes column on left
        for (int i = 1; i < tableaux.GetLength(0); i++)
        {
            tableaux[i, 0] = -i;
        }

        // Adding slack variables to table
        for (int i = 1; i < tableaux.GetLength(0); i++)
        {
            tableaux[i, 1] = 1;
        }

        // Add indexes row on the top
        for (int i = 2; i < tableaux.GetLength(1); i++)
        {
            tableaux[0, i] = i - 1;
        }

        return tableaux;
    }

    // return leave basis variable, not leave basis variable row index
    /// <summary>
    /// Gets the mininum ratio row.
    /// </summary>
    /// <param name="tableaux"></param>
    /// <param name="enterBasisVariableColumnIndex"></param>
    /// <returns>Variable that will leave the basis.</returns>
    public static int GetMinRatioRowVariable(Rational[,] tableaux, int enterBasisVariableColumnIndex)
    {
        Rational currentRatio = 0;
        Rational? minRatio = null;
        int lowestRatioRowVariable = 0;
        const int BasisColumnIndex = 1;

        // lowest ratio test
        for (int i = 1; i < tableaux.GetLength(0); i++)
        {
            if (tableaux[i, enterBasisVariableColumnIndex] < Rational.Zero)
            {
                currentRatio = tableaux[i, BasisColumnIndex] / -tableaux[i, enterBasisVariableColumnIndex];

                if (minRatio == null || currentRatio < minRatio) // problem with minus sign
                {
                    minRatio = currentRatio;
                    lowestRatioRowVariable = (int)tableaux[i, 0];
                }
            }
        }

        return lowestRatioRowVariable;
    }

    public static int MakePivotStep(ref Rational[,] tableaux, int valueToEnterBasis, int firstPlayerStratgiesCount)
    {
        // min ratio test
        Rational currentRatio = 0;
        Rational? minRatio = null;
        int lowestRatioRowVariable = 0;
        int lowestRatioRowIndex = 0;
        const int BasisColumnIndex = 1;
        int enterBasisVariableColumnIndex = 0;

        // Problem z zamianą zmiennej wchodzącej do bazy na kolumne
        // Create own method for getting this
        for (int i = 2; i < tableaux.GetLength(1); i++)
        {
            if (tableaux[0, i] == valueToEnterBasis)
            {
                enterBasisVariableColumnIndex = i;
            }
        }
        //int enterBasisVariableColumnIndex = Math.Abs(valueToEnterBasis)+ 1; // Tutaj zaimplementować to gówno


        // lowest ratio test
        // Check only rows in the appropiate part of the tableaux

        foreach (int i in GetRowsIndexes(tableaux, valueToEnterBasis, firstPlayerStratgiesCount))
        {
            if (tableaux[i, enterBasisVariableColumnIndex] < Rational.Zero)
            {
                currentRatio = tableaux[i, BasisColumnIndex] / -tableaux[i, enterBasisVariableColumnIndex];

                if (minRatio == null || currentRatio < minRatio) // problem with minus sign
                {
                    minRatio = currentRatio;
                    lowestRatioRowIndex = i;
                    lowestRatioRowVariable = (int)tableaux[i, 0];
                }
            }
        }


        // Zaimplementować to tak jak jest w pdf
        //TODO: Change variables naming
        Rational alfa = tableaux[lowestRatioRowIndex, enterBasisVariableColumnIndex];

        // Update values in a leave basis variable row
        for (int j = 1; j < tableaux.GetLength(1); j++)
        {
            tableaux[lowestRatioRowIndex, j] /= -alfa;
            tableaux[lowestRatioRowIndex, j] = tableaux[lowestRatioRowIndex, j].CanonicalForm;
        }

        tableaux[lowestRatioRowIndex, enterBasisVariableColumnIndex] = (1 / alfa).CanonicalForm;

        // Get only appropiate rows
        IEnumerable<int> stragiesRowIndexesToUpdate = GetRowsIndexesToUpdate(tableaux, firstPlayerStratgiesCount, lowestRatioRowIndex);

        foreach (int strategyRowIndex in stragiesRowIndexesToUpdate)
        {
            if (strategyRowIndex != lowestRatioRowIndex)
            {
                // ZOPTYMALIZOWAĆ
                Rational beta = tableaux[strategyRowIndex, enterBasisVariableColumnIndex];
                tableaux[strategyRowIndex, enterBasisVariableColumnIndex] = 0;

                // jazda po obecnym wierszu
                for (int j = 1; j < tableaux.GetLength(1); j++)
                {
                    tableaux[strategyRowIndex, j] += beta * tableaux[lowestRatioRowIndex, j];
                    tableaux[strategyRowIndex, j] = tableaux[strategyRowIndex, j].CanonicalForm;
                }
            }
        }

        // wymiana indeksu
        tableaux[0, enterBasisVariableColumnIndex] = lowestRatioRowVariable;
        tableaux[lowestRatioRowIndex, 0] = valueToEnterBasis;

        return lowestRatioRowVariable;
    }

    public static (double firstPlayersGamePayoff, double secondPlayerGamePayoff) GetPlayersGamePayoffs(
        int[,] firstPlayerPayoutsMatrix,
        int[,] secondPlayerPayoutsMatrix,
        List<Rational> firstPlayerStrategyProbabilities,
        List<Rational> secondPlayerStrategyProbabilities)
    {
        double firstPlayerGamePayoff = 0;
        double secondPlayerGamePayoff = 0;
        double test = 0;

        // First player payoff
        for (int i = 0; i < firstPlayerPayoutsMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < firstPlayerPayoutsMatrix.GetLength(1); j++)
            {
                test += firstPlayerPayoutsMatrix[i, j] * (double)secondPlayerStrategyProbabilities[j];
            }
            firstPlayerGamePayoff += test * (double)firstPlayerStrategyProbabilities[i];
            test = 0;
        }

        // Second player payoff
        for (int j = 0; j < secondPlayerPayoutsMatrix.GetLength(1); j++)
        {
            for (int i = 0; i < secondPlayerPayoutsMatrix.GetLength(0); i++)
            {
                test += secondPlayerPayoutsMatrix[i, j] * (double)firstPlayerStrategyProbabilities[i];
            }
            secondPlayerGamePayoff += test * (double)secondPlayerStrategyProbabilities[j];
            test = 0;
        }

        return (Math.Round(firstPlayerGamePayoff, 2), Math.Round(secondPlayerGamePayoff, 2));


    }

    private static IEnumerable<int> GetRowsIndexes(Rational[,] tableaux, int enterBasisVariable, int firstPlayerStrategiesCount)
    {
        int secondPlayerStrategiesCount = tableaux.GetLength(0) - firstPlayerStrategiesCount - 1;

        if (-firstPlayerStrategiesCount <= enterBasisVariable && enterBasisVariable < 0 || enterBasisVariable > firstPlayerStrategiesCount)
        {
            return Enumerable.Range(1, firstPlayerStrategiesCount);
        }

        return Enumerable.Range(firstPlayerStrategiesCount + 1, secondPlayerStrategiesCount);
    }
    private static IEnumerable<int> GetRowsIndexesToUpdate(Rational[,] tableaux, int firstPlayerStratgiesCount, int strategyRowToLeaveBasisIndex)
    {
        int secondPlayerStrategiesCount = tableaux.GetLength(0) - firstPlayerStratgiesCount - 1;

        if (strategyRowToLeaveBasisIndex <= firstPlayerStratgiesCount)
        {
            return Enumerable.Range(1, firstPlayerStratgiesCount);
        }

        return Enumerable.Range(firstPlayerStratgiesCount + 1, secondPlayerStrategiesCount);
    }

    public static (int[,] FirstPlayerMatrix, int[,] SecondPlayerMatrix) GetNormalizedMatrixes(int[,] firstPlayerPayoutsMatrix, int[,] secondPlayerPayoutsMatrix)
    {
        // no negative numbers in matrixes
        int lowestValue = firstPlayerPayoutsMatrix[0, 0];
        var matrixes = new List<int[,]>
            {
                firstPlayerPayoutsMatrix,
                secondPlayerPayoutsMatrix
            };

        // Check for the lowest value
        // TODO: Consider to refactor (local function or some method)
        // Becasue of the indentation violation
        foreach (var matrix in matrixes)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] < lowestValue)
                    {
                        lowestValue = matrix[i, j];
                    }
                }
            }
        }

        // TODO: Check out code calistenistic/ violating indenation rule
        if (lowestValue <= 0)
        {
            int equalizationValue = -lowestValue + 1;

            foreach (var matrix in matrixes)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        matrix[i, j] += equalizationValue;
                    }
                }
            }
        }

        // no row with all zeros
        foreach (var matrix in matrixes)
        {
            int rowZerosCount = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        rowZerosCount++;
                    }
                }

                if (rowZerosCount == matrix.GetLength(1))
                {
                    throw new ArgumentException("Neither of the rows can't have only zero values.");
                }
                rowZerosCount = 0;
            }
        }

        // no column with all zeros
        foreach (var matrix in matrixes)
        {
            int columnZerosCount = 0;

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    if (matrix[i, j] == 0)
                    {
                        columnZerosCount++;
                    }
                }

                if (columnZerosCount == matrix.GetLength(0))
                {
                    throw new ArgumentException("Neither of the columns can't have only zero values.");
                }
                columnZerosCount = 0;
            }
        }

        return (matrixes[0], matrixes[1]);
    }

    public static (List<Rational> FirstPlayer, List<Rational> SecondPlayer) GetNormalizedNashEquilibrium(
        IList<Rational> nashEquilibrium,
        int firstPlayerStrategiesCount)
    {
        List<Rational> firstPlayerStrategiesProbabilities;
        List<Rational> secondPlayerStrategiesProbabilities;
        int strategiesCount = nashEquilibrium.Count;
        int secondPlayerStrategiesCount = strategiesCount - firstPlayerStrategiesCount;

        firstPlayerStrategiesProbabilities = nashEquilibrium
            .Take(firstPlayerStrategiesCount)
            .ToList();

        secondPlayerStrategiesProbabilities = nashEquilibrium
            .TakeLast(secondPlayerStrategiesCount)
            .ToList();

        IEnumerable<Rational> firstPlayerNormalizedNashEquilibrium =
            GetNormalizedEquilibriumForPlayer(firstPlayerStrategiesProbabilities);

        IEnumerable<Rational> secondPlayerNormalizedNashEquilibrium =
            GetNormalizedEquilibriumForPlayer(secondPlayerStrategiesProbabilities);

        return (firstPlayerNormalizedNashEquilibrium.ToList(), secondPlayerNormalizedNashEquilibrium.ToList());
    }

    private static IEnumerable<Rational> GetNormalizedEquilibriumForPlayer(List<Rational> playerStrategiesProbabilities)
    {
        Rational probabilitySum = playerStrategiesProbabilities
            .Sum()
            .CanonicalForm;

        var sum = Rational.Pow(probabilitySum, -1);

        foreach (Rational strategyProbability in playerStrategiesProbabilities)
        {
            yield return (strategyProbability * sum).CanonicalForm;
        }
    }
}