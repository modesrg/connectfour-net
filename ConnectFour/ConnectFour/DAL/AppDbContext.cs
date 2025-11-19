using ConnectFour.DAL;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<BoardEntity> Boards { get; set; }
    public DbSet<ColumnEntity> Columns { get; set; }
    public DbSet<RowEntity> Rows { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Board
        modelBuilder.Entity<BoardEntity>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<BoardEntity>()
            .HasMany(b => b.Columns)
            .WithOne()
            .HasForeignKey(c => c.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        // Column: composite key (BoardID + ID)
        modelBuilder.Entity<ColumnEntity>()
            .HasKey(c => new { c.BoardId, c.Id });

        modelBuilder.Entity<ColumnEntity>()
            .HasMany(c => c.Rows)
            .WithOne()
            .HasForeignKey(r => new { r.BoardId, r.ColumnId })
            .OnDelete(DeleteBehavior.Cascade);

        // Row: composite key (BoardId + ColumnId + ID)
        modelBuilder.Entity<RowEntity>()
            .HasKey(r => new { r.BoardId, r.ColumnId, r.Id });
    }

}
