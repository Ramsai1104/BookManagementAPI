using System;
using System.Collections.Generic;
using System.Text;

namespace BooksManagementAPI.Repository.Models
{
   public class BookDetails
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public double Price { get; set; }
    }
}
