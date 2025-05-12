using Npgsql;
using robot_controller_api.Persistence;
using task3._2.Models;

public class RobotCommandRepository : IRobotCommandDataAccess, IRepository {
    private IRepository _repo => this;

    public List<RobotCommand> GetRobotCommands(){
        var commands = _repo.ExecuteReader<RobotCommand>("SELECT * FROM robot_commands");
        return commands;
    }

    public RobotCommand AddRobotCommand(RobotCommand newCommand){
        var sqlParams = new NpgsqlParameter[]{
            new("Name", newCommand.Name),
            new("description", newCommand.Description ?? (object)DBNull.Value),
            new("is_move_command", newCommand.IsMoveCommand),
            new("created_date", newCommand.CreatedDate),
            new("modified_date", newCommand.ModifiedDate)
        };

        var result = _repo.ExecuteReader<RobotCommand>(
            "INSERT INTO robot_commands (\"Name\", description, is_move_command, created_date, modified_date) " +
            "VALUES (@Name, @description, @is_move_command, @created_date, @modified_date) RETURNING *;",
            sqlParams).Single();

        return result;
    }

    public RobotCommand UpdateRobotCommand(RobotCommand updatedCommand){
        var sqlParams = new NpgsqlParameter[]{
            new("id", updatedCommand.Id),
            new("Name", updatedCommand.Name),
            new("description", updatedCommand.Description ?? (object)DBNull.Value),
            new("is_move_command", updatedCommand.IsMoveCommand)
        };

        // It can return the whole entity RETURNING * 
        // or just a particular field RETURNING id that can then be used in the application layer if needed.
        var result = _repo.ExecuteReader<RobotCommand>(
            "UPDATE robot_commands SET \"Name\"=@Name, description=@description, " +
            "is_move_command=@is_move_command, modified_date=current_timestamp AT TIME ZONE 'UTC' WHERE id=@id RETURNING *;", 
            sqlParams).Single();

        return result;
    }

    public RobotCommand DeleteRobotCommand(RobotCommand toDelete){
        var sqlParams = new NpgsqlParameter[]{
            new("id", toDelete.Id)
        };

        var result = _repo.ExecuteReader<RobotCommand>(
            "DELETE FROM robot_commands WHERE id = @id RETURNING *;",
            sqlParams).Single();

        return result;
    }
}