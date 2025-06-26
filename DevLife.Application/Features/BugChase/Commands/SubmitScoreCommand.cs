using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.BugChase.Commands;
public class SubmitScoreCommand : IRequest
{
    public int Score { get; set; }
}