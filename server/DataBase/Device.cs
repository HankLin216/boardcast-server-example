using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace boardcast_server_example.DataBase;

[Table("Devices")]
public class Device
{
    [Key]
    public int ID { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public string? Status { get; set; }
    public DateTime Create_at { get; set; }
    public DateTime Update_at { get; set; }
}
