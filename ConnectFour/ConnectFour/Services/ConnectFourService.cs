using ConnectFour.DAL.Repositories.Interface;
using ConnectFour.Extensions;
using ConnectFour.Mappings;
using ConnectFour.Models;
using ConnectFour.Services.Interfaces;
using System.Text.Json;

namespace ConnectFour.Services;

public class ConnectFourService(IConnectFourRepository connectFourRepository) : IConnectFourService
{
    public async Task<string> PlacePiece(DropedPiece dropPiece)
    {
        var board = await GetBoard(dropPiece.BoardId);

        var playedColumn = board.Cells.GetColumn(dropPiece.Column);
        var landingSpot = playedColumn.ToList().IndexOf(null);
        board.Cells[dropPiece.Column, landingSpot] = dropPiece.Color;

        var boardJson = await connectFourRepository.CreateOrUpdateBoard(board.ToBoardEntity());

        return JsonSerializer.Serialize(board.ToBoardEntity());
    }

    public async Task<BoardGrid> GetBoard(int boardId)
    {
        var boardEntity = await connectFourRepository.GetBoardById(boardId);
        return boardEntity?.ToBoardGrid() ?? new BoardGrid();
    }

}
