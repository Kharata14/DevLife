using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Dating.Queries
{
    public class GetPotentialMatchesQuery : IRequest<List<DatingProfileDto>> { }

}
