namespace SecondApi2.Services;

public class SingletonService : ISingletonService
{
    private readonly Guid _serviceId;
    private readonly DateTime _createdAt;
    public SingletonService()
    {
        _serviceId = Guid.NewGuid();
        _createdAt = DateTime.Now;
    }
    public string Name => nameof(SingletonService);
    public string SayWassup()
    {
        return $"wassup! i am {Name}. My id is {_serviceId}. I was creatred at {_createdAt : yyyy-MM-dd HH:mm:ss}.";
    }
}