using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatientManagement.BusinessLogic;
using PatientManagement.BusinessLogic.Helpers;
using PatientManagement.BusinessLogic.Implementation;
using PatientManagement.Data;
using PatientManagement.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PatientDb")));

builder.Services.AddIdentity<ApplicationModel, IdentityRole>()
    .AddEntityFrameworkStores<PatientDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

           .AddJwtBearer(option =>
           {
               option.SaveToken = true;
               option.RequireHttpsMetadata = false;

               option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateIssuerSigningKey = true,
                   ValidAudience = builder.Configuration["Jwt:ValidAudience"],
                   ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
               };
           });

builder.Services.AddCors(options =>
{
    options.AddPolicy("corspolicy", builder =>
    {
        builder.WithOrigins("*")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddMemoryCache();
builder.Services.AddLazyCache();


builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddAutoMapper(typeof(PatientProfile).Assembly);

builder.Services.AddTransient<Filtering>();
builder.Services.AddTransient<Sorting>();
builder.Services.AddTransient<Pagination>();
builder.Services.AddTransient<MemoryCaching>();

builder.Services.AddTransient<IPatientRepository, PatientRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();

//builder.Services.AddAutoMapper(typeof(Program));

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

app.UseCors("corspolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
