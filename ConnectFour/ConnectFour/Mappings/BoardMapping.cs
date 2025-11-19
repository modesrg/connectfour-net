using ConnectFour.DAL;
using ConnectFour.Models;

namespace ConnectFour.Mappings;

public static class BoardGridMapping
{
    public static BoardEntity ToBoardEntity(this BoardGrid grid)
    {
        var board = new BoardEntity { Id = grid.Id };

        int cols = grid.Cells.GetLength(0);
        int rows = grid.Cells.GetLength(1);

        for (int c = 0; c < cols; c++)
        {
            var column = new ColumnEntity
            {
                BoardId = grid.Id,
                Id = c
            };

            for (int r = 0; r < rows; r++)
            {
                column.Rows.Add(new RowEntity
                {
                    BoardId = grid.Id,
                    ColumnId = c,
                    Id = r,
                    Value = grid.Cells[c, r]
                });
            }

            board.Columns.Add(column);
        }

        return board;
    }

    public static BoardGrid ToBoardGrid(this BoardEntity board)
    {
        if (board.Columns.Count == 0)
            return new BoardGrid { Id = board.Id };

        int cols = board.Columns.Count;
        int rows = board.Columns.First().Rows.Count;

        var grid = new BoardGrid
        {
            Id = board.Id,
            Cells = new string[cols, rows]
        };

        foreach (var column in board.Columns)
        {
            foreach (var row in column.Rows)
            {
                grid.Cells[column.Id, row.Id] = row.Value;
            }
        }

        return grid;
    }




}
