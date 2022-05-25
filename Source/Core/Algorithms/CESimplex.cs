using NashEquilibriumFinder.Core.Extensions;
using System.Diagnostics;
using Q = Rationals.Rational;

namespace NashEquilibriumFinder.Core.Algorithms;
public static class CESimplex
{
    // Variable indexes row, objective function row
    private const int _additionalRowsCount = 2;
    // Base column, z column, Cj column
    private const int _additionalColumnsCount = 3;
    public static Q[,] CreateTablueaxForFirstPhase(Q[,] firstPlayerPayouts, Q[,] secondPlayerPayouts)
    {
        // Base column, z column, Cj column
        const int additionalColumnsCount = 3;
        int firstPlayerStrategiesCount = firstPlayerPayouts.GetLength(0);
        int secondPlayerStrategiesCount = secondPlayerPayouts.GetLength(1);
        int strategyProfilesCount = firstPlayerStrategiesCount * secondPlayerStrategiesCount;
        // p1+p2...px + a1 = 1
        const int artificialProbsContraintsCount = 1;
        // Becasue there are (strategiesCount - 1) strategies which player can deviate to
        int inequalityConstraintsCount =
            (firstPlayerStrategiesCount * (firstPlayerStrategiesCount - 1)) +
            (secondPlayerStrategiesCount * (secondPlayerStrategiesCount - 1));
        int horizontalSize =
            additionalColumnsCount +
            strategyProfilesCount +
            inequalityConstraintsCount +
            artificialProbsContraintsCount;

        // variables indexes row + objective function row
        const int additionalRowsCount = 2;
        int verticalSize =
            additionalRowsCount +
            inequalityConstraintsCount +
            artificialProbsContraintsCount;

        // Initialize empty tableaux
        Q[,] emptyTableaux = CreateEmptyTableaux(horizontalSize, verticalSize);
        var firstPlayerconstraints = GenerateConstraintsForPlayer(firstPlayerPayouts, 1).ToList();
        var secondPlayerContraints = GenerateConstraintsForPlayer(secondPlayerPayouts, 2).ToList();
        // generate artificial variable contraint
        Q[] artificialVarConstraint = GenerateArtificialVarConstraint(strategyProfilesCount, inequalityConstraintsCount);
        var mergedContraints = firstPlayerconstraints.Concat(secondPlayerContraints).ToList();
        //mergedContraints.Add(artificialVarConstraint);

        // Populate
        return GetPopulatedTableaux(emptyTableaux, mergedContraints, strategyProfilesCount);
    }

