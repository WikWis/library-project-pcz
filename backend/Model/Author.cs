using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public virtual ICollection<Title> Titles { get; set; } = [];
    }
}
