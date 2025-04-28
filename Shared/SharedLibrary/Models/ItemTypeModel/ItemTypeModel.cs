public class ItemTypeModel
{
    public int Id { get; set; }
    public string Level { get; set; }
    public ICollection<ItemModel> Items { get; set; }
}