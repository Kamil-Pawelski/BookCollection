using BookCollection.App.DTO;
using BookCollection.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BookCollection.Tests;

public class BookCollectionControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private static readonly object _fileLock = new();

    private void WriteTestData(string content)
    {
        using (var writer = new StreamWriter(AppConfiguration.BookCollectionFile))
        {
            writer.WriteLine(content);
        }
    }
    public BookCollectionControllerTests(WebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Staging");

        _factory = factory;
    }

    [Fact]
    public async Task Get_EndpoinReturnSuccess()
    {
        int bookId = 1;

        var client = _factory.CreateClient();
        WriteTestData("[{\"Id\":1,\"Title\":\"raz\",\"Author\":\"dwa\",\"Year\":2000}]");

        var response = await client.GetAsync($"/api/BookCollection/books/{bookId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_EndpoinReturnNotFound()
    {
        int bookId = 2;

        var client = _factory.CreateClient();
        WriteTestData("[{\"Id\":1,\"Title\":\"raz\",\"Author\":\"dwa\",\"Year\":2000}]");

        var response = await client.GetAsync($"/api/BookCollection/books/{bookId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetBooks_EndpoinReturnSuccess()
    {
        var client = _factory.CreateClient();
        WriteTestData("[{\"Id\":1,\"Title\":\"raz\",\"Author\":\"dwa\",\"Year\":2000}]");

        var response = await client.GetAsync("/api/BookCollection/books");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetBooks_Empty_EndpoinReturnSuccess()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/BookCollection/books");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_EndpoinReturnSuccess()
    {
        int bookId = 1;

        var client = _factory.CreateClient();
        WriteTestData("[{\"Id\":1,\"Title\":\"raz\",\"Author\":\"dwa\",\"Year\":2000}]");

        var response = await client.DeleteAsync($"/api/BookCollection/books/{bookId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_EndpoinReturnNotFound()
    {
        int bookId = 2;

        var client = _factory.CreateClient();
        WriteTestData("[{\"Id\":1,\"Title\":\"raz\",\"Author\":\"dwa\",\"Year\":2000}]");

        var response = await client.DeleteAsync($"/api/BookCollection/books/{bookId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_EndpoinReturnSuccess()
    {
        var client = _factory.CreateClient();

        var book = new BookDTO
        {
            Title = "raz",
            Author = "dwa",
            Year = 2000
        };

        var json = JsonSerializer.Serialize(book);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"/api/BookCollection/books", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_EndpoinReturnSuccess()
    {
        int bookId = 1;

        var client = _factory.CreateClient();
        WriteTestData("[{\"Id\":1,\"Title\":\"raz\",\"Author\":\"dwa\",\"Year\":2000}]");

        var book = new BookDTO
        {
            Title = "trzy",
            Author = "cztery",
            Year = 2000
        };

        var json = JsonSerializer.Serialize(book);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"/api/BookCollection/books/{bookId}", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Put_EndpoinReturnNotFound()
    {
        int bookId = 2;

        var client = _factory.CreateClient();
        WriteTestData("[{\"Id\":1,\"Title\":\"raz\",\"Author\":\"dwa\",\"Year\":2000}]");

        var book = new BookDTO
        {
            Title = "trzy",
            Author = "cztery",
            Year = 2000
        };

        var json = JsonSerializer.Serialize(book);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"/api/BookCollection/books/{bookId}", content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("Some random book", "", HttpStatusCode.OK)]
    [InlineData("", "Jacob Wal", HttpStatusCode.OK)]
    [InlineData("Some random book", "Jacob Wal", HttpStatusCode.OK)]
    [InlineData("Next das", "Jacob Wal", HttpStatusCode.OK)]

    public async Task GetBooksByTitleOrAuthor_EndpoinReturnSuccess(string title, string author, HttpStatusCode statusCode)
    {
        var client = _factory.CreateClient();

        WriteTestData("[{\"Id\":1,\"Title\":\"raz\",\"Author\":\"dwa\",\"Year\":2000},{\"Id\":2,\"Title\":\"Some random book\",\"Author\":\"Jacob Wal\",\"Year\":2010}]");

        var query = "";

        if(!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(author))
        {
            query = $"?Title={title}&Author={author}";
        }
        else if(string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(author))
        {
            query = $"?Author={author}";
        }
        else if (!string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author))
        {
            query = $"?Title={title}";
        }

        var response = await client.GetAsync("/api/BookCollection/books/search" + query);

        Assert.Equal(statusCode, response.StatusCode);
    }

    [Fact]
    public async Task GetBooksByTitleOrAuthor_EndpoinReturnBadRequest()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/BookCollection/books/search?Name=Abc&Test=sa");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    public void Dispose()
    {
        if (File.Exists(AppConfiguration.BookCollectionFile))
        {
            File.Delete(AppConfiguration.BookCollectionFile);
        }
    }
}
