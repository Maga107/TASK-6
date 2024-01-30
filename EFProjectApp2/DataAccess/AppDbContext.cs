using EFProjectApp2.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFProjectApp2.DataAccess;

public class AppDbContext:DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=MSI\SQLEXPRESS;Database=EFDb;Trusted_Connection=true;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentGroup>()
            .HasKey(sg => new { sg.StudentId, sg.GroupId });
        modelBuilder.Entity<Group>()
            .HasIndex(g => g.Name)
            .IsUnique();
        modelBuilder.Entity<Student>()
            .HasMany(s => s.StudentGroups)
            .WithOne(sg => sg.Student)
            .HasForeignKey(sg => sg.StudentId);
        modelBuilder.Entity<Group>()
            .HasMany(g => g.StudentGroups)
            .WithOne(sg => sg.Group)
            .HasForeignKey(sg => sg.GroupId);
    }
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<StudentGroup> StudentGroups { get; set; } = null!;
}
