using API.Infrastructure.Context;
using API.Services.Services;
using API.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApiContext"),
    b => b.MigrationsAssembly("API.Infrastructure")));
// Add services to the container.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStoryService, StoryService>();
builder.Services.AddScoped<IVoteService, VoteService>();

// Add CORS services
builder.Services.AddCors(p => p.AddPolicy("AllowOrigin", build =>
{
    build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
