using Npgsql;
namespace robot_controller_api.Persistence;

using task3._2.Models;

public class MapADO : IMapDataAccess {

    private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=password;Database=sit331";

    public List<Map> GetMaps(){
        var maps = new List<Map>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT * FROM maps", conn);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        { //read values off the data reader and create a new map here and then add it to the result list.

            var id = dr.GetInt32(0);
            var columns = dr.GetInt32(1);
            var rows = dr.GetInt32(2);
            var name = dr.GetString(3);
            var desc = dr.IsDBNull(2) ? null : dr.GetString(4);
            var createdDate = dr.GetDateTime(5);
            var modDate = dr.GetDateTime(6);
            var isSquare = dr.GetBoolean(7);


            Map map = new Map(id, columns, rows, name, desc, createdDate, modDate, isSquare);
            maps.Add(map);
        }
        return maps;
    }

    public Map AddMap(Map newMap){
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand(
            "INSERT INTO maps (columns, rows, \"Name\", description, created_date, modified_date, is_square)" +
            "VALUES (@columns, @rows, @Name, @description, @created_date, @modified_date, @is_square) RETURNING id", conn);

        cmd.Parameters.AddWithValue("columns", newMap.Columns);
        cmd.Parameters.AddWithValue("rows", newMap.Rows);
        cmd.Parameters.AddWithValue("Name", newMap.Name);
        cmd.Parameters.AddWithValue("Description", (object?)newMap.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("Created_Date", newMap.CreatedDate);
        cmd.Parameters.AddWithValue("Modified_Date", newMap.ModifiedDate);
        cmd.Parameters.AddWithValue("Is_Square", (object?)newMap.IsSquare ?? DBNull.Value);

        int newId = (int)cmd.ExecuteScalar();

        return new Map(newId, newMap.Columns, newMap.Rows, newMap.Name, newMap.Description, newMap.CreatedDate, newMap.ModifiedDate, newMap.IsSquare);
    }

    public Map UpdateMap(Map updatedMap){
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand(
            "UPDATE maps SET columns = @columns, rows = @rows, \"Name\" = @name, \"description\" = @description, " +
            "\"modified_date\" = @modified_date, \"is_square\" = @is_square WHERE id = @id", conn);

        cmd.Parameters.AddWithValue("columns", updatedMap.Columns);
        cmd.Parameters.AddWithValue("rows", updatedMap.Rows);
        cmd.Parameters.AddWithValue("id", updatedMap.Id);
        cmd.Parameters.AddWithValue("Name", updatedMap.Name);
        cmd.Parameters.AddWithValue("description", (object?)updatedMap.Description ?? DBNull.Value); // Handle NULL values
        cmd.Parameters.AddWithValue("modified_date", DateTime.Now);  // Update modified date
        cmd.Parameters.AddWithValue("is_square", (object?)updatedMap.IsSquare ?? DBNull.Value);

        int rowsAffected = cmd.ExecuteNonQuery();
        return updatedMap;                                    // Return true if an entry was updated
    }

    public Map DeleteMap(Map toDelete){
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand("DELETE FROM maps WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", toDelete.Id);

        int rowsAffected = cmd.ExecuteNonQuery();
        return toDelete;                // Return true if a row was deleted
    }
}