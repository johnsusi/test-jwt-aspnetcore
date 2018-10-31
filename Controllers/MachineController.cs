using System;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using test_jwt;

namespace Controllers
{

  [Route("/machine")]
  [Authorize]
  public class MachineController : Controller
  {
    private readonly IAuthorizationService _authorizationService;
    private readonly IMachineRepository _machineRepository;

    public MachineController(IAuthorizationService authorizationService, IMachineRepository machineRepository)
    {
      _authorizationService = authorizationService
        ?? throw new ArgumentNullException(nameof(authorizationService));

      _machineRepository = machineRepository
        ?? throw new ArgumentNullException(nameof(machineRepository));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MachineDto>> GetMachine(int id)
    {
      var machine = _machineRepository.GetById(id);
      if (machine == null) return NotFound();

        var authorizationResult = await _authorizationService
            .AuthorizeAsync(User, machine, "HasAccessToMachine");

        if (authorizationResult.Succeeded)
        {
          return Ok(machine);
        }
        else
        {
          return Forbid();
        }
    }

  }

}