    private static Q[,] GetPopulatedTableaux(Q[,] tableauxToInitialize, List<Q[]> contraints, int strategyProfilesCount)
    {
        // Initialize index row
        const int indexRow = 0;
        const int additionalColumnsCount = 2;
        int verticalSizeWithoutCjAndBase = tableauxToInitialize.GetLength(1) - additionalColumnsCount;

        DebugExtensions.Print2D(tableauxToInitialize);
        PopulateIndexesRow(tableauxToInitialize, indexRow, verticalSizeWithoutCjAndBase);
        Debug.Print(nameof(PopulateIndexesRow));
        DebugExtensions.Print2D(tableauxToInitialize);
        Debug.Print("");

        PopulateObjectiveFunctionRow(tableauxToInitialize, verticalSizeWithoutCjAndBase);
        Debug.Print(nameof(PopulateObjectiveFunctionRow));
        DebugExtensions.Print2D(tableauxToInitialize);
        Debug.Print("");

        PopulateBaseColumn(tableauxToInitialize, strategyProfilesCount);
        Debug.Print(nameof(PopulateBaseColumn));
        DebugExtensions.Print2D(tableauxToInitialize);
        Debug.Print("");

        PopulateContraintRows(tableauxToInitialize, contraints);
        Debug.Print(nameof(PopulateContraintRows));
        DebugExtensions.Print2D(tableauxToInitialize);
        Debug.Print("");

        SetSurplusVariablesOnCross(tableauxToInitialize, contraints, strategyProfilesCount);
        Debug.Print(nameof(SetSurplusVariablesOnCross));
        DebugExtensions.Print2D(tableauxToInitialize);
        Debug.Print("");

        PopulateProbabilitySumConstraintRow(tableauxToInitialize, additionalColumnsCount, strategyProfilesCount);
        Debug.Print(nameof(PopulateProbabilitySumConstraintRow));
        DebugExtensions.Print2D(tableauxToInitialize);
        Debug.Print("");

        // Add artificial column * (-1) to objective function to get rid of 1 in object function for a1
        int artificialVarContraintRowIndex = tableauxToInitialize.GetLength(0) - 1;
        int horizontalSize = tableauxToInitialize.GetLength(1);
        var negativeRow = new Q[horizontalSize];
        for (int i = 0; i < horizontalSize - 1; i++)
        {
            negativeRow[i] = tableauxToInitialize[artificialVarContraintRowIndex, i] * -1;
        }
        // add to objective function row
        const int objectiveFunctionRowIndex = 1;
        for (int i = 0; i < horizontalSize - 1; i++)
        {
            tableauxToInitialize[objectiveFunctionRowIndex, i] += negativeRow[i];
        }
        Debug.Print("get rid of 1 in a1");
        DebugExtensions.Print2D(tableauxToInitialize);
        Debug.Print("");

        return tableauxToInitialize;


        static void PopulateProbabilitySumConstraintRow(Q[,] tableauxToInitialize, int additionalColumnsCount, int strategyProfilesCount)
        {
            int artificialConstraintRowIndex = tableauxToInitialize.GetLength(0) - 1;
            int artificialContraintColumnIndex = tableauxToInitialize.GetLength(1) - additionalColumnsCount - 1;
            for (int i = 1; i <= strategyProfilesCount; i++)
            {
                tableauxToInitialize[artificialConstraintRowIndex, i] = 1;
            }

            int cjColumnIndex = tableauxToInitialize.GetCjColumnIndex();
            tableauxToInitialize[artificialConstraintRowIndex, artificialContraintColumnIndex] = 1;
            tableauxToInitialize[artificialConstraintRowIndex, cjColumnIndex] = 1;
        }

        static void SetSurplusVariablesOnCross(Q[,] tableauxToInitialize, List<Q[]> contraints, int strategyProfilesCount)
        {
            int startColumnIndex = strategyProfilesCount + 1;
            for (int i = 0; i < contraints.Count; i++)
            {
                tableauxToInitialize[_additionalRowsCount + i, startColumnIndex + i] = 1;
            }
        }

        static void PopulateContraintRows(Q[,] tableauxToInitialize, List<Q[]> contraints)
        {
            for (int i = 0; i < contraints.Count; i++)
            {
                for (int j = 0; j < contraints[i].Length; j++)
                {
                    tableauxToInitialize[_additionalRowsCount + i, 1 + j] = contraints[i][j];
                }
            }
        }

        static void PopulateBaseColumn(Q[,] tableauxToInitialize, int strategyProfilesCount)
        {
            int startingIndex = strategyProfilesCount + 1;
            int baseColumnIndex = tableauxToInitialize.GetLength(1) - 1;
            for (int i = 0; i < tableauxToInitialize.GetLength(0) - _additionalRowsCount; i++)
            {
                tableauxToInitialize[_additionalRowsCount + i, baseColumnIndex] = startingIndex++;
            }
        }

        static void PopulateObjectiveFunctionRow(Q[,] tableauxToInitialize, int verticalSizeWithoutCjAndBase)
        {
            int artificialVarColumnIndex = verticalSizeWithoutCjAndBase - 1;
            tableauxToInitialize[1, 0] = 1;
            tableauxToInitialize[1, artificialVarColumnIndex] = 1;
        }

        static void PopulateIndexesRow(Q[,] tableauxToInitialize, int indexRow, int verticalSizeWithoutCjAndBase)
        {
            for (int i = 0; i < verticalSizeWithoutCjAndBase; i++)
            {
                tableauxToInitialize[indexRow, i] = i;
            }
        }
    }

