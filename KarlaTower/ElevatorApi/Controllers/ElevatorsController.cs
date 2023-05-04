using KarlaTower.Models;
using KarlaTower.Services;
using Microsoft.AspNetCore.Mvc;

namespace KarlaTower.Controllers;

[ApiController]
[Route("[controller]")]
public class ElevatorsController : ControllerBase
{
    private readonly ILogger<ElevatorsController> _logger;
    private readonly IElevatorService _elevatorService;

    public ElevatorsController(ILogger<ElevatorsController> logger, IElevatorService elevatorService)
    {
        _logger = logger;
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
}