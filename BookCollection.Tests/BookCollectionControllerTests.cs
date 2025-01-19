using BookCollection.App.DTO;
using BookCollection.Configuration;
using BookCollection.Domain;
using BookCollection.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
namespace BookCollection.Tests;

public class BookCollectionControllerTests : IDisposable
{
    private readonly IBookCollectionService _bookCollectionService;
    public BookCollectionControllerTests()
    {
        var configuration = new ConfigurationBuilder()
         .AddJsonFile("appsettings.json")
         .AddJsonFile("appsettings.Staging.json")
         .Build();

        // For now, failure tests need to be done again. At this moment, the tests are working on the same file so there is some problems with it.
        AppConfigurationConstants.Initialize(configuration);

        _bookCollectionService = new BookCollectionService();

    }
    [Fact]
    public void AddAndGetBook_Ok()
    {
        var book = new BookAddDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);

        var result = _bookCollectionService.GetBook(1);
        Assert.NotNull(result.Data);
        Assert.Equal(book.Author, result.Data.Author);
        Assert.Equal(book.Title, result.Data.Title);
        Assert.Equal(book.Year, result.Data.Year);
        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);

    }

    [Fact]
    public void GetBook_NotFound()
    {
        var book = new BookAddDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);

        var result = _bookCollectionService.GetBook(200);
        Assert.Null(result.Data);
        Assert.Equal(OperationStatusCode.NotFound, result.StatusCode);
        Assert.Equal("The requested book does not exist.", result.Message);
    }

    [Fact]
    public void GetBooks_Ok()
    {
        var booksToAdd = new List<BookAddDTO>(){
            new()
            {
                Author = "Jacob Wal",
                Title = "Some random book",
                Year = 2013
            },
            new()
            {
                Author = "Adam Szym",
                Title = "Next das",
                Year = 2010
            },
            new()
            {
                Author = "Kim Min",
                Title = "Han guk",
                Year = 2019
            },
        };

        foreach (var book in booksToAdd)
        {
            _bookCollectionService.AddBook(book);
        }

        var result = _bookCollectionService.GetBooks();
        Assert.NotNull(result.Data);
        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);
    }

    [Fact]
    public void GetBooks_Empty_Ok()
    {
        var result = _bookCollectionService.GetBooks();
        Assert.Empty(result.Data);
        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);
    }

    [Fact]
    public void DeleteBook_Ok()
    {
        var book = new BookAddDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);
        var result = _bookCollectionService.DeleteBook(1);

        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);
    }

    [Fact]
    public void DeleteBook_NotFound()
    {
        var book = new BookAddDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);
        var result = _bookCollectionService.DeleteBook(12);

        Assert.Equal(OperationStatusCode.NotFound, result.StatusCode);
        Assert.Equal("The requested book does not exist.", result.Message);
    }

    [Fact]
    public void UpdateBook_Ok()
    {
        var book = new BookAddDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);

        var updatedBook = new BookDTO
        {
            Id = 1,
            Author = "Pavel Atr",
            Title = "New Book",
            Year = 2013
        };

        var result = _bookCollectionService.UpdateBook(updatedBook);

        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);
    }

    [Fact]
    public void UpdateBook_NotFound()
    {
        var book = new BookAddDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);

        var updatedBook = new BookDTO
        {
            Id = 15,
            Author = "Pavel Atr",
            Title = "New Book",
            Year = 2013
        };

        var result = _bookCollectionService.UpdateBook(updatedBook);

        Assert.Equal(OperationStatusCode.NotFound, result.StatusCode);
    }

    [Theory]
    [InlineData("Some random book", "", true)]
    [InlineData("", "Jacob Wal", true)]
    [InlineData("Some random book", "Jacob Wal", true)]
    [InlineData("Next das", "Jacob Wal", false)]
    public void GetBooksByTitleOrAuthor_AuthorAndTitle_Ok(string title, string author, bool boolResult)
    {
        var booksToAdd = new List<BookAddDTO>(){
        new()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        },
        new()
        {
            Author = "Adam Szym",
            Title = "Next das",
            Year = 2010
        },
        new()
        {
            Author = "Kim Min",
            Title = "Han guk",
            Year = 2019
        },
    };

        foreach (var book in booksToAdd)
        {
            _bookCollectionService.AddBook(book);
        }

        var searchDTO = new BookSearchDTO()
        {
            Title = title,
            Author = author
        };

        var result = _bookCollectionService.GetBooksByTitleOrAuthor(searchDTO);

        if (boolResult)
        {
            Assert.NotEmpty(result.Data);
        }
        else
        {
            Assert.Empty(result.Data);
        }
        Assert.Equal(OperationStatusCode.Ok, result.StatusCode);
    }

    public void Dispose()
    {
        if (File.Exists(AppConfigurationConstants.BookCollectionFile))
        {
            File.Delete(AppConfigurationConstants.BookCollectionFile);
        }
    }
}