using Frontend.Exceptions;
using Frontend.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogLogging();

builder.AddHttpClients();

builder.AddAuthenticationAndSession();

builder.AddApplicationServices();


var app = builder.Build();


app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
