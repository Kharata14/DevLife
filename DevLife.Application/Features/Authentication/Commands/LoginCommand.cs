using DevLife.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Authentication.Commands;
 public class LoginCommand : IRequest<User>
{
    public string Username { get; set; }
}
