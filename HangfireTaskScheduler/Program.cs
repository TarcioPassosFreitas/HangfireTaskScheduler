using Hangfire;
using HangfireTaskScheduler.API.Mappers;
using HangfireTaskScheduler.Core.Interfaces.Repository;
using HangfireTaskScheduler.Core.Interfaces.Service;
using HangfireTaskScheduler.Core.Services;
using HangfireTaskScheduler.Infraestructure.Context;
using HangfireTaskScheduler.Infraestructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

builder.Services.Configure<HangfireTaskScheduler.Core.Configuration.JwtConfiguration>(builder.Configuration.GetSection("JWT"));

// Add Hangfire services.
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireTaskSchedulerDB")));
builder.Services.AddHangfireServer();

// Add MailJet services.
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

builder.Services.Configure<HangfireTaskScheduler.Core.Configuration.MailJetConfiguration>(
    builder.Configuration.GetSection("MailJet"));

// Add DbContext services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Configure dependency injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


//AutoMapper
builder.Services.AddAutoMapper(typeof(UserMapper));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use Hangfire Dashboard and server.
app.UseHangfireDashboard();

app.UseAuthorization();

app.MapControllers();

app.Run();