using Microsoft.AspNetCore.Mvc;
using boardcast_server_example.Models;
using boardcast_server_example.Hubs;
using Microsoft.AspNetCore.SignalR;
using boardcast_server_example.DataBase;
namespace boardcast_server_example.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{
    private readonly IHubContext<DeviceHub> _deviceHubContext;
    private readonly DbHelper _db;

    public DeviceController(IHubContext<DeviceHub> hubContext, EF_DataContext context)
    {
        _deviceHubContext = hubContext;
        _db = new DbHelper(context);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_db.List());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var d = _db.Get(id);
        if (d != null)
        {
            return Ok(d);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    public IActionResult Create(DeviceModel d)
    {
        return Ok(_db.SaveOrUpdate(d));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, DeviceModel d)
    {
        d.ID = id;
        bool updateSuccess = _db.SaveOrUpdate(d);
        if (updateSuccess)
        {
            var updateRes = Get(id);
            if (updateRes is OkObjectResult ok)
            {
                DeviceModel? ud = (DeviceModel?)ok.Value;
                if (ud != null)
                {
                    await _deviceHubContext.Clients.All.SendAsync("UpdateDevice", ud);
                }
                else
                {
                    return StatusCode(500, "SignalR update failed");
                }
            }
            else
            {
                return StatusCode(500, "Get update device failed");
            }
        }

        return updateSuccess ? Ok() : StatusCode(500, "Update failed");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return Ok(_db.Delete(id));
    }
}