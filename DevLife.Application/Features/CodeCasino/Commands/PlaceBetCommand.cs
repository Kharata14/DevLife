using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeCasino.Commands;
public class PlaceBetCommand : IRequest<PlaceBetResultDto>
{
    public Guid ChallengeId { get; set; }
    public string ChosenSnippetId { get; set; }
    public int BetAmount { get; set; }
}