    /// <summary>
    /// Get the variable which is going to enter the basis.
    /// Basically variable is the same as index.
    /// </summary>
    /// <param name="variableToEnterBasisIndex"></param>
    /// <returns>Boolean value that indicates whether such variable was found or not.</returns>
    public static bool TryGetVariableToEnterBasis(Q[,] tableaux, out int variableToEnterBasisIndex) {
        Q[] objectiveFunctionRow = GetObjectiveFunctionRow(tableaux);
        Dictionary<int, Q> varValueByIndex = new();
        for (int i = 0; i < objectiveFunctionRow.Length - _additionalColumnsCount; i++) {
            if (objectiveFunctionRow[i] < 0) {
                varValueByIndex.Add(i, objectiveFunctionRow[i]);
            }
        }

        if (varValueByIndex.Count == 0) {
            variableToEnterBasisIndex = -1;
            return false;
        }

        variableToEnterBasisIndex = varValueByIndex.MinBy(x => x.Value).Key;
        return true;


        static Q[] GetObjectiveFunctionRow(Q[,] tableaux) {
            var objectiveFunctionRow = new Q[tableaux.GetLength(1)];
            const int objectiveFunctionRowIndex = 1;
            for (int j = 0; j < tableaux.GetLength(1); j++) {
                objectiveFunctionRow[j] = tableaux[objectiveFunctionRowIndex, j];
            }

            return objectiveFunctionRow;
        }
    }

    private static Q[,] CreateEmptyTableaux(int horizontalSize, int verticalSize)
    {
        List<List<Q>> tableaux = new();
        for (int i = 0; i < verticalSize; i++)
        {
            List<Q> row = new();
            for (int j = 0; j < horizontalSize; j++)
            {
                row.Add(0);
            }
            tableaux.Add(row);
        }

        return tableaux
            .Select(x => x.ToArray())
            .ToArray()
            .To2dArray();
    }

    public static void Pivot(ref Q[,] initialTablueax, int varToEnterBasisColumnIndex, int varToLeaveBasisRowIndex)
    {
        // Put variable index in base
        int baseColumnIndex = initialTablueax.GetLength(1) - 1;
        const int variablesRowIndex = 0;
        Q variableNumber = initialTablueax[variablesRowIndex, varToEnterBasisColumnIndex];
        initialTablueax[varToLeaveBasisRowIndex, baseColumnIndex] = variableNumber;

        int pivotRowIndex = varToLeaveBasisRowIndex;
        // Initialize pivotrow column
        Q crossValue = initialTablueax[varToLeaveBasisRowIndex, varToEnterBasisColumnIndex];
        for (int j = 0; j < initialTablueax.GetLength(1) - 1; j++) {
            initialTablueax[varToLeaveBasisRowIndex, j] /= crossValue;
        }
        Debug.Print("Initialize pivot row column");
        DebugExtensions.Print2D(initialTablueax);

        // Time how many i need to multiply another column
        for (int i = 1; i < initialTablueax.GetLength(0); i++)
        {
            // Get compensation value
            if (i != varToLeaveBasisRowIndex)
            {
                Q compensationValue = -initialTablueax[i, varToEnterBasisColumnIndex];
                // Add to each column
                for (int j = 0; j < initialTablueax.GetLength(1) - 1; j++)
                {
                    initialTablueax[i, j] += initialTablueax[pivotRowIndex, j] * compensationValue;
                }
            }
        }
        Debug.Print(nameof(Pivot));
        DebugExtensions.Print2D(initialTablueax);
    }

    public static Q[] ReadSolution(Q[,] finishedTableaux, int strategyProfilesCount)
    {
        // z / cj / base
        const int additionalColumns = 3;
        var variablesToGet = Enumerable.Range(1, strategyProfilesCount).ToList();
        int allVariablesCount = finishedTableaux.GetLength(1) - additionalColumns;
        var result = new Q[strategyProfilesCount];
        result.InitializeWithZero();

        var valueByIndex = new Dictionary<int, Q>();
        // two additionalRows
        const int startingRowIndex = 2;
        int baseColumnIndex = finishedTableaux.GetLength(1) - 1;
        int cjColumnIndex = finishedTableaux.GetLength(1) - 2;

        // Get all values
        for (int i = startingRowIndex; i < finishedTableaux.GetLength(0); i++)
        {
            valueByIndex.Add((int)finishedTableaux[i, baseColumnIndex], finishedTableaux[i, cjColumnIndex]);
        }

        // Put
        foreach (KeyValuePair<int, Q> item in valueByIndex.Where(x => variablesToGet.Contains(x.Key)))
        {
            result[item.Key - 1] = item.Value;
        }

        return result;
    }

