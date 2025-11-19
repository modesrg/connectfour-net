namespace ConnectFour.DAL;

public class ColumnEntity
{
    public int BoardId { get; set; }
    public int Id { get; set; }
    public ICollection<RowEntity> Rows { get; set; } = new HashSet<RowEntity>();
}

public class RowEntity
{
    public int BoardId { get; set; }
    public int ColumnId { get; set; }
    public int Id { get; set; }
    public string? Value { get; set; }
}

public class BoardEntity
{
    public int Id { get; set; }
    public ICollection<ColumnEntity> Columns { get; set; } = new HashSet<ColumnEntity>();
}
