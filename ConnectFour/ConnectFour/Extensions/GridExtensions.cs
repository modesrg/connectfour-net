namespace ConnectFour.Extensions;

public static class GridExtensions
{
    public static string?[] GetColumn(this string[,] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[columnNumber, x])
                .ToArray();
    }

    public static string?[] GetRow(this string[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, rowNumber])
                .ToArray();
    }
}
