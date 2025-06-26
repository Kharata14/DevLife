using DevLife.Api.Middleware;
using DevLife.Application.Common.Behaviors;
using DevLife.Application.Features.Authentication;
using DevLife.Application.Interfaces;
using DevLife.Infrastructure.Persistence;
using DevLife.Infrastructure.Repositories;
using DevLife.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(postgresConnectionString));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("MongoConnection");
    return new MongoClient(connectionString);
});
builder.Services.AddScoped<IExcuseTemplateRepository, MongoExcuseTemplateRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IExcuseLogRepository, ExcuseLogRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "DevLife_";
});

builder.Services.AddScoped<IHoroscopeService, OpenAiHoroscopeService>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(DevLife.Application.AssemblyReference).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(DevLife.Application.AssemblyReference).Assembly);
builder.Services.AddScoped<ISessionService, CookieAuthenticationService>();
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddAuthentication("DevLifeCookie")
    .AddCookie("DevLifeCookie", options =>
    {
        options.Cookie.Name = "DevLifeCookie";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "http://localhost:4200", "http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();