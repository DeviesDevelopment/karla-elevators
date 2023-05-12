using KarlaTower.Models;
using KarlaTower.Services;
using Microsoft.AspNetCore.Mvc;

namespace KarlaTower.Controllers;

[ApiController]
[Route("[controller]")]
public class ElevatorsController : ControllerBase
{
    private readonly IElevatorService _elevatorService;

    public ElevatorsController(IElevatorService elevatorService)
    {
        _elevatorService = elevatorService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Elevator>> Get()
    {
        return Ok(_elevatorService.GetAllElevators());
    }
    
    [HttpGet("{id:int}")]
    public ActionResult<Elevator> Get([FromRoute] int id)
    {
        var elevator = _elevatorService.GetElevator(id); 
        return elevator != null ? Ok(elevator) : NotFound();
    }
    
    [HttpPost("{id:int}/send")]
    public ActionResult<Elevator> SendElevator([FromRoute] int id, [FromBody] int floor)
    {
        var elevator = _elevatorService.SendElevator(id, floor);
        return Ok(elevator);
    }
    
    [HttpPost("{id:int}/stop")]
    public ActionResult<Elevator> StopElevator([FromRoute] int id)
    {
        var elevator = _elevatorService.StopElevator(id);
        return Ok(elevator);
    }
    
    [HttpGet("order/{floor:int}")]
    public ActionResult<Elevator> OrderElevator([FromRoute] int floor)
    {
        var elevator = _elevatorService.OrderElevator(floor); 
        return elevator != null ? Ok(elevator) : NotFound();
    }
    
    [HttpPost("{id:int}/enter")]
    public ActionResult<Elevator> EnterElevator([FromRoute] int id, [FromBody] int floor)
    {
        var elevator = _elevatorService.EnterElevator(id, floor); 
        return elevator != null ? Ok(elevator) : NotFound();
    }
    
    [HttpPost("{id:int}/leave")]
    public ActionResult<Elevator> LeaveElevator([FromRoute] int id, [FromBody] int floor)
    {
        var elevator = _elevatorService.LeaveElevator(id, floor); 
        return elevator != null ? Ok(elevator) : NotFound();
    }
}