using BookApp.DAL;
using BookApp.Helpers;
using BookApp.Services;

var builder = WebApplication.CreateBuilder(args);

{
    var services = builder.Services;
    services.AddScoped<DataContext>();
    services.AddScoped<IBookRepository, BookRepository>();
    services.AddScoped<IBookService, BookService>();
}
builder.Services.AddControllersWithViews();

var app = builder.Build();

var loggerFactory = app.Services.GetService<ILoggerFactory>();

loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Book/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");

app.Run();