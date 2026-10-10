using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Publisher
    {
        public int PublisherId { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Title> Titles { get; set; } = [];
    }
}
