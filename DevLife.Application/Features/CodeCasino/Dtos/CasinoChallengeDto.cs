using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeCasino.Dtos;
public class CasinoChallengeDto
{
    public string Description { get; set; }
    public List<CodeSnippetDto> Snippets { get; set; }
}

public class CodeSnippetDto
{
    public string Id { get; set; }
    public string Code { get; set; }
    public bool IsCorrect { get; set; }
    public string Explanation { get; set; }
}