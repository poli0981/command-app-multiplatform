namespace CommandApp.Models;

public class Command
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // true nếu command cần argument từ user (mkdir, rm, touch...)
    public bool RequiresArgument { get; set; }

    // Command template cross-platform: dùng {0} làm placeholder cho arg
    // Nếu không cần arg thì để string rỗng hoặc null
    public string? WindowsCommand { get; set; }
    public string? UnixCommand { get; set; }
}