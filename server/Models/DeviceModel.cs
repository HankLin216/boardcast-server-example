namespace boardcast_server_example.Models;

public class DeviceModel
{
    public int ID { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public string? Status { get; set; }
    public DateTime Create_at { get; set; }
    public DateTime Update_at { get; set; }
}