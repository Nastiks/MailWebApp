using MailTask.Server.Data;
using MailTask.Server.Hubs;
using MailTask.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetValue<string>("SQLiteConnectionString");
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseLazyLoadingProxies();
    x.UseSqlite(connectionString);
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddAuthentication("MailUser").AddCookie("MailUser");
builder.Services.AddAuthorization();
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);
builder.Services.AddScoped<AccountService>();
builder.Services.AddSignalR();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseResponseCompression();
app.MapHub<MessageHub>("/messageHub");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});

app.Run();
