namespace EFProjectApp2.Core.Entities;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedTime { get; set; }
    public ICollection<StudentGroup> StudentGroups { get; set; }

}
