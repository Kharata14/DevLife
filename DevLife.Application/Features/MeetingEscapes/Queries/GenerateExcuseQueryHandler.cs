using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using DevLife.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.MeetingEscapes.Queries;
public class GenerateExcuseQueryHandler : IRequestHandler<GenerateExcuseQuery, ExcuseDto>
{
    private readonly IExcuseLogRepository _logRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IExcuseTemplateRepository _templateRepository;
    private static readonly Random _random = new Random();

    public GenerateExcuseQueryHandler(
        IExcuseLogRepository logRepository,
        IHttpContextAccessor httpContextAccessor,
        IExcuseTemplateRepository templateRepository)
    {
        _logRepository = logRepository;
        _httpContextAccessor = httpContextAccessor;
        _templateRepository = templateRepository;
    }

    public async Task<ExcuseDto> Handle(GenerateExcuseQuery request, CancellationToken cancellationToken)
    {
        string templateCategory = GetTemplateCategory(request.MeetingType);

        string text = await _templateRepository.GetRandomTemplateAsync(templateCategory, cancellationToken);
 
        var userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
 
        Guid.TryParse(userIdString, out var userId);
 
        var logEntry = new ExcuseLog
        {
            Id = Guid.NewGuid(),
            ExcuseText = text,
            Category = templateCategory,
            UserId = userId
        };
        await _logRepository.LogAsync(logEntry, cancellationToken);

        var excuseDto = new ExcuseDto
        {
            Text = text,
            Category = templateCategory,
            BelievabilityScore = Math.Round(_random.NextDouble() * (99.9 - 10.0) + 10.0, 1)
        };

        return excuseDto;
    }

    private string GetTemplateCategory(MeetingType meetingType)
    {
        return meetingType switch
        {
            MeetingType.DailyStandup => "Technical",
            MeetingType.SprintPlanning => "Personal",
            MeetingType.ClientMeeting => "Creative",
            _ => "Personal"
        };
    }

    private (string text, string type) GenerateRandomExcuse(MeetingType meetingType)
    {
        var technicalExcuses = new[] { "სერვერს ცეცხლი გაუჩნდა.", "ჩემი IDE-ს ლიცენზიას ვადა გაუვიდა და არ მიშვებს.", "Production-ის ბაზა შემთხვევით წავშალე და აღდგენაზე ვმუშაობ." };
        var personalExcuses = new[] { "კატამ კლავიატურაზე გადაიარა და production-ში დააფუშა.", "ყავის აპარატი გაფუჭდა, სასწრაფოდ ხელოსანი ვარ.", "ინტერნეტის კაბელი მომიპარეს." };
        var creativeExcuses = new[] { "ხელოვნურმა ინტელექტმა ცნობიერება შეიძინა და დახმარებას მთხოვს.", "ჩემს რეპოზიტორიში დროში მოგზაური გამოჩნდა და მომავლის კოდს მაჩვენებს.", "Red Bull-მა ფრთები მართლა შემასხა." };

        return meetingType switch
        {
            MeetingType.DailyStandup => (technicalExcuses[_random.Next(technicalExcuses.Length)], "Technical"),
            MeetingType.SprintPlanning => (personalExcuses[_random.Next(personalExcuses.Length)], "Personal"),
            MeetingType.ClientMeeting => (creativeExcuses[_random.Next(creativeExcuses.Length)], "Creative"),
            MeetingType.TeamBuilding => ("სოციალური ბატარეა დამიჯდა, დამუხტვა მჭირდება.", "Personal"),
            _ => ("რაღაც გაუთვალისწინებელი მოხდა.", "Generic")
        };
    }
}