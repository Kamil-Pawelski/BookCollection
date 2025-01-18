using BookCollection.Controllers;
using BookCollection.Data.DTO;
using BookCollection.Services;
using NSubstitute;


namespace BookCollection.Tests;

public class BookCollectionControllerTests
{
    private readonly BookCollectionController _controller;
    private readonly IBookCollectionService _mockBookCollectionService;
    public BookCollectionControllerTests(BookCollectionController controller)
    {
        _mockBookCollectionService = Substitute.For<IBookCollectionService>();
        _controller = new BookCollectionController(_mockBookCollectionService);
    }

    [Fact]
    public void GetAllBooks()
    {
        var Books = new List<BookAddDTO>()
        {
            new() 
            {
                Title = "The Lord of the Rings",
                Author = "J.R.R. Tolkien",
                Year = 1954
            },
            new()
            {
                Title = "1984",
                Author = "George Orwell",
                Year = 1949
            }
        };


        foreach (var book in Books) 
        {
            _mockBookCollectionService.AddBook(book);
        }

        var result = _controller.GetBooks();

    }
}