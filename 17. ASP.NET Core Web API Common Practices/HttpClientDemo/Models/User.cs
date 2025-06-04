namespace HttpClientDemo.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Address Address { get; set; } = new Address();
    public string Phone { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public Company Company { get; set; } = new Company();
}
