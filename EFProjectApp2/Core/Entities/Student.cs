namespace EFProjectApp2.Core.Entities;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Surname { get; set; }
    public int Age { get; set; }
    public DateTime CreatedTime { get; set; }
    public ICollection<StudentGroup> StudentGroups { get; set; }
    public DateTime CreatedDate { get; internal set; }
}
