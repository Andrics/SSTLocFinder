using Microsoft.EntityFrameworkCore;
using PasswordLookupApp.Data;
using DotNetEnv; // Add this

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env (local only)
Env.Load();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=users.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    SeedData.Initialize(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://0.0.0.0:{port}");


app.Run();
