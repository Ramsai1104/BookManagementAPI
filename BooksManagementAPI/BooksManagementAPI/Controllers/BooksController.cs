using BooksManagementAPI.Repository.Models;
using BooksManagementAPI.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BooksManagementAPI.Controllers
{
   
    public class BooksController : ApiController
    {
        private BookRepository repository = null;

        public BooksController()
        {
            this.repository = new BookRepository();
        }

        /// <summary>
        /// Method to retrieve all of the books in the catalog.
        /// Example: GET api/v1/books
        /// </summary>
        
        [HttpGet, Route("api/books")]
        public HttpResponseMessage Get()
        {
            IEnumerable<BookDetails> books = this.repository.GetAllBooks();
            if (books != null)
            {
                return Request.CreateResponse<IEnumerable<BookDetails>>(HttpStatusCode.OK, books);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Method to retrieve a specific book from the catalog.
        /// Example: GET api/v1/books/5
        /// </summary>
        
        [HttpGet, Route("api/books/{id:int}")]
        public HttpResponseMessage Get(String id)
        {
            BookDetails book = this.repository.GetBookById(id);
            if (book != null)
            {
                return Request.CreateResponse<BookDetails>(HttpStatusCode.OK, book);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Method to retrieve a specific book from the catalog.
        /// Example: GET api/v1/books/darwin
        /// </summary>

        [HttpGet, Route("api/books/{author}")]
        public HttpResponseMessage FindBooksByAuthor(string author)
        {
            IEnumerable<BookDetails> books = this.repository.GetBookByAuthor(author);
            if (books != null)
            {
                return Request.CreateResponse<IEnumerable<BookDetails>>(HttpStatusCode.OK, books);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Method to retrieve a specific book from the catalog.
        /// Example: GET api/v1/books/20.99
        /// </summary>

        [HttpGet, Route("api/books/{minPrice:double}/{maxPrice:double}")]
        public HttpResponseMessage FindBooksByPrice(double minPrice, double maxPrice)
        {
             
            if (minPrice > maxPrice)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,"minPrice should be always lesser than maxPrice");
            }

            IEnumerable<BookDetails> books = this.repository.GetBookByPrice(minPrice, maxPrice);


                if (books != null)
                {
                    return Request.CreateResponse<IEnumerable<BookDetails>>(HttpStatusCode.OK, books);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
          
        }

        /// <summary>
        /// Method to add a new book to the catalog.
        /// Example: POST api/v1/books
        /// </summary>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] BookDetails book)
        {
            if ((this.ModelState.IsValid) && (book != null))
            {
                BookDetails newBook = this.repository.AddNewBook(book);
                if (newBook != null)
                {
                    var httpResponse = Request.CreateResponse<BookDetails>(HttpStatusCode.Created, newBook);
                    string uri = Url.Link("DefaultApi", new { id = newBook.Id });
                    httpResponse.Headers.Location = new Uri(uri);
                    return httpResponse;
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Method to update an existing book in the catalog.
        /// Example: PUT api/v1/books/5
        /// </summary>
        [HttpPut, Route("api/books/{id:int}")]
        public HttpResponseMessage Put([FromUri]String id, [FromBody]BookDetails book)
        {
            if ((this.ModelState.IsValid) && (book != null) && (book.Id.Equals(id)))
            {
                BookDetails modifiedBook = this.repository.UpdateBookDetails(id, book);
                if (modifiedBook != null)
                {
                    return Request.CreateResponse<BookDetails>(HttpStatusCode.OK, modifiedBook);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Method to remove an existing book from the catalog.
        /// Example: DELETE api/v1/books/5
        /// </summary>
        [HttpDelete, Route("api/books/{id:int}")]
        public HttpResponseMessage Delete(String id)
        {
            BookDetails book = this.repository.GetBookById(id);
            if (book != null)
            {
                if (this.repository.DeleteBookDetails(id))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}

 
