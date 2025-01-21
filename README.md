# Book Collection

Book Collection is a project that provides an API for managing a collection of books, with data stored in JSON format, supporting operations like adding, editing, deleting, and searching.

# Built with

- ASP.NET Core - For building the web API.
- XUnit - For unit testing.
- Mvc.Testing - For testing API endpoints.
- Visual Studio 2022 - IDE

# API Documentation
| **Method** | **Endpoint**                          | **Parameters**                  | **Data**                    | **Description**                                                 |
|------------|---------------------------------------|---------------------------------|-----------------------------|-----------------------------------------------------------------|
| GET | /api/bookcollection/books             | -                            | -                        | Return all books in the collection                       |
| GET | /api/bookcollection/books/{id}        | (Integer) id                    | -                        |Return a book with a specific id                        |
| POST | /api/bookcollection/books             | -                            |  title, author, year        | Create a new book                                       |
| PUT | /api/bookcollection/books/{id}        | (Integer) id                    |  title, author, year     | Update a book with a specific id                         |
| DELETE | /api/bookcollection/books/{id}      | (Integer) id                    | -                     | Delete a book with a specific id                         |
|GET | /api/bookcollection/books/search      | title, author        | -                        | Search books by title or author                         |
