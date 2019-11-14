using BooksManagementAPI.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BooksManagementAPI.Repository.Contracts
{
   public interface IBookRepository
    {
        BookDetails AddNewBook(BookDetails bookDetails);
        IEnumerable<BookDetails> GetAllBooks();
        BookDetails GetBookById(string Id);
        IEnumerable<BookDetails> GetBookByAuthor(string author);
        IEnumerable<BookDetails> GetBookByPrice(double minPrice, double maxPrice);
        BookDetails UpdateBookDetails(string Id, BookDetails book);
        bool DeleteBookDetails(string Id);

    }
}
