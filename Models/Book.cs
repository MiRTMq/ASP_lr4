public class Book
{
    public string Name { set; get; }
    public string Author { set; get; }
    public int Id { set; get; }
    public Book(string name, string author, string id)
    {
        Name = (name != null) ? name : "N/A";
        Author = (author != null) ? author : "N/A";
        Id = int.Parse(id);
    }
}
