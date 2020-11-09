using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace DatePot.Models
{
    public class Films
    {
        public class FilmIndex
        {
            public List<FilmList> FilmList { get; set; }
            public List<UserList> UserList { get; set; }
        }
        public class FilmList
        {
            [Key]
            public int FilmID { get; set; }
            [Display(Name = "Name")]
            public string FilmName { get; set; }
            [Display(Name = "Release Date")]
            public string ReleaseDate { get; set; }
            [Display(Name = "Added Date")]
            public string AddedDate { get; set; }
            [Display(Name = "Genre")]
            public string GenreText { get; set; }
            public int GenreID { get; set; }
            public string Watched { get; set; }
            public string UserName { get; set; }
            public List<RandomFilm> RandomFilm { get; set; }
        }
        public class UserList
        {
            [Key]
            public int UserID { get; set; }
            [Display(Name = "Name")]
            public string UserName { get; set; }
        }
        public class NewFilm
        {
            [Required]
            [DisplayName("Added by")]
            public string AddersName { get; set; }
            [Required]
            [MaxLength(100, ErrorMessage = "Names must not exceed 100 characters")]
            [MinLength(2, ErrorMessage = "Names must be at least 2 characters long")]
            [DisplayName("Name")]
            public string FilmName { get; set; }
            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Release Date")]
            public DateTime ReleaseDate { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Please select a genre from the list")]
            [DisplayName("Genre")]
            public int GenreID { get; set; }
            public bool Watched { get; set; }
            public List<Genres> Genres { get; set; }
            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Date Added")]
            public DateTime AddedDate { get; set; }
        }
        public class Genres
        {
            public int GenreID { get; set; }
            public string GenreText { get; set; }
        }
        public class Directors
        {
            public int DirectorID { get; set; }
            public string DirectorName { get; set; }
        }
        public class FilmDetails
        {
            [Key]
            public int FilmID { get; set; }
            [Display(Name = "Name")]
            public string FilmName { get; set; }
            [Display(Name = "Release Date")]
            public DateTime ReleaseDate { get; set; }
            [Display(Name = "Genre")]
            public int GenreID { get; set; }
            public bool Watched { get; set; }
            public int AddedByID { get; set; }
            [DisplayName("Added by")]
            public string AddersName { get; set; }
            [DisplayName("Director")]
            public int DirectorID { get; set; }
        }
        public class UpdateFilmDetails: FilmDetails
        {
        }
        public class RandomFilm : FilmDetails { }
        public class NewGenre
        {
            [Required]
            [DisplayName("Genre")]
            public string GenreText { get; set; }
        }
        public class NewDirector
        {
            [Required]
            [DisplayName("Director")]
            public string DirectorText { get; set; }
        }

    }
}
