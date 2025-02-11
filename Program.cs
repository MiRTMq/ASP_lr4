var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

Dictionary<string, string> path = new Dictionary<string, string>();
path.Add("users", "config/users.json");
path.Add("books", "config/books.json");

builder.Configuration.AddJsonFile(path["books"]);
builder.Configuration.AddJsonFile(path["users"]);
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

string GetBookTitleById(int id, IConfiguration config)
{
    var title = config.GetSection("Books")
                  .GetSection(id.ToString())
                  .GetValue<string>("Title");
    return title ?? "not found";
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/Library", async context =>
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync("Вітаємо у бібліотеці!");
    });

    endpoints.MapGet("/Library/Books", (IConfiguration config) =>
    {
        IConfigurationSection section = config.GetSection("Books");

        var bookList = section.GetChildren()
            .Select(book => $"{book.GetValue<string>("Title")} - {book.GetValue<string>("Author")}")
            .ToList();

        return Results.Json(bookList);
    });

    endpoints.MapGet("/Library/Profile/{id?}", (HttpContext context, IConfiguration config) =>
    {
        string userId = context.Request.RouteValues["id"]?.ToString();
        if (string.IsNullOrEmpty(userId))
        {
            return Results.Json(new { Message = "Артем 22210909" });
        }

        if (!int.TryParse(userId, out int id) || id < 0 || id > 5)
        {
            return Results.BadRequest("ID недоступне (лише від 0 до 5)");
        }

        IConfigurationSection userSection = config.GetSection("Users").GetSection(id.ToString());
        if (!userSection.Exists())
        {
            return Results.NotFound($"Користувач із ID {id} не знайдений");
        }

        var rentedBooks = userSection.GetSection("RentedBooks")
                             .GetChildren()
                             .Select(bookId => GetBookTitleById(int.Parse(bookId.Value), config))
                             .ToList();

        var user = new
        {
            Name = userSection.GetValue<string>("Name"),
            Surname = userSection.GetValue<string>("Surname"),
            RentedBooks = rentedBooks
        };

        return Results.Json(user);
    });
});

app.MapGet("/", () =>
{
    return "/Library\n/Library/Books\n/Library/Profile/{id}";
});

app.Run(async context =>
{
    context.Response.ContentType = "text/plain; charset=utf-8";
    await context.Response.WriteAsync("Сторінка не знайдена");
});

app.Run();
