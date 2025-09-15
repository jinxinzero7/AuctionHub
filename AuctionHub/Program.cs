using Application.Services;
using Domain.Interfaces;
using Domain.Interfaces.Jwt;
using Domain.Interfaces.PassHasher;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Services;
using JwtProvider;
using Microsoft.EntityFrameworkCore;
using PasswordHashProvider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

builder.Services.AddJwtProvider();
builder.Services.AddInfrastructure();
builder.Services.AddPasswordHashProvider();


builder.Services.Configure<AuthSettings>(
    builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddAuth(builder.Configuration);

builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
