using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using User.User.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<ILogin, User.User.Infrastructure.RepositoriesImpl.Login>();

ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(configuration);

//builder.Services.AddAuthorization(options =>
//{
//    //options.AddPolicy("ClientIdPolicy", policy => policy.RequireClaim("client_id", "movieClient", "movies_mvc_client"));
//    //options.AddPolicy("policiesss", policy => policy.RequireClaim("client_id", "emmanuel", "olamide", "kelvin"));
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
//app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();