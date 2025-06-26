using DevLife.Application.Features.CodeCasino.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces;
public interface ICasinoChallengeGenerator
{
    Task<CasinoChallengeDto> GenerateChallengeAsync(string language, string experienceLevel);
}