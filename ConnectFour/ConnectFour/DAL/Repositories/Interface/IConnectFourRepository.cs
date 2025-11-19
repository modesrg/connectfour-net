namespace ConnectFour.DAL.Repositories.Interface;

public interface IConnectFourRepository
{
    Task<BoardEntity?> GetBoardById(int boardId);
    Task<BoardEntity> CreateOrUpdateBoard(BoardEntity position);
}