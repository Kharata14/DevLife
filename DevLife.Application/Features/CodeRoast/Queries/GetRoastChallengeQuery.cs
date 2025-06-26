using DevLife.Application.Features.CodeRoast.Dtos;
using MediatR;

namespace DevLife.Application.Features.CodeRoast.Queries
{
    public class GetRoastChallengeQuery : IRequest<CodingChallengeDto>
    {
        public string Difficulty { get; set; }
    }
}