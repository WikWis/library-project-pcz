using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Title
    {
        public int TitleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfPublishing { get; set; }
        public long ISBN { get; set; }

        public string Genres { get; set; } = string.Empty; // FORMAT: genreA/genreB/.../genreX

        public int PublisherId { get; set; }
        public virtual Publisher? Publisher { get; set; }
        public virtual ICollection<Author> Authors { get; set; } = [];
    }
}
