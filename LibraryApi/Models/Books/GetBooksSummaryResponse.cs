using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models.Books
{
    public class GetBooksSummaryResponse : CollectionRepresentation<BookSummaryItem>
    {
    }

    public class BookSummaryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
