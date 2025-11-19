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
            var count = 1;

            count += CountInDirection(board, col, row, dCol, dRow, playedColor);
            count += CountInDirection(board, col, row, -dCol, -dRow, playedColor);

            if (count >= 4)
                return true;
        }

        return false;
    }

    private int CountInDirection(BoardGrid board, int col, int row, int dCol, int dRow, string color)
    {
        var cols = board.Cells.GetLength(0);
        var rows = board.Cells.GetLength(1);
        var streak = 0;

        for (var step = 1; step < 4; step++)
        {
            var c = col + step * dCol;
            var r = row + step * dRow;

            if (c < 0 || c >= cols || r < 0 || r >= rows)
                break;

            var value = board.Cells[c, r];
            if (value is null || !value.Equals(color, StringComparison.OrdinalIgnoreCase))
                break;

            streak++;
        }

        return streak;
    }

}
