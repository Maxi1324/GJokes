using GStatsFaker;
using GStatsFaker.DBContexts;
using GStatsFaker.Repository;
using GStatsFaker.Repository.Implementations;
using GStatsFaker.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Cors;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IJwtAuthenticationManager,JwtAuthenticationManager>();
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IConfigRepo, ConfigRepo>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IContRepo, ContRepo>();

builder.Services.AddSingleton<IStatsFaker>(new StatsFaker());
builder.Services.AddSingleton<IMailManager>(new MailManager());

builder.Services.AddDbContext<GSFContext>(o => o.UseSqlite("Data Source=App.db"));

builder.Services.AddSingleton<IContManager, ContManager>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Config.key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddCors(
    option => option.AddDefaultPolicy(
        builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()
        )
    ); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseRouting();
    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthentication();
    app.UseAuthorization();
}

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
