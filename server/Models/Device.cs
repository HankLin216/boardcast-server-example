using System.ComponentModel.DataAnnotations;

namespace boardcast_server_example.Models;

public class Device
{
    [Required]
    public string? ID { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Type { get; set; }
    public string? Status { get; set; }
    public string? Create_Date { get; set; }
    public string? Update_Date { get; set; }
}
