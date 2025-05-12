namespace robot_controller_api.Persistence;

using task3._2.Models;
public interface IRobotCommandDataAccess {
    List<RobotCommand> GetRobotCommands();
    RobotCommand AddRobotCommand(RobotCommand newCommand);
    RobotCommand UpdateRobotCommand(RobotCommand updatedCommand);    
    RobotCommand DeleteRobotCommand(RobotCommand toDelete);           
}