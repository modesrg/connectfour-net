using ConnectFour.Constants;
using ConnectFour.DAL.Repositories.Interface;
using ConnectFour.Extensions;
using ConnectFour.Mappings;
using ConnectFour.Models;
using ConnectFour.Services.Interfaces;
using System.Text.Json;
using static ConnectFour.Constants.ConnectFourConstants;

namespace ConnectFour.Services;

public class ConnectFourService(IConnectFourRepository connectFourRepository) : IConnectFourService
{
    public async Task<string> PlacePiece(DropedPiece dropPiece)
    {
        var board = await GetBoard(dropPiece.BoardId);

        var playedColumn = board.Cells.GetColumn(dropPiece.Column);
        var landingRow = playedColumn.ToList().IndexOf(null);
        board.Cells[dropPiece.Column, landingRow] = dropPiece.Color;

        var isWinner = CheckWinningMove(board, dropPiece.Column, landingRow, dropPiece.Color);

        var boardJson = await connectFourRepository.CreateOrUpdateBoard(board.ToBoardEntity());

        return JsonSerializer.Serialize(board.ToBoardEntity());
    }

    public async Task<BoardGrid> GetBoard(int boardId)
    {
        var boardEntity = await connectFourRepository.GetBoardById(boardId);
        return boardEntity?.ToBoardGrid() ?? new BoardGrid();
    }

    private bool CheckWinningMove(BoardGrid board, int col, int row, string playedColor)
    {
        foreach (var (dCol, dRow) in CountDirection.Directions)
        {
            int count = 1;

            count += CountInDirection(board, col, row, dCol, dRow, playedColor);
            count += CountInDirection(board, col, row, -dCol, -dRow, playedColor);

            if (count >= 4)
                return true;
        }

        return false;
    }

    private int CountInDirection(BoardGrid board, int col, int row, int dCol, int dRow, string color)
    {
        int cols = board.Cells.GetLength(0);
        int rows = board.Cells.GetLength(1);
        int streak = 0;

        for (int step = 1; step < 4; step++)
        {
            int c = col + step * dCol;
            int r = row + step * dRow;

            if (c < 0 || c >= cols || r < 0 || r >= rows)
                break;

            var value = board.Cells[c, r];
            if (value is null || !value.Equals(color, StringComparison.OrdinalIgnoreCase))
                break;

            streak++;
        }

        return streak;
    }


    private bool CheckWinningMoveOld(BoardGrid board, int colNumber, int rowNumber, string playedColor)
    {
        var upperCol = board.Cells.GetLength(0);
        var upperRow = board.Cells.GetLength(1);

        var horizontalRange = Enumerable.Range(
                Math.Max(0, colNumber - 3),
                Math.Min(4 + Math.Min(colNumber, 3), upperCol - Math.Max(0, colNumber - 3))
            );

        var verticalRange = Enumerable.Range(
                Math.Max(0, rowNumber - 3),
                Math.Min(4 + Math.Min(rowNumber, 3), upperRow - Math.Max(0, rowNumber - 3))
            );

        var horizontalCount = 0;
        foreach (var col in horizontalRange)
        {
            var value = board.Cells[col, rowNumber];
            horizontalCount = value is not null && value.Equals(playedColor, StringComparison.OrdinalIgnoreCase)
                ? horizontalCount + 1
                : 0;

            if (horizontalCount >= 4)
                return true;
        }

        var verticalCount = 0;
        foreach (var row in verticalRange)
        {
            var value = board.Cells[colNumber, row];
            verticalCount = value is not null && value.Equals(playedColor, StringComparison.OrdinalIgnoreCase)
                ? verticalCount + 1
                : 0;

            if (verticalCount >= 4)
                return true;
        }

        var ascendingCount = 0;
        for (int offset = -3; offset <= 3; offset++)
        {
            var c = colNumber + offset;
            var r = rowNumber + offset;

            if (c < 0 || c >= upperCol || r < 0 || r >= upperRow)
                continue;

            var value = board.Cells[c, r];
            ascendingCount = value is not null &&
                             value.Equals(playedColor, StringComparison.OrdinalIgnoreCase)
                ? ascendingCount + 1
                : 0;

            if (ascendingCount >= 4)
                return true;
        }

        var descendingCount = 0;
        for (int offset = -3; offset <= 3; offset++)
        {
            var c = colNumber + offset;
            var r = rowNumber - offset;

            if (c < 0 || c >= upperCol || r < 0 || r >= upperRow)
                continue;

            var value = board.Cells[c, r];
            descendingCount = value is not null &&
                              value.Equals(playedColor, StringComparison.OrdinalIgnoreCase)
                ? descendingCount + 1
                : 0;

            if (descendingCount >= 4)
                return true;
        }

        return false;
    }


}
