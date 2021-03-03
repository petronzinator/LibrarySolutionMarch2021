using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace LibraryApi.Controllers
{
    public class BooksController : ControllerBase
    {
        private readonly LibraryDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;
        private readonly ILogger<BooksController> _logger;

        public BooksController(LibraryDataContext context, IMapper mapper, MapperConfiguration config, ILogger<BooksController> logger)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _logger = logger;
        }

        [HttpPut("/books/{id:int}/genre")]
        public async Task<ActionResult> UpdateGenre(int id, [FromBody] string genre)
        {
            var book = await _context.AvailableBooks.SingleOrDefaultAsync(b => b.Id == id);

            if(book != null)
            {
                book.Genre = genre; // One "Gotcha" - we aren't validating here.
                await _context.SaveChangesAsync();
                return Accepted(); // 202 means "I Did it!", could also use "No Content"
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("/books/{id:int}")]
        public async Task<ActionResult> RemoveBookFromInventory(int id)
        {
            var book = await _context.AvailableBooks.SingleOrDefaultAsync(b => b.Id == id);
            if(book != null)
            {
                //_context.Books.Remove(book);
                book.IsAvailable = false;
                await _context.SaveChangesAsync();
            }
            return NoContent(); // this is a 204 message. 
        }

        //XML COMMENTS type 3 slashes
        /// <summary>
        /// Use this to add a book to our inventory
        /// </summary>
        /// <param name="request">Which book you want to add</param>
        /// <returns>a new book</returns>

        [HttpPost("/books")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 15)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<GetBookDetailsResponse>> AddABook([FromBody] PostBookRequest request)
        {
            // 1 Validate it. If Not - return a 400 Bad Request, optionally with some info
            // programatic, imperative validation
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // 2. Save it in the database
            //    - Turn a PostBookRequest -> Book
            var bookToSave = _mapper.Map<Book>(request);
            //    - Add it to the Context
            _context.Books.Add(bookToSave);
            //    - Tell the context to save the changes.
            await _context.SaveChangesAsync();
            //    - Turn that saved book into a GetBookDetailsResponse
            var response = _mapper.Map<GetBookDetailsResponse>(bookToSave);
            // 3. Return:
            //    - 201 Created Status Code
            //    - A location header with the Url of the newly created book.
            //    - A copy of the newly created book (what they would get if they followed the location)

            return CreatedAtRoute("books#getbooksbyid", new { id = response.Id }, response);

        }

        [HttpGet("/books")]
        public async Task<ActionResult<GetBooksSummaryResponse>> GetAllBooks([FromQuery] string genre = null)
        {

            var query = _context.AvailableBooks;
            if(genre != null)
            {
                query = query.Where(b => b.Genre == genre);
            };

            var data = await query.ProjectTo<BookSummaryItem>(_config).ToListAsync();

            var response = new GetBooksSummaryResponse
            {
                Data = data,
                GenreFilter = genre
            };
            return Ok(response);
        }

        [HttpGet("/books/{id:int}", Name= "books#getbooksbyid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetBookDetailsResponse>> GetBookById(int id)
        {
            var book = await _context.AvailableBooks
                .Where(b => b.Id == id)
                .ProjectTo<GetBookDetailsResponse>(_config)
                .SingleOrDefaultAsync();

            if (book == null)
            {
                _logger.LogWarning("No book with that id!", id);
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }
    }
}
