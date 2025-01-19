using BookCollection.App.DTO;
using BookCollection.Configuration;
using BookCollection.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BookCollection.Tests;

public class BookCollectionControllerTests
{
    private readonly IBookCollectionService _bookCollectionService;
    public BookCollectionControllerTests()
    {
        var configuration = new ConfigurationBuilder()
         .AddJsonFile("appsettings.Staging.json")
         .Build();

        AppConfigurationConstants.Initialize(configuration);

        _bookCollectionService = new ServiceCollection()
            .AddTransient<IBookCollectionService, BookCollectionService>()
            .BuildServiceProvider()
            .GetRequiredService<IBookCollectionService>();

    }

    [Fact]
    public void AddBook()
    {
        var book = new BookAddDTO()
        {
            Author = "Jacob Wal",
            Title = "Some random book",
            Year = 2013
        };

        _bookCollectionService.AddBook(book);

        var addedBook = (_bookCollectionService.GetBooks().Data).FirstOrDefault();
        Assert.NotNull(addedBook);
        Assert.Equal(book.Author, addedBook.Author);
        Assert.Equal(book.Title, addedBook.Title);
        Assert.Equal(book.Year, addedBook.Year);

        File.Delete(AppConfigurationConstants.BookCollectionFile);
    }
}