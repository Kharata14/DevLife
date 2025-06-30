using DevLife.Domain.Enums;
using MediatR;

namespace DevLife.Application.Features.Dating.Commands
{
    public class UpdateDatingProfileCommand : IRequest
    {
        public string? Bio { get; set; }
        public Gender? Gender { get; set; }
        public Interest? InterestedIn { get; set; }
    }
}