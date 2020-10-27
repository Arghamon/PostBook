using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostBook.Domains
{
    public class Tag
    {
        [Key]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
