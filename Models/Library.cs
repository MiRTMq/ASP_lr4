public class Library
{
    public List<Book> Books { set; get; }
    public List<User>? Users { set; get; }

    public Library(IConfiguration config)
    {
        Books[0].Name = config.GetSection("Books")
                              .GetSection("1")
                              .GetSection("Title")
                              .ToString();
    }
}
