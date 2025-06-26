using DevLife.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Dashboard.Queries;

public class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, DashboardResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IHoroscopeService _horoscopeService;

    public GetDashboardDataQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IHoroscopeService horoscopeService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _horoscopeService = horoscopeService;
    }

    public async Task<DashboardResponse> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var userIdString = userIdClaim?.Value;

        if (!Guid.TryParse(userIdString, out var userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated correctly.");
        }

        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException("User associated with this session not found in the database.");
        }

        string dailyHoroscope = await _horoscopeService.GetDailyHoroscopeAsync(user.ZodiacSign, cancellationToken);

        string personalizedAdvice = GetPersonalizedAdvice();
        string luckyTechnology = GetLuckyTechnology();

        var response = new DashboardResponse
        {
            WelcomeMessage = $"გამარჯობა {user.FirstName}! ♏ [{user.ZodiacSign}], დღეს {personalizedAdvice}",
            DailyHoroscope = dailyHoroscope,
            LuckyTechnology = luckyTechnology,
            CurrentPoints = user.Points
        };

        return response;
    }

    private string GetPersonalizedAdvice()
    {
        string[] advices = { "დალიე ყავა და დაწერე კოდი!", "დროა, დახურო Stack Overflow და ენდო საკუთარ თავს.", "გახსოვდეს, ; (წერტილ-მძიმე) შენი მეგობარია." };
        return advices[new Random().Next(advices.Length)];
    }

    private string GetLuckyTechnology()
    {
        string[] techs = { "Rust", "Go", "C# 13", "React", "HTMX", "Blazor", "Kubernetes" };
        return techs[new Random().Next(techs.Length)];
    }
}