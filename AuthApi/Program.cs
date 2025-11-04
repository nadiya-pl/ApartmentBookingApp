using AuthAPI.Data;
using AuthAPI.Extensions;
using AuthAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// serilog
builder.Host.AddSerilogLogging();

// database (DbContext with SQL Server)
builder.Services.AddDatabase(builder.Configuration);

// identity (users, roles, lockout settings)
builder.Services.AddIdentityServices();

// validation (FluentValidation + disable default ModelState validation)
builder.Services.AddValidation();

// auth services (AuthService, TokenGenerator, etc.)
builder.Services.AddAuthServices();

// controllers (API endpoints)
builder.Services.AddControllers();

// swagger (OpenAPI documentation)
builder.Services.AddSwaggerDocumentation();



var app = builder.Build();

// global error handling middleware
app.UseErrorHandleMiddleware();

// enable swagger only in development
app.UseSwaggerIfDev();

// middleware pipeline
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbInitializer.SeedRolesAndUsersAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
