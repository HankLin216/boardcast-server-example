using Microsoft.AspNetCore.SignalR;
using boardcast_server_example.Models;

namespace boardcast_server_example.Hubs
{
    public class DeviceHub : Hub
    {
        public async Task SendDevice(Device device)
        {
            await Clients.All.SendAsync("ReceiveDevice", device);
        }
    }
}