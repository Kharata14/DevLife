using DevLife.Application.Features.BugChase.Dtos;
using DevLife.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.BugChase.Queries;
public class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardQuery, List<LeaderboardEntryDto>>
{
    private readonly IGameScoreRepository _scoreRepository;
    public GetLeaderboardQueryHandler(IGameScoreRepository scoreRepository)
        => _scoreRepository = scoreRepository;

    public async Task<List<LeaderboardEntryDto>> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        return await _scoreRepository.GetLeaderboardAsync();
    }
}