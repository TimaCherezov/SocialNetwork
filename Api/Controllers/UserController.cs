using Application.Abstractions;
using Application.DTO;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IRegisterUserService registerUserService,
    ILoginUserService loginUserService,
    IRefreshTokenService refreshTokenService)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var response = await registerUserService.RegisterAsync(request, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var response = await loginUserService.LoginAsync(request, cancellationToken);
        HttpContext.Response.Cookies.Append("just_cookie", response.AccessToken);
        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenPairResponse>> Refresh([FromBody] RefreshRequest request, CancellationToken cancellationToken)
    {
        var result = await refreshTokenService.RefreshAsync(request.RefreshToken, cancellationToken);
        return Ok(result);
    }

}

