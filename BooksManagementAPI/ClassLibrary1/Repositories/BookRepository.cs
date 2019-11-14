using BooksManagementAPI.Repository.Contracts;
using BooksManagementAPI.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using System.Xml.XPath;

namespace BooksManagementAPI.Repository.Repositories
{
    public class BookRepository : IBookRepository
    {
        private string xmlFilename = string.Empty;
        private XDocument xmlDocument = null;

        public BookRepository()
        {
            try
            {
                // Determine the path to the books.xml file.
                xmlFilename = HttpContext.Current.Server.MapPath("~/app_data/books.xml");
                // Load the contents of the books.xml file into an XDocument object.
                xmlDocument = XDocument.Load(xmlFilename);
            }
            catch (Exception ex)
            {
                // Rethrow the exception.
                throw ex;
            }

        }

        public BookDetails AddNewBook(BookDetails bookDetails)
        {
            try
            {
                // Retrieve the book with the highest ID from the catalog.
                var highestBook = (
                    from bookNode in xmlDocument.Elements("books_catalog").Elements("book")
                    orderby bookNode.Attribute("id").Value descending
                    select bookNode).Take(1);
                // Extract the ID from the book data.
                string highestId = highestBook.Attributes("id").First().Value;
                // Create an ID for the new book.
                string newId = "bk" + (Convert.ToInt32(highestId.Substring(2)) + 1).ToString();
                // Verify that this book ID does not currently exist.
                if (this.GetBookById(newId) == null)
                {
                    // Retrieve the parent element for the book catalog.
                    XElement bookCatalogRoot = xmlDocument.Elements("books_catalog").Single();
                    // Create a new book element.
                    XElement newBook = new XElement("book", new XAttribute("id", newId));
                    // Create elements for each of the book's data items.
                    XElement[] bookInfo = FormatBookData(bookDetails);
                    // Add the element to the book element.
                    newBook.ReplaceNodes(bookInfo);
                    // Append the new book to the XML document.
                    bookCatalogRoot.Add(newBook);
                    // Save the XML document.
                    xmlDocument.Save(xmlFilename);
                    // Return an object for the newly-added book.
                    return this.GetBookById(newId);
                }
            }
            catch (Exception ex)
            {
                // Rethrow the exception.
                throw ex;
            }
            // Return null to signify failure.
            return null;

        }

        /// <summary>
        /// Populates a book BookDetails class with the data for a book.
        /// </summary>
        private XElement[] FormatBookData(BookDetails book)
        {
            XElement[] bookInfo =
            {
                new XElement("book_name", book.BookName),
                new XElement("author", book.Author),
                new XElement("publish_date", book.PublishedDate.ToString()),
                new XElement("price", book.Price.ToString())
            };
            return bookInfo;
        }

