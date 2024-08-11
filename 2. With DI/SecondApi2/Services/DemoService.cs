namespace SecondApi2.Services;

public class DemoService : IDemoService
{
    private readonly Guid _serviceId;
    private readonly DateTime _createdAt;

    public DemoService()
    {
        _serviceId = Guid.NewGuid();
        _createdAt = DateTime.UtcNow;
    }

    public string SayWassup()
    {
        return $"Hello! my id is {_serviceId}. I was created at {_createdAt:yyyy-mm-dd HH:MM:ss}.";
    }
}