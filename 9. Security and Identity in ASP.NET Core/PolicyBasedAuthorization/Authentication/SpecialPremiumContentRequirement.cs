using Microsoft.AspNetCore.Authorization;

namespace PolicyBasedAuthorization.Authentication;

public class SpecialPremiumContentRequirement : IAuthorizationRequirement
{
    public string Country { get; }
    public SpecialPremiumContentRequirement(string country)
    {
        Country = country;
    }
}