using System;
namespace PostBook.Domains
{
    public class PostTag
    {
        public virtual Post Post { get; set; }

        public Guid PostId { get; set; }

        public virtual Tag Tag { get; set; }

        public string TagName { get; set; }
    }
}
