using Microsoft.AspNetCore.Mvc;

public class LibraryController : ControllerBase
{
    [HttpGet]
    public IActionResult ShowBooks(Library libraryModel){
      return Ok("List of books\n"+libraryModel.Books[0]);

    }
}
