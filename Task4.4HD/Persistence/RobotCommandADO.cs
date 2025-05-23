using Npgsql;
namespace robot_controller_api.Persistence;
using task3._2.Models;
public class RobotCommandADO : IRobotCommandDataAccess{
    // Connection string is usually set in a config file for the ease of change. 
    private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=password;Database=sit331";

    public List<RobotCommand> GetRobotCommands(){
        var robotCommands = new List<RobotCommand>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT * FROM robot_commands", conn);
        using var dr = cmd.ExecuteReader();
        while (dr.Read()){ //read values off the data reader and create a new robotCommand here and then add it to the result list. 
            var id = dr.GetInt32(0);
            var name = dr.GetString(1);
            var descr = dr.IsDBNull(2) ? null : dr.GetString(2);
            var isMove = dr.GetBoolean(3);
            var createdDate = dr.GetDateTime(4);
            var modDate = dr.GetDateTime(5);

            RobotCommand robotCommand = new RobotCommand(id, name, descr, isMove, createdDate, modDate);
            robotCommands.Add(robotCommand);
        }
        return robotCommands;
    }

    public RobotCommand AddRobotCommand(RobotCommand newCommand){
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand(
            "INSERT INTO robot_commands (\"Name\", description, is_move_command, created_date, modified_date) " +
            "VALUES (@Name, @description, @is_move_command, @created_date, @modified_date) RETURNING id", conn);

        //cmd.Parameters.AddWithValue("id", newCommand.Id); //Autogenerated
        cmd.Parameters.AddWithValue("Name", newCommand.Name);
        cmd.Parameters.AddWithValue("description", (object?)newCommand.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("ismovecommand", newCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("created_date", newCommand.CreatedDate);
        cmd.Parameters.AddWithValue("modified_date", newCommand.ModifiedDate);

        int newId = (int)cmd.ExecuteScalar(); // Get the new ID

        return new RobotCommand(newId, newCommand.Name, newCommand.Description, newCommand.IsMoveCommand,
                                newCommand.CreatedDate, newCommand.ModifiedDate);
    }
    public RobotCommand UpdateRobotCommand(RobotCommand updatedCommand){
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand(
            "UPDATE robot_commands SET \"Name\" = @name, \"description\" = @description, \"is_move_command\" = @is_move_command, " +
            "\"modified_date\" = @modified_date WHERE id = @id", conn);

        cmd.Parameters.AddWithValue("id", updatedCommand.Id);
        cmd.Parameters.AddWithValue("Name", updatedCommand.Name);
        cmd.Parameters.AddWithValue("description", (object?)updatedCommand.Description ?? DBNull.Value); // Handle NULL values
        cmd.Parameters.AddWithValue("is_move_command", updatedCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("modified_date", DateTime.Now);  // Update modified date

        int rowsAffected = cmd.ExecuteNonQuery();
        return updatedCommand;                                    // Return true if an entry was updated
    }

    public RobotCommand DeleteRobotCommand(RobotCommand toDelete){
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand("DELETE FROM robot_commands WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", toDelete.Id);

        int rowsAffected = cmd.ExecuteNonQuery();
        return toDelete;                // Return true if a row was deleted
    }
}