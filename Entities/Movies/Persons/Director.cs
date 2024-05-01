namespace API.Entities.Movies.Persons
{
    public class Director : Person
    {
        public ICollection<Movie> Movies { get; set; }
        public DirectorImage DirectorImage { get; set; }
    }
}
