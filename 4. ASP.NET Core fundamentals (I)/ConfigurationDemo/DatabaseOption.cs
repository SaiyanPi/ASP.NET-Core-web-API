namespace ConfigurationDemo;

//  options pattern:
//  option class must be non-abstract with the public parameterless constructor
public class DatabaseOption
{
    // used to specify the section name in appsetting.json file.
    public const string SectionName = "Database";   
    public string Type { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}
//
//
//
//
//
//  there are multiple ways to use the options pattern
//  1) using the ConfigurationBinder.Bind() method
//  2) using the ConfigurationBinder.Get<TOption>() method
//  3) Using the IOptions<TOption> interface
//      -> ASP.NET Core provides built-in DI support for options pattern.
//      -> to use DI, we need to register the DatabasewOption class in the  Services.Configure() 
//          method of the Program.cs.


//  named options
public class DatabaseOptions
{
    public const string SectionName = "Databases";
    public const string SystemDatabaseSectionName = "System";
    public const string BusinessDatabaseSectionName = "Business";
    public string Type { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
} 