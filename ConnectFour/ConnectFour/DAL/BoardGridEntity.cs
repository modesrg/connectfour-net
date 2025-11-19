using static ConnectFour.Constants.ConnectFourConstants;

namespace ConnectFour.DAL;

public class BoardGridEntity
{
    public int Id { get; set; }
    public List<List<string>> Cells { get; set; } = Initialize(Size.Default.Columns, Size.Default.Rows);

    private static List<List<string>> Initialize(int rows, int cols)
    {
        var grid = new List<List<string>>(rows);

        for (int r = 0; r < rows; r++)
        {
            var row = new List<string>(cols);
            for (int c = 0; c < cols; c++)
                row.Add("");

            grid.Add(row);
        }

        return grid;
    }
}
