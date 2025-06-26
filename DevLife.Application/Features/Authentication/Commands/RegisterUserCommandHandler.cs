using DevLife.Application.Common;
using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Authentication.Commands;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(request.Username, cancellationToken);
        if (existingUser != null)
        {
            throw new ArgumentException("User with this username already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            TechStack = request.TechStack,
            ExperienceLevel = request.ExperienceLevel,
            ZodiacSign = ZodiacCalculator.Calculate(request.DateOfBirth),
            Points = 50
        };

        await _userRepository.AddUserAsync(user, cancellationToken);

        return user;
    }
}
