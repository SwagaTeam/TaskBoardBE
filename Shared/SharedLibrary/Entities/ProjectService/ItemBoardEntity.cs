using SharedLibrary.Entities.ProjectService;

public class ItemBoardEntity
{
    public int Id { get; set; }
    public int BoardId { get; set; }
    public int ItemId { get; set; }
    public int StatusId { get; set; }

    public virtual BoardEntity Board { get; set; } = null!;
    public virtual ItemEntity Item { get; set; } = null!;
    public virtual StatusEntity Status { get; set; } = null!;
}