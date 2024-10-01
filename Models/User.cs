public class User
{
    public string Name { set; get; }
    public string Surname { set; get; }
    public List<Book>? RentedBooks { set; get; }
    public int Id { set; get; }

    public User()
    {
        Name = "N/A";
        Surname = "N/A";
    }
}
