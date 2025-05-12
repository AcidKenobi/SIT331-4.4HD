using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;
using task3._2.Models;

[Route("api/[controller]")]
[ApiController]
public class MapsController : ControllerBase {
    private readonly IMapDataAccess _mapsRepo;
    public MapsController(IMapDataAccess mapsRepo){
        _mapsRepo = mapsRepo;
    }

    /// <summary> Get all maps </summary>
    /// <returns> List of maps </returns>
    /// <response code = "200"> Returns list of all maps </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet()]
    public IEnumerable<Map> GetAllMaps(){
        return _mapsRepo.GetMaps();
    }

    /// <summary> Get all maps that are square</summary>
    /// <returns> List of maps that are square</returns>
    /// <response code = "200"> Returns list of all maps that are square</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("square")]
    public IEnumerable<Map> GetAllSquareMaps(){
        return _mapsRepo.GetMaps().Where(m => m.Columns == m.Rows);
    }

    /// <summary> Get map by Id </summary>
    /// <param name="id"> Unique identifier </param>
    /// <returns> Map with respective Id </returns>
    /// <response code = "200"> Map with respective Id </response>
    /// <response code = "404"> Map with ID not found </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public IActionResult GetMapById(int id){
        var m = _mapsRepo.GetMaps().Find(m => m.Id == id);
        if(m == null){
            return NotFound($"Map with Id: {id} invalid.");
        }
        return Ok(m);
    }

    /// <summary> Create a new map </summary>
    /// <param name="newMap"> A new map from the HTTP request </param>
    /// <returns> Newly created map </returns>
    /// <remarks> <para>
    /// Sample request:
    /// 
    /// POST /api/maps {
    ///     "id": 0,
    ///     "columns": 0,
    ///     "rows": 0,
    ///     "name": "string",
    ///     "description": "string",
    ///     "createdDate": "2025-04-18T04:33:04.088Z",
    ///     "modifiedDate": "2025-04-18T04:33:04.088Z"
    /// }
    /// 
    /// </para> </remarks>
    /// <response code = "201"> Returns the newly created map </response>
    /// <response code="400"> If the map is null </response> 
    /// <response code="409"> If a map with the same name already exists. </response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public IActionResult AddMap(Map newMap){
        if(newMap == null){
            return BadRequest("Map is null");
        }
        if(_mapsRepo.GetMaps().Any(m => m.Name == newMap.Name)){
            return Conflict($"Map with Name: '{newMap.Name}' already exists");
        }
        var m = _mapsRepo.AddMap(newMap);
        return Created();
    }

    /// <summary> Update a robot command by Id. </summary>
    /// <param name="id"> Unique identifier </param>
    /// <param name="updatedMap"> An updated map from the HTTP request </param>
    /// <returns> No content </returns>
    /// <remarks> <para>
    /// Sample request:
    /// 
    /// PUT /api/maps/{id}{
    ///     "columns": 10,
    ///     "rows": 10,
    ///     "name": "string1",
    ///     "description": "string3",
    ///     "modifiedDate": "2025-03-16T04:49:36.235Z"
    /// }
    /// </para> </remarks>
    /// <response code = "204"> If map updated successfully </response>
    /// <response code = "400"> Request contains invalid or null data </response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public IActionResult UpdateMap(int id, Map updatedMap){
        var m = _mapsRepo.GetMaps().Find(m => m.Id == id);
        if(m == null){
            return NotFound($"Map with Id:{id} not found.");
        }
        
        m.Columns = updatedMap.Columns;
        m.Rows = updatedMap.Rows;
        m.Name = updatedMap.Name;
        m.Description = updatedMap.Description;
        m.ModifiedDate = DateTime.Now;

        var result = _mapsRepo.UpdateMap(m);

        if(result == null){
            return NotFound("No Map found");
        }
        return NoContent();
    }

    /// <summary> Delete a map by Id </summary>
    /// <param name="id"> Unique identifier </param>
    /// <returns> No content </returns>
    /// <response code = "204"> If map deleted successfully </response>
    /// <response code = "404"> If map with corresponding Id not found </response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public IActionResult DeleteMap(int id){
        var m = _mapsRepo.GetMaps().Find(m => m.Id == id);
        if(m == null){
            return NotFound($"Entry with id: {id} not found.");
        }

        var result = _mapsRepo.DeleteMap(m);
        if(result == null){
            return NotFound("No Map found");
        }
        return NoContent();
    }

    /// <summary> Check if coordinate bounds are contained within a map </summary>
    /// <param name="id"> Unique identifier </param>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <returns> true if within map, false if outside of map bounds </returns>
    /// <response code = "200"> If request successful </response>
    /// <response code = "400"> If request is invalid or null </response>
    /// <response code = "404"> If map with corresponding Id not found </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}/{x}-{y}")] 
    public IActionResult CheckCoordinate(int id, int x, int y) { 
        var m = _mapsRepo.GetMaps().Find(m => m.Id == id);
        if(m == null){
            return NotFound($"Map with Id:{id} not found.");
        }
        if(x <= 1 || y <= 1){
            return BadRequest("Map size outside of bounds.");
        }

        bool isOnMap = false; 
        if(x >= 2 && x <= m.Columns && y >= 2 && y <= m.Rows){
            isOnMap = true;
        }
        
        return Ok(isOnMap); 
    } 
}