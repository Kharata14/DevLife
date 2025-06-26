using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace DevLife.Infrastructure.Services;

public class CookieAuthenticationService : ISessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SignInAsync(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("FirstName", user.FirstName),
                new Claim("ZodiacSign", user.ZodiacSign.ToString())
            };

        var claimsIdentity = new ClaimsIdentity(claims, "DevLifeCookie");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await _httpContextAccessor.HttpContext.SignInAsync("DevLifeCookie", claimsPrincipal);
    }

    public async Task SignOutAsync()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync("DevLifeCookie");
    }
}
