using DevLife.Application.Features.MeetingEscapes.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.MeetingEscapes.Commands;
public class SaveFavoriteExcuseCommand : IRequest<Unit>
{
    public ExcuseDto Excuse { get; set; }
}