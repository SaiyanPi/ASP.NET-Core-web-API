namespace ClaimBasedAuthorizationDemo.Authentication;

public static class AppAuthorizationPolicies
{
    public const string RequireDrivingLicenseNumber = "RequireDrivingLicenseNumber";
    public const string RequireAccessNumber = "RequireAccessNumber";
    public const string RequireCountry = "RequireCountry";
    public const string RequireDrivingLicenseNumberAndAccessNumber = "RequireDrivingLicenseNumberAndAccessNumber";
}