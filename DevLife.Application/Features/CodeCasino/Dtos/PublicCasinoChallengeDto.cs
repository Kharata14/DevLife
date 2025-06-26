using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeCasino.Dtos;
public class PublicCasinoChallengeDto
{
    public Guid ChallengeId { get; set; }
    public string Description { get; set; }
    public List<PublicCodeSnippetDto> Snippets { get; set; }
}

public class PublicCodeSnippetDto
{
    public string Id { get; set; }
    public string Code { get; set; }
}