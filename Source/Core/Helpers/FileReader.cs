namespace NashEquilibriumFinder.Core.Helpers;

public static class FileReader
{
    public static string[] GetClearedText(string filePath)
    {
        string text = File.ReadAllText(filePath);

        // TODO: Exception for wrong format input
        return text.Split(
            new string[] { "\n\n" },
            StringSplitOptions.RemoveEmptyEntries);
    }
}