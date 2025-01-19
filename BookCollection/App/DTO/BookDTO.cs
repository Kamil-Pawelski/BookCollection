using Microsoft.AspNetCore.Mvc;

namespace BookCollection.App.DTO;

public class BookDTO
{
    [FromRoute]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
}
