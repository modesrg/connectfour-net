namespace ConnectFour.Models;

public class DropedPiece
{
    public int BoardId { get; set; } = 1;
    public required string Color { get; set; }
    public required int Column { get; set; }
}
