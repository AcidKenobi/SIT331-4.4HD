using Npgsql;
namespace robot_controller_api.Persistence;
using task3._2.Models;

public class MapRepository : IMapDataAccess, IRepository{
    private IRepository _repo => this;

    public List<Map> GetMaps(){
        var maps = _repo.ExecuteReader<Map>("SELECT * FROM maps");
        return maps;
    }

    public Map AddMap(Map newMap){
        var sqlParams = new NpgsqlParameter[]{
            new("columns", newMap.Columns),
            new("rows", newMap.Rows),
            new("Name", newMap.Name),
            new("description", newMap.Description ?? (object)DBNull.Value),
            new("created_date", newMap.CreatedDate),
            new("modified_date", newMap.ModifiedDate),
            new("is_square", newMap.IsSquare)
        };
        
        var result = _repo.ExecuteReader<Map>(
            "INSERT INTO maps (columns, rows, \"Name\", description, created_date, modified_date, is_square) " +
            "VALUES (@columns, @rows, @Name, @description, @created_date, @modified_date, @is_square) RETURNING *;", 
            sqlParams).Single();

        return result;
    }

    public Map UpdateMap(Map updatedMap){
        var sqlParams = new NpgsqlParameter[]{
            new("id", updatedMap.Id),
            new("columns", updatedMap.Columns),
            new("rows", updatedMap.Rows),
            new("Name", updatedMap.Name),
            new("description", updatedMap.Description ?? (object)DBNull.Value),
            new("is_square", updatedMap.IsSquare)
        };

        // It can return the whole entity RETURNING * 
        // or just a particular field RETURNING id that can then be used in the application layer if needed.
        var result = _repo.ExecuteReader<Map>(
            "UPDATE maps SET columns=@columns, rows=@rows, \"Name\"=@Name, description=@description, " +
            "modified_date=current_timestamp, is_square=@is_square WHERE id=@id RETURNING *;", 
            sqlParams).Single();

        return result;
    }

    public Map DeleteMap(Map toDelete){
        var sqlParams = new NpgsqlParameter[]{
            new("id", toDelete.Id)
        };

        var result = _repo.ExecuteReader<Map>(
            "DELETE FROM maps WHERE id = @id RETURNING *;",
            sqlParams).Single();

        return result;
    }
}