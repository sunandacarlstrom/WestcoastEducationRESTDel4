using System.ComponentModel;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WestcoastEducation.Api.Data;
using WestcoastEducation.Api.Models;
using WestcoastEducation.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Setup database connection... 
builder.Services.AddDbContext<WestcoastEducationContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("sqlite"));
});

// Setup Identity...
builder.Services.AddIdentityCore<UserModel>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WestcoastEducationContext>();

// makes TokenService available 
builder.Services.AddScoped<TokenService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
 {
     options.SwaggerDoc("v1", new OpenApiInfo
     {
         Version = "v1",
         Title = "Westcoast Education API",
         Description = "Ett api för att hantera kurser, studenter, lärare och dess skills",
         // redirect to a (fictive)webpage where you can read the terms in order to use this api
         TermsOfService = new Uri("https://westcoast-education.se/terms"),
         Contact = new OpenApiContact
         {
             Name = "Teknisk support",
             Url = new Uri("https://westcoast-education.se/contact")
         },
         License = new OpenApiLicense
         {
             Name = "Licens",
             Url = new Uri("https://westcoast-education.se/license")
         }
     });
     // define where the documentation for Swagger will be located in our file
     var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
     // include our XML-comments
     options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
 });

// makes Authentication with JWT available 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("tokenSettings:tokenKey").Value))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Seed the database 
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<WestcoastEducationContext>();
    var userMgr = services.GetRequiredService<UserManager<UserModel>>();
    var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

    await context.Database.MigrateAsync();

    await SeedData.LoadRolesAndUsers(userMgr, roleMgr);
    await SeedData.LoadTeacherData(context);
    await SeedData.LoadTeacherSkillsData(context);
    await SeedData.LoadCourseData(context);
    await SeedData.LoadStudentData(context);
}
catch (Exception ex)
{
    Console.WriteLine("{0}", ex.Message);
    throw;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
