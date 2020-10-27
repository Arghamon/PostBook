using System.Collections.Generic;

namespace PostBook.Contracts.Requests
{
    public class CreatePostRequest
    {
        public string Name { get; set; }

         public IEnumerable<string> Tags { get; set; }
    }
}
