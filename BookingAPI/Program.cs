using BookingAPI.Extensions;
using BookingAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogLogging();

builder.Services.AddAppDbContext(builder.Configuration);

builder.Services.AddAppServices(builder.Configuration);

builder.Services.AddAppHttpClients(builder.Configuration);

builder.Services.AddAppMapping();

builder.Services.AddAppValidation();

builder.Services.AddResponseCaching();

builder.Services.AddControllers();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddSwaggerWithJwt();



var app = builder.Build();

app.UseExceptionHandlerMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.UseResponseCaching();

app.MapControllers();

app.Run();
