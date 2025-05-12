using robot_controller_api.Persistence;
using task3._2.Models;
using task3._2.Persistence;

public class MapEF : IMapDataAccess
{
    private readonly RobotContext _context;

    public MapEF(RobotContext context)
    {
        _context = context;
    }

    public List<Map> GetMaps()
    {
        return _context.Maps.ToList();
    }

    public Map AddMap(Map newMap)
    {
        _context.Maps.Add(newMap);
        _context.SaveChanges();
        return newMap;
    }

    public Map UpdateMap(Map updatedMap)
    {
        var toUpdate = _context.Maps.Find(updatedMap.Id);
        if (toUpdate == null)
        {
            return null;
        }

        _context.Entry(toUpdate).CurrentValues.SetValues(updatedMap);
        toUpdate.ModifiedDate = DateTime.UtcNow;

        _context.SaveChanges();
        return toUpdate;
    }

    public Map DeleteMap(Map toDelete)
    {
        var m = _context.Maps.Find(toDelete.Id);
        if (m == null)
        {
            return null;
        }

        _context.Maps.Remove(m);
        _context.SaveChanges();
        return m;
    }
}