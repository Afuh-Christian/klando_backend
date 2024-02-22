using API;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

RegisterDependencies.RegisterCORS(builder.Services);

RegisterDependencies.RegisterAuth(builder.Services, builder.Configuration);

RegisterDependencies.RegisterDbContext(builder.Services, builder.Configuration);

RegisterDependencies.RegisterRepositories(builder.Services);

//RegisterDependencies.RegisterCORS(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header, 
        Name = "Authorization", 
        Type = SecuritySchemeType.ApiKey 
        
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("klando_frontend");

//app.MapIdentityApi<IdentityUser>();

app.MapSwagger().RequireAuthorization();

app.UseHttpsRedirection();

app.UseCookiePolicy();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();









