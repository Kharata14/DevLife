using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeCasino.Dtos;
public class CachedCasinoChallengeDto
{
    public Guid ChallengeId { get; set; }
    public string Description { get; set; }
    public List<CodeSnippetDto> Snippets { get; set; }
}