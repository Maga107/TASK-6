using EFProjectApp2.Core.Entities;
using EFProjectApp2.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

public class Program
{
    private static async Task Main(string[] args)
    {
        string Welcome = "  _      _________________  __  _________\r\n | | /| / / __/ / ___/ __ \\/  |/  / __/ /\r\n | |/ |/ / _// / /__/ /_/ / /|_/ / _//_/ \r\n |__/|__/___/_/\\___/\\____/_/  /_/___(_)  \r\n                                         ";
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(Welcome);
        string Option = "Option download:)";
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(Option);
        Console.ResetColor();

        AppDbContext context = new();
        List<Student> students = new();
        List<Group> groups = new();
        List<StudentGroup> studentGroups = new();

        bool run = true;
        while (run)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1 == Create Student\n" +
                              "2 == Show All Students\n" +
                              "3 == Create Group\n" +
                              "4 == Show All Groups\n" +
                              "5 == Add Student to group\n" +
                              "6 == Update Student \n" +
                              "0 == Close App\n" +
                              " ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Chosse the option >> ");
            Console.ResetColor();

            string? option = Console.ReadLine();
            int IntOption;
            bool IsInt = int.TryParse(option, out IntOption);
            if (IsInt)
            {
                if (IntOption >= 0 && IntOption <= 5)
                {
                    switch (IntOption)
                    {
                        case 1:
                            try
                            {
                                Console.Write("Enter Student Name: ");
                                string? studentName = Console.ReadLine();
                                if (string.IsNullOrEmpty(studentName)) throw new ArgumentNullException();
                                Console.Write("Enter student surname: ");
                                string? studentSurname = Console.ReadLine();
                                if (string.IsNullOrEmpty(studentSurname)) throw new ArgumentNullException();
                                int studentAge;
                                do
                                {
                                    Console.Write("Enter student age : ");
                                } while (int.TryParse(Console.ReadLine(), out studentAge) || studentAge <= 15);

                                Student student = new()
                                {
                                    Name = studentName,
                                    Surname = studentSurname,
                                    Age = studentAge,
                                    CreatedDate = DateTime.Now
                                };
                                students.Add(student);
                                await context.Students.AddAsync(student);
                                await context.SaveChangesAsync();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Student created!");
                                Console.ResetColor();
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(ex.Message);
                                Console.ResetColor();
                                goto case 1;
                            }
                            break;
                        case 2:
                            try
                            {
                                Console.Write("Enter group name:");
                                string? groupName = Console.ReadLine();
                                if (string.IsNullOrEmpty(groupName)) throw new ArgumentNullException();
                                Group? existingGroup = await context.Groups.FirstOrDefaultAsync(g => g.Name == groupName);
                                if (existingGroup is null)
                                {
                                    Group group = new()
                                    {
                                        Name = groupName,
                                        CreatedTime = DateTime.Now
                                    };
                                    groups.Add(group);
                                    await context.Groups.AddAsync(group);
                                    await context.SaveChangesAsync();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Group created!");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Group with name '{groupName}' already exists.");
                                    Console.ResetColor();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(ex.Message);
                                Console.ResetColor();
                                goto case 2;
                            }
                            break;
                        case 3:
                            try
                            {
                                Console.Write("Enter student ID to add to the group: ");
                                static void ShowAllStudents(AppDbContext context)
                                {
                                    var students = context.Students.ToList();
                                    Console.WriteLine("All Students:");
                                    foreach (var student in students)
                                    {
                                        Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name}, Age: {student.Age}");
                                    }
                                };
                                if (int.TryParse(Console.ReadLine(), out int studentId))
                                {
                                    Student? existingStudent = await context.Students.FindAsync(studentId);
                                    if (existingStudent is not null)
                                    {
                                        Console.Write("Enter group ID to add the student to: ");
                                        static void ShowAllGroups(AppDbContext context)
                                        {
                                            var groups = context.Groups.ToList();
                                            Console.WriteLine("All Groups:");
                                            foreach (var group in groups)
                                            {
                                                Console.WriteLine($"Group ID: {group.Id}, Name: {group.Name}");
                                            }
                                        };
                                        if (int.TryParse(Console.ReadLine(), out int groupId))
                                        {
                                            Group? existingGroup = await context.Groups.FindAsync(groupId);
                                            if (existingGroup is not null)
                                            {
                                                if (await context.StudentGroups.AnyAsync(sg => sg.StudentId == studentId && sg.GroupId == groupId))
                                                {
                                                    StudentGroup studentGroup = new()
                                                    {
                                                        StudentId = studentId,
                                                        GroupId = groupId,
                                                    };
                                                    studentGroups.Add(studentGroup);
                                                    await context.StudentGroups.AddAsync(studentGroup);
                                                    await context.SaveChangesAsync();

                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("Student added to the group !");
                                                    Console.ResetColor();
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("Student is already in the group");
                                                    Console.ResetColor();
                                                }
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine($"\nGroup with ID '{groupId}' not found.\n");
                                                Console.ResetColor();
                                            }
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Invalid group ID format");
                                            Console.ResetColor();
                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"Student with ID '{studentId}' not found");
                                        Console.ResetColor();
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid student ID format");
                                    Console.ResetColor();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(ex.Message);
                                Console.ResetColor();
                                goto case 3;
                            }
                            break;
                        case 4:
                            try
                            {
                                Console.Write("Enter student ID to update group: ");
                                static void ShowAllStudents(AppDbContext context)
                                {
                                    var students = context.Students.ToList();
                                    Console.WriteLine("All Students:");
                                    foreach (var student in students)
                                    {
                                        Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name}, Age: {student.Age}");
                                    }
                                };
                                if (int.TryParse(Console.ReadLine(), out int updateStudentId))
                                {
                                    Student? existingStudent = await context.Students.FindAsync(updateStudentId);
                                    if (existingStudent is not null)
                                    {
                                        Console.Write("Enter new group ID for the student: ");
                                        static void ShowAllGroups(AppDbContext context)
                                        {
                                            var groups = context.Groups.ToList();
                                            Console.WriteLine("All Groups:");
                                            foreach (var group in groups)
                                            {
                                                Console.WriteLine($"Group ID: {group.Id}, Name: {group.Name}");
                                            }
                                        };
                                        if (int.TryParse(Console.ReadLine(), out int newGroupId))
                                        {
                                            Group? existingGroup = await context.Groups.FindAsync(newGroupId);
                                            if (existingGroup is not null)
                                            {
                                                if (await context.StudentGroups.AnyAsync(sg => sg.StudentId == updateStudentId && sg.GroupId == newGroupId))
                                                {
                                                    var currentStudentGroup = await context.StudentGroups.FirstOrDefaultAsync(sg => sg.StudentId == updateStudentId);
                                                    if (currentStudentGroup is not null)
                                                    {
                                                        context.StudentGroups.Remove(currentStudentGroup);
                                                    }
                                                    StudentGroup newStudentGroup = new()
                                                    {
                                                        StudentId = updateStudentId,
                                                        GroupId = newGroupId,
                                                    };
                                                    await context.StudentGroups.AddAsync(newStudentGroup);
                                                    await context.SaveChangesAsync();

                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("Student group updated!");
                                                    Console.ResetColor();
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("Student is already in this group.");
                                                    Console.ResetColor();
                                                }
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine($"Group with ID '{newGroupId}' not found.");
                                                Console.ResetColor();
                                            }
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Invalid group ID format.");
                                            Console.ResetColor();
                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"Student with ID '{updateStudentId}' not found.");
                                        Console.ResetColor();
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid student ID format.");
                                    Console.ResetColor();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(ex.Message);
                                Console.ResetColor();
                                goto case 4;
                            }
                            break;
                        case 5:
                            static void ShowAllStudents2(AppDbContext context)
                            {
                                var students = context.Students.ToList();
                                Console.WriteLine("All Students:");
                                foreach (var student in students)
                                {
                                    Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name}, Age: {student.Age}");
                                }
                            }; ;
                            break;
                        case 6:
                            static void ShowAllGroups1(AppDbContext context)
                            {
                                var groups = context.Groups.ToList();
                                Console.WriteLine("All Groups:");
                                foreach (var group in groups)
                                {
                                    Console.WriteLine($"Group ID: {group.Id}, Name: {group.Name}");
                                }
                            }; ;
                            break;
                        case 0:
                            run = false;
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("App closed!");
                            Console.ResetColor();
                            break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nPlease enter correct number!\n");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nPlease enter correct format!\n");
                Console.ResetColor();
            }
        }
    }
}
