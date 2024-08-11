namespace SecondApi2.Services;


public class ScopedService : IScopedService
{
    private readonly Guid _serviceId;
    private readonly DateTime _createdAt;
    private readonly ITransientService _transientService;
    private readonly ISingletonService _singletonService;
    public ScopedService(ITransientService transientService, ISingletonService singletonService)
    {
        _transientService = transientService;
        _singletonService = singletonService;
        _serviceId = Guid.NewGuid();
        _createdAt = DateTime.Now;
    }
    public string Name => nameof(ScopedService);
    public string SayWassup()
    {
        var scopedServiceMessage = $"wassup! i am {Name}. My id is {_serviceId}. I was creatred at {_createdAt : yyyy-MM-dd HH:mm:ss}.";
        var TransientServiceMessage = $"wassup! i am {Name}. My id is {_serviceId}.I was creatred at {_createdAt : yyyy-MM-dd HH:mm:ss}.";
        var SingletonServiceMessage = $"wassup! i am {Name}. My id is {_serviceId}.I was creatred at {_createdAt : yyyy-MM-dd HH:mm:ss}.";
        return $"wassup! i am {Name}. My id is {_serviceId}. I was creatred at {_createdAt : yyyy-MM-dd HH:mm:ss}.";

    }
}