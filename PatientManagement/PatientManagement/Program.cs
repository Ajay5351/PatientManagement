using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Data;
using PatientManagement.Models;
using PatientManagement.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PatientContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PatientDb")));

builder.Services.AddIdentity<ApplicationModel, IdentityRole>()
    .AddEntityFrameworkStores<PatientContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.AddTransient<IPatientRepository, PatientRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();

builder.Services.AddAutoMapper(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
