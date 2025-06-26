using DevLife.Application.Features.CodeCasino.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeCasino.Queries;
 public class GetCasinoChallengeQuery : IRequest<PublicCasinoChallengeDto>
{
}