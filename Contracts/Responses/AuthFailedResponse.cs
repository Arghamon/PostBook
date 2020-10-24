using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace PostBook.Contracts.Responses
{
    internal class AuthFailedResponse : ModelStateDictionary
    {
        public IEnumerable<string> Errors { get; set; }
    }
}