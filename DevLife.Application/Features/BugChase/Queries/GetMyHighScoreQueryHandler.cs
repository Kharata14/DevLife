using DevLife.Application.Features.BugChase.Dtos;
using DevLife.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.BugChase.Queries;
public class GetMyHighScoreQueryHandler : IRequestHandler<GetMyHighScoreQuery, MyHighScoreDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGameScoreRepository _scoreRepository;

    public GetMyHighScoreQueryHandler(IHttpContextAccessor httpContextAccessor, IGameScoreRepository scoreRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _scoreRepository = scoreRepository;
    }
    public async Task<MyHighScoreDto> Handle(GetMyHighScoreQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        var highScore = await _scoreRepository.GetHighScoreForUserAsync(userId);

        return new MyHighScoreDto { HighScore = highScore };
    }
}
