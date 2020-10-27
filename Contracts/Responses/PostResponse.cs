using System;
using System.Collections.Generic;
using PostBook.Domains;

namespace PostBook.Contracts.Responses
{
    internal class PostResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TagResponse> Tags { get; set; }
    }
}