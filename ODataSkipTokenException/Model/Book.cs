namespace ODataSkipTokenException.Model;

public class Book
{
    public string Id { get; set; }

    public string Name { get; set; }

    public IList<Review> Reviews { get; set; }
}