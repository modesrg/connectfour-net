using static ConnectFour.Constants.ConnectFourConstants;

namespace ConnectFour.Models;

public class BoardGrid
{
    public int Id { get; set; }
    public string[,] Cells { get; set; } = new string[Size.Default.Columns, Size.Default.Rows];
}
