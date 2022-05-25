namespace NashEquilibriumFinder.Core.Helpers;

public static class MatrixParser
{
    public static (int[,] rowPayoffs, int[,] columnPayoffs) GetMatricesFromFile(string filePath)
    {
        string[] clearedText = FileReader.GetClearedText(filePath);

        string[] rows = clearedText[0]
            .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        string[] columns = clearedText[1]
            .Split('\n', StringSplitOptions.RemoveEmptyEntries);

        int[,] rowPayoffs = GetPayoffMatrix(rows);
        int[,] columnPayoffs = GetPayoffMatrix(columns);

        return (rowPayoffs, columnPayoffs);
    }

    private static int[,] GetPayoffMatrix(string[] text)
    {
        int[,] payoffMatrix = null;

        for (int i = 0; i < text.Length; i++)
        {
            var payoffsOfStrategy = text[i].Split(' ');
            payoffMatrix = new int[text.Length, payoffsOfStrategy.Length];

            for (int j = 0; j < payoffsOfStrategy.Length; j++)
            {
                payoffMatrix[i, j] = int.Parse(payoffsOfStrategy[j]);
            }
        }

        return payoffMatrix;
    }
}