        public bool DeleteBookDetails(string id)
        {
            try
            {
                if (this.GetBookById(id) != null)
                {
                    // Remove the specific child node from the catalog.
                    xmlDocument
                        .Elements("books_catalog")
                        .Elements("book")
                        .Where(x => x.Attribute("id").Value.Equals(id))
                        .Remove();
                    // Save the XML document.
                    xmlDocument.Save(xmlFilename);
                    // Return a success status.
                    return true;
                }
                else
                {
                    // Return a failure status.
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Rethrow the exception.
                throw ex;
            }
        }

        public IEnumerable<BookDetails> GetAllBooks()
        {
            try
            {
                // Return a list that contains the catalog of book ids/titles.
                return (
                    // Query the catalog of books.
                    from book in xmlDocument.Elements("books_catalog").Elements("book")
                        // Sort the catalog based on book IDs.
                    orderby book.Attribute("id").Value ascending
                    // Create a new instance of the detailed book information class.
                    select new BookDetails
                    {
                        // Populate the class with data from each of the book's elements.
                        Id = Convert.ToInt32(book.Attribute("id").Value),
                        BookName = book.Element("book_name").Value,
                        Author = book.Element("author").Value,
                        PublishedDate = Convert.ToDateTime(book.Element("publish_date").Value),
                        Price = Convert.ToDouble(book.Element("price").Value)
                    }).ToList();
            }
            catch (Exception ex)
            {
                // Rethrow the exception.
                throw ex;
            }
        }

        public IEnumerable<BookDetails> GetBookByAuthor(string author)
        {
            try
            {
                // Retrieve a specific book from the catalog.
                var result = (
                     // Query the catalog of books.
                     from book in xmlDocument.Elements("books_catalog").Elements("book")
                         // Specify the specific book ID to query.
                     where book.Element("author").Value.Contains(author)
                    // Create a new instance of the detailed book information class.
                    select new BookDetails
                     {
                        // Populate the class with data from each of the book's elements.
                        Id = Convert.ToInt32(book.Attribute("id").Value),
                         BookName = book.Element("book_name").Value,
                         Author = book.Element("author").Value,
                         PublishedDate = Convert.ToDateTime(book.Element("publish_date").Value),
                         Price = Convert.ToDouble(book.Element("price").Value)
                     }).ToList();

                return result;
            }
            catch(Exception ex)
            {
                // Return null to signify failure.
                throw ex;
               // return null;
            }
        }

        public BookDetails GetBookById(string id)
        {
            try
            {
                // Retrieve a specific book from the catalog.
               return (
                        // Query the catalog of books.
                    from book in xmlDocument.Elements("books_catalog").Elements("book")
                        // Specify the specific book ID to query.
                    where book.Attribute("id").Value.Equals(id)
                    // Create a new instance of the detailed book information class.
                    select new BookDetails
                    {
                        // Populate the class with data from each of the book's elements.
                        Id = Convert.ToInt32(book.Attribute("id").Value),
                        BookName = book.Element("book_name").Value,
                        Author = book.Element("author").Value,
                        PublishedDate = Convert.ToDateTime(book.Element("publish_date").Value),
                        Price = Convert.ToDouble(book.Element("price").Value)
                    }).Single();
            }
            catch
            {
                // Return null to signify failure.
                return null;
            }

        }

        public IEnumerable<BookDetails> GetBookByPrice(double minPrice, double maxPrice)
        {
            try
            {

                 // Retrieve a specific book from the catalog.
                return (
                     // Query the catalog of books.
                     from book in xmlDocument.Elements("books_catalog").Elements("book")
                         // Specify the specific book ID to query.
                    where Convert.ToDouble(book.Element("price").Value) >= (minPrice) && Convert.ToDouble(book.Element("price").Value) < (maxPrice)
                    // Create a new instance of the detailed book information class.
                    select new BookDetails
                     {
                        // Populate the class with data from each of the book's elements.
                        Id = Convert.ToInt32(book.Attribute("id").Value),
                        BookName = book.Element("book_name").Value,
                         Author = book.Element("author").Value,
                         PublishedDate = Convert.ToDateTime(book.Element("publish_date").Value),
                         Price = Convert.ToDouble(book.Element("price").Value)
                     }).ToList();
            }
            catch
            {
                // Return null to signify failure.
                return null;
            }

        }

        public BookDetails UpdateBookDetails(string id, BookDetails bookDetails)
        {
            try
            {
                // Retrieve a specific book from the catalog.
                XElement updateBook = xmlDocument.XPathSelectElement(String.Format("books_catalog/book[@id='{0}']", id));
                // Verify that the book exists.
                if (updateBook != null)
                {
                    // Create elements for each of the book's data items.
                    XElement[] bookInfo = FormatBookData(bookDetails);
                    // Add the element to the book element.
                    updateBook.ReplaceNodes(bookInfo);
                    // Save the XML document.
                    xmlDocument.Save(xmlFilename);
                    // Return an object for the updated book.
                    return this.GetBookById(id);
                }
            }
            catch (Exception ex)
            {
                // Rethrow the exception.
                throw ex;
            }
            // Return null to signify failure.
            return null;
        }
    }
}
