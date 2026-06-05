namespace MetaCrudFosis.Web.Models;

public class SystemLog
{
    public int Id { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? Source { get; set; }
    public string? Details { get; set; }
}