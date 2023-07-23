using boardcast_server_example.DataBase;

namespace boardcast_server_example.Models;

public class DbHelper
{
    private EF_DataContext _context;
    public DbHelper(EF_DataContext context)
    {
        _context = context;
    }
    public List<DeviceModel> List()
    {
        List<DeviceModel> dms = new();
        var dataList = _context.Devices.ToList();
        foreach (var d in dataList)
        {
            dms.Add(
                new DeviceModel()
                {
                    ID = d.ID,
                    Name = d.Name,
                    Type = d.Type,
                    Status = d.Status,
                    Create_at = d.Create_at,
                    Update_at = d.Update_at
                }
            );
        }

        return dms;
    }
    public DeviceModel? Get(int id)
    {
        var d = _context.Devices.Where(r => r.ID == id).FirstOrDefault();
        if (d != null)
        {
            return new DeviceModel()
            {
                ID = d.ID,
                Name = d.Name,
                Type = d.Type,
                Status = d.Status,
                Create_at = d.Create_at,
                Update_at = d.Update_at
            };
        }

        return null;
    }
    public bool SaveOrUpdate(DeviceModel m)
    {
        if (m.ID > 0)
        {
            var device = _context.Devices.Where(r => r.ID == m.ID).FirstOrDefault();
            if (device != null)
            {
                device.Name = m.Name;
                device.Type = m.Type;
                device.Status = m.Status;
                device.Update_at = DateTime.Now;
            }
        }
        else
        {
            _context.Devices.Add(new Device()
            {
                Name = m.Name,
                Type = m.Type,
                Status = m.Status,
            });
        }
        var n = _context.SaveChanges();
        return n != 0;
    }
    public bool Delete(int id)
    {
        bool success = false;
        var d = _context.Devices.Where(r => r.ID == id).FirstOrDefault();
        if (d != null)
        {
            _context.Devices.Remove(d);
            var n = _context.SaveChanges();
            if (n != 0)
            {
                success = true;
            }
        }

        return success;
    }
}