using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Tutorial_DotNet.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase {
    private readonly IAuthRepository _authRepository;
    
    public AuthController(IAuthRepository authRepository) {
        _authRepository = authRepository;
    }
    
    [HttpPost("register")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(int))]
    public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterRequestDto userRegisterRequestDto) {
        var response = await _authRepository.Register(new User{ Username = userRegisterRequestDto.Username },
            userRegisterRequestDto.Password);
        if (!response.Success)
            return BadRequest(response);
        return Ok(response);
    }
    
    [HttpPost("login")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginRequestDto userLoginRequestDto) {
        var response = await _authRepository.Login(userLoginRequestDto.Username, userLoginRequestDto.Password);
        if (!response.Success)
            return BadRequest(response);
        return Ok(response);
    }
}