namespace robot_controller_api.Persistence;
using task3._2.Models;
public interface IMapDataAccess{
    List<Map> GetMaps();
    Map AddMap(Map newMap);
    Map UpdateMap(Map updatedMap);
    Map DeleteMap(Map toDelete);
}