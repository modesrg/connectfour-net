namespace ConnectFour.Constants;

public class ConnectFourConstants
{
    public struct PieceColor
    {
        public const string Red = "RED";
        public const string Yellow = "YELLOW";
    }

    public struct Size
    {
        public struct Default
        {
            public const int Rows = 6;
            public const int Columns = 7;
        }
    }

    public struct CountDirection
    {
        public static (int dCol, int dRow) Horizontal = (1, 0);
        public static (int dCol, int dRow) Vertical = (0, 1);
        public static (int dCol, int dRow) Asceding = (1, 1);
        public static (int dCol, int dRow) Descending = (1, -1);

        public static (int dCol, int dRow)[] Directions = [Horizontal, Vertical, Asceding, Descending];
    }
}
