using DevLife.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.MeetingEscapes.Queries;
public class GenerateExcuseQuery : IRequest<ExcuseDto>
{
    public MeetingType MeetingType { get; set; }
}
