using DevLife.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Dating.Commands
{
    public class UpdateDatingProfileCommandHandler : IRequestHandler<UpdateDatingProfileCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateDatingProfileCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(UpdateDatingProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

            if (user != null)
            {
                user.Bio = request.Bio ?? user.Bio;
                user.Gender = request.Gender ?? user.Gender;
                user.InterestedIn = request.InterestedIn ?? user.InterestedIn;
                await _userRepository.UpdateUserAsync(user);
            }
        }
    }
}