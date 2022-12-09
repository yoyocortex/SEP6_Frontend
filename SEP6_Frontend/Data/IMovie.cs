using System.Collections.Generic;
using System.Threading.Tasks;
using SEP6_Frontend.Models;

namespace SEP6_Frontend.Data {
    public interface IMovie {
        Task<Movie> MovieInformation(string movieID);
        Task<List<SearchResponse>> SearchForMovies(string filter);
        Task<List<MovieShort>> GetRandomMovies();
        Task<bool> SetFavoriteMovie(string movieID, string email);
        Task<bool> IsThisMovieFav(string movieID, string email);
        Task<List<MovieShort>> GetFavoriteMovies(string email);
        Task<List<DataItem>> GetRatingOverYears();
        Task<List<Person>> GetBestRatingDirectors();
        Task<List<Person>> GetBestRatingActors();
    }
}