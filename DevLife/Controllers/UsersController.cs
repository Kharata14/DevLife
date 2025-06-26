using DevLife.Application.Features.Authentication;
using DevLife.Application.Features.Authentication.Commands;
using DevLife.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LoginRequest = DevLife.Application.Features.Authentication.LoginRequest;

namespace DevLife.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISessionService _sessionService;
    #region constructor
    public UsersController(IMediator mediator, ISessionService sessionService)
    {
        _mediator = mediator;
        _sessionService = sessionService;
    }
    #endregion
    #region Register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
            var newUser = await _mediator.Send(command);
            await _sessionService.SignInAsync(newUser);
            return Ok(newUser);
    }
    #endregion
    #region Login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var user = await _mediator.Send(command);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username." });
        }
        await _sessionService.SignInAsync(user);
        return Ok(new { message = $"Welcome back, {user.FirstName}!" });
    }
    #endregion
    #region Logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new LogoutCommand());
        return Ok(new { message = "You have been successfully logged out." });
    }
    #endregion

}


