using DevLife.Application.Features.CodeCasino.Dtos;
using DevLife.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeCasino.Queries;
public class GetCasinoLeaderboardQueryHandler : IRequestHandler<GetCasinoLeaderboardQuery, List<CasinoLeaderboardEntryDto>>
{
    private readonly IUserRepository _userRepository;

    public GetCasinoLeaderboardQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<CasinoLeaderboardEntryDto>> Handle(GetCasinoLeaderboardQuery request, CancellationToken cancellationToken)
    {
        var topUsers = await _userRepository.GetTopUsersByPointsAsync(10);
        var leaderboardDto = topUsers.Select(user => new CasinoLeaderboardEntryDto
        {
            Username = user.Username,
            Points = user.Points,
            LongestStreak = user.LongestStreak
        }).ToList();

        return leaderboardDto;
    }
}