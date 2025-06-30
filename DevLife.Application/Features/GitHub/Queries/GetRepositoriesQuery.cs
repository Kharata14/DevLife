using DevLife.Application.Features.GitHub.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.GitHub.Queries
{
    public class GetRepositoriesQuery : IRequest<List<GitHubRepoDto>> { }
}
