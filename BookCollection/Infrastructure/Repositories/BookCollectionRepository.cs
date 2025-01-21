using BookCollection.Configuration;
using BookCollection.Domain;
using BookCollection.Domain.Repositories;
using System.IO;
using System.Text.Json;

namespace BookCollection.Infrastructure.Repositories;

public class BookCollectionRepository : IBookCollectionRepository
{
    public List<Book> ReadFile()
    {
        if (File.Exists(AppConfiguration.BookCollectionFile))
        {
            using (var reader = new StreamReader(AppConfiguration.BookCollectionFile)) 
            {
                var fileText = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(fileText))
                {
                    return [];
                }
                return JsonSerializer.Deserialize<List<Book>>(fileText) ?? [];
            }
        }
        return [];
    }

    public void WriteToFile(List<Book> booksList)
    {
        using (var writer = new StreamWriter(AppConfiguration.BookCollectionFile))
        {
            var content = JsonSerializer.Serialize(booksList);
            writer.Write(content);
        }     
    }
}
