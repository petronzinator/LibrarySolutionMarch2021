using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class CollectionRepresentation<T>
    {
        public IList<T> Data { get; set; }
    }
}
