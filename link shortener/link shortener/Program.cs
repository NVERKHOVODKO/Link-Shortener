using System.Configuration;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProjectX.Middlewares;
using Repository;
using TestApplication.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Users CRUD API",
        Description = "An ASP.NET Core Web API for CRUD operations on users"
    });
});

builder.Services.AddTransient<ILinkService, LinkService>();
builder.Services.AddTransient<IDbRepository, DbRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; });

var connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
        options.UseMySql(connString, ServerVersion.AutoDetect(connString)));
builder.Services.AddTransient<DataContext>();


var app = builder.Build();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    DbInitializer.Initialize(context);
}

app.Run();