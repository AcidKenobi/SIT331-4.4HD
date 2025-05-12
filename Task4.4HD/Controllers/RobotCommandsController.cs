using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;
using task3._2.Models;

[Route("api/[controller]")]
[ApiController]
public class RobotCommandsController : ControllerBase {
    private readonly IRobotCommandDataAccess _robotCommandsRepo;
    public RobotCommandsController(IRobotCommandDataAccess robotCommandsRepo){
        _robotCommandsRepo = robotCommandsRepo;   
    }

    /// <summary> Get all robot commands </summary>
    /// <returns> List of robot commands </returns>
    /// <response code = "200"> Returns list of all robot commands </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet()] 
    public IEnumerable<RobotCommand> GetAllRobotCommands(){
        return _robotCommandsRepo.GetRobotCommands();
    }

    /// <summary> Get all robot commands that are move functions </summary>
    /// <returns>List of robot commands that are move functions </returns>
    /// <response code = "200"> Returns list of robot commands that are move functions </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("move")] 
    public IEnumerable<RobotCommand> GetMoveCommandsOnly() { 
        var comms = _robotCommandsRepo.GetRobotCommands();
        return comms.Where(comms => comms.IsMoveCommand);
    } 

    /// <summary> Get a robot command by Id </summary>
    /// <param name="id"> Unique identifier </param>
    /// <returns> Robot command with respective Id </returns>
    /// <response code = "200"> Robot command with respective Id </response>
    /// <response code = "404"> Robot command with ID not found </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = "GetRobotCommand")] 
    public IActionResult GetRobotCommandById(int id) { 
        var c = _robotCommandsRepo.GetRobotCommands().Find(c => c.Id == id);
        if(c == null){
            return NotFound($"Command with Id: {id} Invalid.");
        }
        return Ok(c); 
    } 


    /// <summary> Create a new robot command </summary>
    /// <param name="newCommand"> A new robot command from the HTTP request.</param>
    /// <returns>Newly created robot command</returns>
    /// <remarks> <para>
    /// Sample request:
    /// 
    /// POST /api/robot-commands {
    ///     "name": "testing2",
    ///     "description": "special",
    ///     "isMoveCommand": false,
    ///     "createdDate": "2025-03-16T04:03:30.362Z",
    ///     "modifiedDate": "2025-03-16T04:03:30.362Z"
    /// }
    /// 
    /// </para> </remarks>
    /// <response code = "201"> Returns the newly created robot command </response>
    /// <response code="400"> If the robot command is null </response> 
    /// <response code="409"> If a robot command with the same name already exists. </response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost()] 
    public IActionResult AddRobotCommand(RobotCommand newCommand) { 
        if(newCommand == null){
            return BadRequest("Command is null");
        }
        if(_robotCommandsRepo.GetRobotCommands().Any(c => c.Name == newCommand.Name)){
            return Conflict($"Command with name: '{newCommand.Name}' already exists.");
        }
        var c = _robotCommandsRepo.AddRobotCommand(newCommand);
        return CreatedAtRoute("GetRobotCommand", new {id = c.Id}, c);
    } 

    /// <summary> Update a robot command by Id. </summary>
    /// <param name="id"> Unique identifier </param>
    /// <param name="updatedCommand"> An updated robot command from the HTTP request </param>
    /// <returns> No content </returns>
    /// <remarks> <para>
    /// Sample request:
    /// 
    /// PUT /api/robot-commands/{id}{
    ///     "name": "To update",
    ///     "description": "New description",
    ///     "isMoveCommand": true
    /// }
    /// </para> </remarks>
    /// <response code = "204"> If robot command updated successfully </response>
    /// <response code = "400"> Request contains invalid or null data </response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")] 
    public IActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand) {
        var c = _robotCommandsRepo.GetRobotCommands().Find(c => c.Id == id);
        if (c == null) {
            return BadRequest("Invalid command data.");
        }

        c.Name = updatedCommand.Name;
        c.Description = updatedCommand.Description;
        c.IsMoveCommand = updatedCommand.IsMoveCommand;

        var result = _robotCommandsRepo.UpdateRobotCommand(c);   
        
        if(result == null){
            return NotFound($"No command found");
        }
        return NoContent();
    } 

    /// <summary> Delete a robot command by Id </summary>
    /// <param name="id"> Unique identifier </param>
    /// <returns> No content </returns>
    /// <response code = "204"> If robot command deleted successfully </response>
    /// <response code = "404"> If robot command with corresponding Id not found </response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")] 
    public IActionResult DeleteRobotCommand(int id) { 
        var c = _robotCommandsRepo.GetRobotCommands().Find(c => c.Id == id);
        if(c == null){
            return NotFound($"Entry with id: {id} not found.");
        }

        var result = _robotCommandsRepo.DeleteRobotCommand(c);
        if(result == null){
            return NotFound($"No command found");
        }
        return NoContent();
    }
}