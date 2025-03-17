namespace SchoolManagement.GraphQL.Mutations;

public record AddTeacherInput(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? Bio);