using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Mutations;

// public class AddTeacherPayload
// {
//     public Teacher Teacher { get; }
//     public AddTeacherPayload(Teacher teacher)
//     {
//         Teacher = teacher;
//     }
// }
public class AddTeacherPayload(Teacher teacher)
{
    public Teacher Teacher { get; } = teacher;
}