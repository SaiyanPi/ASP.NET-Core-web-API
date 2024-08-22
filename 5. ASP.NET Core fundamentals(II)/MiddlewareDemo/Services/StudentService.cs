

namespace MiddlewareDemo.Services;

public class StudentService : IStudentService
{
    private readonly DateTime _dateTime;
    public StudentService()
    {
        _dateTime = DateTime.Now;
    }
    public string StudentDetail()
    {
        return $"student id: 1 \n student name: Robin \n student course: ASP.NET Core \n created at: {_dateTime : yyyy-MM-dd HH:mm:ss}";
    }
}