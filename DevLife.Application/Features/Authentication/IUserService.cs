using DevLife.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Authentication;
public interface IUserService
{
    Task<User> RegisterUserAsync(RegistrationRequest request);
}
