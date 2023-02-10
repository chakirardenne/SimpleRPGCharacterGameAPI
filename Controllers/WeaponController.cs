using System.Net.Mime;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tutorial_DotNet.Services;

namespace Tutorial_DotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeaponController : ControllerBase {
    private readonly IWeaponService _weaponService;
    
    public WeaponController(IWeaponService weaponService) {
        _weaponService = weaponService;
    }
    
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCharacterResponseDto))]
    public async Task<ActionResult<ServiceResponse<GetCharacterResponseDto>>> AddWeapon(AddWeaponRequestDto addWeaponRequestDto) {
        return Ok(await _weaponService.AddWeapon(addWeaponRequestDto));
    }
}