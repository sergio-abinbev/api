using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Application.MappingProfiles;
using dotenv.net;
using EmployeeManagement.Api;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
Settings.Initialize(builder.Configuration);

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
var connectionString = Settings.GetConnectionString();

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddScoped<IEmployeeRepository, EmployeeRepository>();
services.AddScoped<EmployeeService>(); 
services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>(); 
services.AddAutoMapper(typeof(EmployeeProfile).Assembly); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();