using BookCollection.Data;
using BookCollection.Data.DTO;
using BookCollection.Routes;
using BookCollection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookCollection.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookCollectionController : ControllerBase
{
    private readonly IBookCollectionService _bookCollectionService;
    public BookCollectionController(IBookCollectionService bookCollectionService)
    {
        _bookCollectionService = bookCollectionService;
    }

    [HttpGet]
    [Route(BookCollectionRoutes.GetBooks)]
    public IActionResult GetBooks()
    {
        var result = _bookCollectionService.GetBooks();

        if (result.StatusCode == OperationStatusCode.InternalError)
        {
            return StatusCode(500, result.Message);
        }

        return Ok(result.Data);
    }

    [HttpGet]
    [Route(BookCollectionRoutes.GetBook)]
    public IActionResult GetBook([FromRoute] int id)
    {
        var result = _bookCollectionService.GetBook(id);
        if (result.StatusCode == OperationStatusCode.NotFound)
        {
            return NotFound("The requested book does not exist.");
        }
        else if (result.StatusCode == OperationStatusCode.InternalError)
        {
            return StatusCode(500, result.Message);
        }

        return Ok(result.Data);
    }


    [HttpPost]
    [Route(BookCollectionRoutes.AddBook)]
    public IActionResult AddBook([FromBody] BookAddDTO bookDTO)
    {
        if(bookDTO == null)
        {
            return BadRequest("Invalid data in the request body.");
        }

        var result = _bookCollectionService.AddBook(bookDTO);

        if (result.StatusCode == OperationStatusCode.InternalError)
        {
            return StatusCode(500, result.Message);
        }

        return Ok();
    }

    [HttpPut]
    [Route(BookCollectionRoutes.UpdateBook)]
    public IActionResult UpdateBook([FromBody] BookDTO bookDTO)
    {
        if (bookDTO == null)
        {
            return BadRequest("Invalid data in the request body.");
        }

        var result =  _bookCollectionService.UpdateBook(bookDTO);

        if (result.StatusCode == OperationStatusCode.NotFound)
        {
            return NotFound("The requested book does not exist.");
        }
        else if (result.StatusCode == OperationStatusCode.InternalError)
        {
            return StatusCode(500, result.Message);
        }

        return Ok();
    }

    [HttpDelete]
    [Route(BookCollectionRoutes.DeleteBook)]
    public IActionResult DeleteBook([FromRoute] int id)
    {
        var result = _bookCollectionService.DeleteBook(id);

        if (result.StatusCode == OperationStatusCode.NotFound)
        {
            return NotFound("The requested book does not exist.");
        }
        else if (result.StatusCode == OperationStatusCode.InternalError)
        {
            return StatusCode(500, result.Message);
        }

        return Ok();
    }

    [HttpGet]
    [Route(BookCollectionRoutes.GetBookByTitleOrAuthor)]
    public IActionResult GetBooksByTitleOrAuthor([FromQuery] BookSearchDTO bookSearchDTO)
    {
        if (bookSearchDTO == null) 
        {
            return BadRequest("Invalid data in the request body.");
        }

        var result = _bookCollectionService.GetBooksByTitleOrAuthor(bookSearchDTO);
        if (result.StatusCode == OperationStatusCode.InternalError)
        {
            return StatusCode(500, result.Message);
        }

        return Ok(result.Data);
    }
}
