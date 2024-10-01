var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

Dictionary<string, string> path = new Dictionary<string, string>();
path.Add("users", "config/config.json");
path.Add("books", "config/books.json");

builder.Configuration.AddJsonFile(path["books"]);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for
    // production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default",
                       pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", (IConfiguration config) =>

    {
return(config.GetSection("Books").GetSection("1").GetSection("Title"));
});

app.Run();
