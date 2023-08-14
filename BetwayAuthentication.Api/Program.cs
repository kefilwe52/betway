using BetwayAuthentication.DAL.Entities;
using BetwayAuthentication.DAL.Models;
using BetwayAuthentication.DAL.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("BetwayDb"));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x
	   .AllowAnyOrigin()
	   .AllowAnyMethod()
	   .AllowAnyHeader());
app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();

app.Run();
