using Application.Services;
using FluentValidation;
using Application;
using JwtProvider;
using Infrastructure;
using PasswordHashProvider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

builder.Services.AddJwtProvider();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPasswordHashProvider();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddApplication();

// Позже прописать валидацию тут

builder.Services.Configure<AuthSettings>(
    builder.Configuration.GetSection("AuthSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
