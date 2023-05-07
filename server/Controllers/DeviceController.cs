using Microsoft.AspNetCore.Mvc;
using boardcast_server_example.Models;
using boardcast_server_example.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace boardcast_server_example.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{
    private readonly IHubContext<DeviceHub> _deviceHubContext;
    private static Dictionary<string, Device> _deviceByID = new Dictionary<string, Device>(){
        {"1", new Device
        {
            ID = "1",
            Name = "nvme1n1",
            Type = "PCIe",
            Status = "idle",
            Update_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Create_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        }},
        {"2", new Device
        {
            ID = "2",
            Name = "nvme2n1",
            Type = "PCIe",
            Status = "idle",
            Update_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Create_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        }},
        {"3", new Device
        {
            ID = "3",
            Name = "nvme3n1",
            Type = "PCIe",
            Status = "idle",
            Update_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Create_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        }}
    };

    public DeviceController(IHubContext<DeviceHub> hubContext)
    {
        _deviceHubContext = hubContext;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_deviceByID.Values.ToArray());
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        if (_deviceByID.TryGetValue(id, out Device? _dev))
        {
            return Ok(_dev);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    public IActionResult Create(Device dev)
    {
        if (string.IsNullOrEmpty(dev.ID))
        {
            return BadRequest();
        }

        if (_deviceByID.TryGetValue(dev.ID, out Device? _dev))
        {
            return Conflict();
        }
        else
        {
            dev.Status = "Idle";
            dev.Create_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dev.Update_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _deviceByID.Add(dev.ID, dev);
            return Ok(dev.ID);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(string id, Device dev)
    {
        if (!_deviceByID.ContainsKey(id))
        {
            return NotFound();
        }

        _deviceByID[id].Name = dev.Name;
        _deviceByID[id].Type = dev.Type;
        _deviceByID[id].Status = dev.Status;
        _deviceByID[id].Update_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        await _deviceHubContext.Clients.All.SendAsync("UpdateDevice", _deviceByID[id]);

        return Ok(_deviceByID[id]);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        if (!_deviceByID.ContainsKey(id))
        {
            return NotFound();
        }

        _deviceByID.Remove(id);

        return Ok(id);
    }
}