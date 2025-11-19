using ConnectFour.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ConnectFour.DAL.Repositories;

public class ConnectFourRepository(AppDbContext dbContext) : IConnectFourRepository
{
    public async Task<BoardEntity?> GetBoardById(int boardId)
    {
        return await dbContext.Boards
                .Include(b => b.Columns)
                    .ThenInclude(c => c.Rows)
                .FirstOrDefaultAsync(b => b.Id == boardId);
    }

    public async Task<BoardEntity> CreateOrUpdateBoard(BoardEntity boardEntity)
    {
        var existingBoardEntity = await dbContext.Boards
                .Include(b => b.Columns)
                    .ThenInclude(c => c.Rows)
                .FirstOrDefaultAsync(b => b.Id == boardEntity.Id);

        if (existingBoardEntity is not null)
        {
            dbContext.Entry(existingBoardEntity).State = EntityState.Detached;
            dbContext.Update(boardEntity);
        }
        else
        {
            await dbContext.Boards.AddAsync(boardEntity);
        }

        await dbContext.SaveChangesAsync();

        return boardEntity;
    }
}
