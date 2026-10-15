using services.DTOs.AuthorDTOs;
using services.DTOs.PublisherDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs.TitleDTOs
{
    //Chyba nie będzie potrzebne
    public class UpdateTitleDTO
    {
        public int TitleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfPublishing { get; set; }
        public long ISBN { get; set; }

        public IList<string> Genres { get; set; } = []; // FORMAT: genreA/genreB/.../genreX

        public IList<AuthorLookupDTO> Authors { get; set; } = [];
        public PublisherLookupDTO? Publisher { get; set; }
    }
}
