using BookCollection.Configuration;
using BookCollection.Domain;
using BookCollection.Domain.Repositories;
using System.Text.Json;

namespace BookCollection.Infrastructure.Repositories;

public class BookCollectionRepository : IBookCollectionRepository
{
    public List<Book> ReadFile()
    {
        if (File.Exists(AppConfiguration.BookCollectionFile))
        {
            var fileText = File.ReadAllText(AppConfiguration.BookCollectionFile);

            if (string.IsNullOrWhiteSpace(fileText))
            {
                return [];
            }

            return JsonSerializer.Deserialize<List<Book>>(fileText) ?? [];
        }

        return [];
    }

    public void WriteToFile(List<Book> booksList)
    {
        var content = JsonSerializer.Serialize(booksList);

        File.WriteAllText(AppConfiguration.BookCollectionFile, content);
    }
}
