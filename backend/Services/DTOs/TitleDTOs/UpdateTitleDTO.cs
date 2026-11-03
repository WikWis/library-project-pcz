using services.DTOs.AuthorDTOs;
using services.DTOs.PublisherDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.DTOs.TitleDTOs
{
    public class UpdateTitleDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Niepoprawne ID tytułu.")]
        [Required(ErrorMessage = "ID tytułu jest wymagane.")]
        [Display(Name = "ID tytułu")]
        public int TitleId { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Tytuł musi mieć od 1 do 100 znaków.")]
        [Display(Name = "Tytuł")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data publikacji jest wymagana.")]
        [Display(Name = "Data wydania")]
        public DateOnly DateOfPublishing { get; set; }

        [Required(ErrorMessage = "Numer ISBN jest wymagany.")]
        [RegularExpression(@"^\d{10,13}$", ErrorMessage = "ISBN musi składać się wyłącznie z cyfr (od 10 do 13).")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "Musisz podać przynajmniej jeden gatunek.")]
        [Display(Name = "Gatunki")]
        public IList<string> Genres { get; set; } = []; // FORMAT: genreA/genreB/.../genreX

        [MinLength(1, ErrorMessage = "Tytuł musi posiadać co najmniej 1 autora.")]
        [Display(Name = "Autorzy")]
        public IList<AuthorLookupDTO> Authors { get; set; } = [];

        [Required(ErrorMessage = "Wydawca jest wymagany.")]
        [Display(Name = "Wydawca")]
        public PublisherLookupDTO? Publisher { get; set; }
    }
}
