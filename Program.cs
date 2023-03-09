using Microsoft.EntityFrameworkCore;
using MyLoginApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using MyLoginApi.Services.EmailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IEmailService, EmailService>();

// If you want to enable roles you can uncomment these 

//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//    {
//        Description = "Standard Authorization header using the Beared Scheme",
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey
//    });
//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

//    .AddJwtBearer(options =>

//    {

//        options.TokenValidationParameters = new TokenValidationParameters

//        {

//            ValidateIssuerSigningKey = true,

//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8

//                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),

//            ValidateIssuer = false,

//            ValidateAudience = false

//        };

//    }); 


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });                
    });
builder.Services.AddDbContext<UserContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("String")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