    public static Q[,] CreateTableauxForSecondPhase(Q[,] tableauxAfterFirstPhaseTablueax, Q[] originalObjectiveFunction, int artificialContraintsCount)
    {
        // delete artificial contraint columns -> construct new table
        int horizontalSize = tableauxAfterFirstPhaseTablueax.GetLength(1) - artificialContraintsCount;
        int verticalSize = tableauxAfterFirstPhaseTablueax.GetLength(0);

        Q[,] tableaux = new Q[verticalSize, horizontalSize];
        // Add valid columns
        const int additionalColumnsCount = 2;
        int variablesCount = horizontalSize - additionalColumnsCount;
        for (int i = 0; i < verticalSize; i++)
        {
            for (int j = 0; j < variablesCount; j++)
            {
                tableaux[i, j] = tableauxAfterFirstPhaseTablueax[i, j];
            }
        }
        Debug.Print("Add valid columns");
        DebugExtensions.Print2D(tableaux);

        // Add Cj and base column
        int newBaseColumnIndex = tableaux.GetBaseColumnIndex();
        int newCjColumnIndex = tableaux.GetCjColumnIndex();
        int baseColumnIndex = tableauxAfterFirstPhaseTablueax.GetBaseColumnIndex();
        int cjColumnIndex = tableauxAfterFirstPhaseTablueax.GetCjColumnIndex();

        for (int i = 0; i < tableauxAfterFirstPhaseTablueax.GetLength(0); i++)
        {
            tableaux[i, newBaseColumnIndex] = tableauxAfterFirstPhaseTablueax[i, baseColumnIndex];
            tableaux[i, newCjColumnIndex] = tableauxAfterFirstPhaseTablueax[i, cjColumnIndex];
        }

        Debug.Print("Add Cj and base column");
        DebugExtensions.Print2D(tableaux);
        // Populate objective function row
        const int objectiveFunctionRowIndex = 1;
        for (int j = 0; j < originalObjectiveFunction.Length; j++)
        {
            tableaux[objectiveFunctionRowIndex, j] = originalObjectiveFunction[j];
        }
        Debug.Print("Populate objective function row");
        DebugExtensions.Print2D(tableaux);

        // Check for columns that needs to be zeroed (create method for it)
        // Get base variables
        Dictionary<int, int> rowIndexByBaseVariables = new();
        for (int i = additionalColumnsCount; i < tableaux.GetLength(0); i++)
        {
            rowIndexByBaseVariables.Add((int)tableaux[i, newBaseColumnIndex], i);
        }

        // Check which column needs to be zeroed and zero it
        foreach (var item in rowIndexByBaseVariables)
        {
            Q valueToCheck = tableaux[objectiveFunctionRowIndex, item.Key];
            if (valueToCheck != 0)
            {
                Q compensationValue = - valueToCheck;

                // Compensating objective function row
                for (int j = 0; j < horizontalSize - 1; j++)
                {
                    tableaux[objectiveFunctionRowIndex, j] +=
                        compensationValue * tableaux[item.Value, j];
                }
            }
        }

        Debug.Print("Zeroing objective function row columns");
        DebugExtensions.Print2D(tableaux);

        return tableaux;
    }

    private static Q[] GenerateArtificialVarConstraint(int strategyProfilesCount, int contraintsCount)
    {
        // p1 + p2 + p3 + p4 .... + pn = 1
        List<Q> artificialVarConstraint = new();
        foreach (int item in Enumerable.Range(0, strategyProfilesCount))
        {
            artificialVarConstraint.Add(1);
        }
        foreach (int item in Enumerable.Range(0, contraintsCount))
        {
            artificialVarConstraint.Add(0);
        }

        // Add artificial variable coeficcient
        artificialVarConstraint.Add(1);
        // Add rhs
        artificialVarConstraint.Add(1);

        return artificialVarConstraint.ToArray();
    }

