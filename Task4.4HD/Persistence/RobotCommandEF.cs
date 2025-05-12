using robot_controller_api.Persistence;
using task3._2.Models;
using task3._2.Persistence;

public class RobotCommandEF : IRobotCommandDataAccess
{
    private readonly RobotContext _context;

    public RobotCommandEF(RobotContext context)
    {
        _context = context;
    }

    public List<RobotCommand> GetRobotCommands()
    {
        return _context.RobotCommands.ToList();
    }

    public RobotCommand AddRobotCommand(RobotCommand newCommand)
    {
        _context.RobotCommands.Add(newCommand);
        _context.SaveChanges();
        return newCommand;
    }

    public RobotCommand UpdateRobotCommand(RobotCommand updatedCommand)
    {
        var toUpdate = _context.RobotCommands.Find(updatedCommand.Id);
        if (toUpdate == null)
        {
            return null;
        }

        _context.Entry(toUpdate).CurrentValues.SetValues(updatedCommand);
        toUpdate.ModifiedDate = DateTime.UtcNow;

        _context.SaveChanges();
        return toUpdate;
    }

    public RobotCommand DeleteRobotCommand(RobotCommand toDelete)
    {
        var comm = _context.RobotCommands.Find(toDelete.Id);
        if (comm == null)
        {
            return null;
        }

        _context.RobotCommands.Remove(comm);
        _context.SaveChanges();
        return comm;
    }
}