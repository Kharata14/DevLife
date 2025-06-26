using DevLife.Application.Features.BugChase.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.BugChase.Queries;
public class GetLeaderboardQuery : IRequest<List<LeaderboardEntryDto>> 
{
}