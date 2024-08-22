
namespace MiddlewareDemo.Services;

public class AddressService : IAddressService
{
    public string StudentAddress()
    {
        return $"student address: kathmandu, Nepal";
    }
}