    /// <summary>
    /// Convert greater-than inequalities to less-than and generates contraints related to specific player.
    /// </summary>
    /// <param name="payouts"></param>
    /// <param name="playerNumber"></param>
    /// <exception cref="ArgumentException"></exception>
    public static IEnumerable<Q[]> GenerateConstraintsForPlayer(Q[,] payouts, int playerNumber)
    {
        int playerStrategiesCount = 0;
        int otherPlayerStrategiesCount = 0;
        if (playerNumber == 1)
        {
            playerStrategiesCount = payouts.GetLength(0);
            otherPlayerStrategiesCount = payouts.GetLength(1);
        }
        else if (playerNumber == 2)
        {
            playerStrategiesCount = payouts.GetLength(1);
            otherPlayerStrategiesCount = payouts.GetLength(0);
        }
        else
        {
            throw new ArgumentException($"{playerNumber} is a wrong player number.");
        }
        // 2 because comparing 2 strategies to each other
        int strategyCombinationsCount = playerStrategiesCount * 2;
        List<Q[]> constraints = new();
        int strategyProfilesCount = payouts.GetLength(0) * payouts.GetLength(1);
        IEnumerable<int> strategies = Enumerable.Range(0, playerStrategiesCount);
        foreach (int fromStrategy in strategies)
        {
            foreach (int toStrategy in strategies.Where(strategy => strategy != fromStrategy))
            {
                constraints.Add(GenerateConstraint(fromStrategy, toStrategy, payouts, playerNumber, otherPlayerStrategiesCount, strategyProfilesCount));
            }
        }

        return constraints;
    }


    private static Q[] GenerateConstraint(int fromStrategy, int toStrategy, Q[,] playersPayouts, int playerNumber, int otherPlayerStrategiesCount, int strategyProfilesCount)
    {
        var constraint = new Q[strategyProfilesCount];
        for (int i = 0; i < strategyProfilesCount; i++)
        {
            constraint[i] = 0;
        }
        int startingIndex = 0;
        // Multiplies by (-1) to convert greather-than inequalities to less-than inequalities.
        if (playerNumber == 1)
        {
            startingIndex = fromStrategy * otherPlayerStrategiesCount;
            for (int i = 0; i < otherPlayerStrategiesCount; i++)
            {
                constraint[i + startingIndex] = (playersPayouts[fromStrategy, i] - playersPayouts[toStrategy, i]) * -1;
            }
        }
        else if (playerNumber == 2)
        {
            for (int i = 0; i < otherPlayerStrategiesCount; i++)
            {
                //int index = (i * otherPlayerStrategiesCount) + fromStrategy;
                int playerStrategyCount = playersPayouts.GetLength(1);
                int index = (i * playerStrategyCount) + fromStrategy;
                constraint[index] = (playersPayouts[i, fromStrategy] - playersPayouts[i, toStrategy]) * -1;
            }
        }

        return constraint;
    }

    public static int FindVariableToLeaveBasis(Q[,] tableaux, int enterVariable) {
        // Initialize cjColumn and enter variable column
        var cjColumn = new Q[tableaux.GetLength(0)];
        var enterVariableColumn = new Q[tableaux.GetLength(0)];
        int cjColumnIndex = tableaux.GetCjColumnIndex();
        for (int i = 0; i < tableaux.GetLength(0); i++) {
            cjColumn[i] = tableaux[i, cjColumnIndex];
            enterVariableColumn[i] = tableaux[i, enterVariable];
        }

        // Q is initialized to NaN
        var ratioTestColumn = new Q[enterVariableColumn.Length];

        // Needs to be greater than
        for (int i = _additionalRowsCount; i < ratioTestColumn.Length; i++) {
            if (enterVariableColumn[i] <= 0)
            {
                ratioTestColumn[i] = Q.NaN; // marking as not valid
                continue;
            }
            ratioTestColumn[i] = cjColumn[i] / enterVariableColumn[i];
        }

        Q min = ratioTestColumn[_additionalRowsCount..].Where(x => !x.IsNaN).Min(); // address this issue when there is no valid elements -> all are NaN
        // TODO: Może być problem kiedy jakaś zmienna już była a bazie
        return Array.FindIndex(ratioTestColumn, _additionalRowsCount, x => x == min);
    }

    public static Q[] GenerateObjectiveFunctionRow(Q[,] firstPlayerPayouts, Q[,] secondPlayerPayouts, int tablueaxHorizontalSize) {
        var objectiveFunctionRow = new Q[tablueaxHorizontalSize];
        objectiveFunctionRow.InitializeWithZero();

        objectiveFunctionRow[0] = 1;
        int counter = 1;
        for (int i = 0; i < firstPlayerPayouts.GetLength(0); i++) {
            for (int j = 0; j < firstPlayerPayouts.GetLength(1); j++) {
                // Minus becasue you need to change side in objective equation
                objectiveFunctionRow[counter] = - (firstPlayerPayouts[i, j] + secondPlayerPayouts[i, j]);
                counter++;
            }
        }

        return objectiveFunctionRow;
    }
}
