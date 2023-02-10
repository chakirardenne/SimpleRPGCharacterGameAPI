using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tutorial_DotNet.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase {
    
    private ICharacterService _characterService;
    public CharacterController(ICharacterService characterService) {
        _characterService = characterService;
    }

    [AllowAnonymous]
    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<GetCharacterResponseDto>)) ]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDto>>>> GetAll() {
        return Ok(await _characterService.GetAllCharacters());
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(GetCharacterResponseDto)) ]
    [ProducesResponseType(StatusCodes.Status404NotFound )]
    public async Task<ActionResult<ServiceResponse<GetCharacterResponseDto>>> GetSingle(int id ) {
        return Ok(await _characterService.GetCharacterById(id));
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCharacterResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDto>>>> AddCharacter(AddCharacterRequestDto character) {
        return Ok(await _characterService.AddCharacter(character));
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(GetCharacterResponseDto)) ]
    [ProducesResponseType(StatusCodes.Status404NotFound )]
    public async Task<ActionResult<ServiceResponse<GetCharacterResponseDto>>> UpdateCharacter(UpdateCharacterDtoRequest updateCharacterDtoRequest) {
        var response = await _characterService.UpdateCharacter(updateCharacterDtoRequest);
        if (response.Data is null) {
            return NotFound(response);
        }
        return Ok(response);
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(bool)) ]
    [ProducesResponseType(StatusCodes.Status404NotFound )]
    public async Task<ActionResult<ServiceResponse<bool>>> DeleteCharacter(int id) {
        var response = await _characterService.DeleteCharacter(id);
        if (!response.Data) {
            return NotFound(response);
        }
        return Ok(response);
    }
}