using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SEP6_Frontend.Authentication;
using SEP6_Frontend.Models;

namespace SEP6_Frontend.Data.Implementation {
    public class ImplMovieService : IMovie {
        private HttpClient _client;

        public async Task<Movie> MovieInformation(string movieID)
        {
            _client = new HttpClient();

            var responseMessage = await _client.GetAsync($"https://movies-backend.azurewebsites.net/api/v1/movies/moviedata?id={movieID}");

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");

            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            var finalMovie = JsonSerializer.Deserialize<Movie>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (finalMovie == null) throw new Exception("Fetching movie went wrong");
            
            return finalMovie;
        }
        public async Task<List<SearchResponse>> SearchForMovies(string filter)
        {
            _client = new HttpClient();
            var responseMessage = new HttpResponseMessage();

            var body = new StringContent(filter, Encoding.UTF8, "application/json");
            
            try
            {
                responseMessage =
                    await _client.PostAsync($"https://movies-backend.azurewebsites.net/api/v1/movies/search", body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var content = await responseMessage.Content.ReadAsStringAsync();
            List<SearchResponse> success = null;
            try
            {
                success = JsonSerializer.Deserialize<List<SearchResponse>>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return success;
        }
        
        public async Task<List<MovieShort>> GetRandomMovies()
        {
            _client = new HttpClient();

            var responseMessage = await _client.GetAsync($"https://movies-backend.azurewebsites.net/api/v1/movies/getRandomMovies");

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");

            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            var finalMovies = JsonSerializer.Deserialize<List<MovieShort>>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (finalMovies == null) throw new Exception("Fetching movies went wrong");
            
            return finalMovies;
        }
        
        public async Task<bool> SetFavoriteMovie(string movieID, string email)
        {
            _client = new HttpClient();
            var responseMessage = new HttpResponseMessage();

            string custom = "{\"email\":\"" + email + "\",\"movieID\":\"" + movieID + "\"}";

            var body = new StringContent(custom, Encoding.UTF8, "application/json");
            try
            {
                responseMessage =
                    await _client.PostAsync($"https://movies-backend.azurewebsites.net/api/v1/movies/favouriteMovie", body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var content = await responseMessage.Content.ReadAsStringAsync();
            bool success = false;
            try
            {
                success = JsonSerializer.Deserialize<bool>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return success;
        }
        
        public async Task<bool> IsThisMovieFav(string movieID, string email)
        {
            _client = new HttpClient();
            var responseMessage = new HttpResponseMessage();

            string custom = "{\"email\":\"" + email + "\",\"movieID\":\"" + movieID + "\"}";

            var body = new StringContent(custom, Encoding.UTF8, "application/json");
            try
            {
                responseMessage =
                    await _client.PostAsync($"https://movies-backend.azurewebsites.net/api/v1/movies/isFavouriteMovie", body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var content = await responseMessage.Content.ReadAsStringAsync();
            bool success = false;
            try
            {
                success = JsonSerializer.Deserialize<bool>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return success;
        }
        
        public async Task<List<MovieShort>> GetFavoriteMovies(string email)
        {
            _client = new HttpClient();

            var responseMessage = await _client.GetAsync($"https://movies-backend.azurewebsites.net/api/v1/movies/getFavouriteMovies?email={email}");

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");

            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            var favMovies = JsonSerializer.Deserialize<List<MovieShort>>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (favMovies == null) throw new Exception("Fetching movies went wrong");
            
            return favMovies;
        }
        
        public async Task<List<DataItem>> GetRatingOverYears()
        {
            _client = new HttpClient();

            var responseMessage = await _client.GetAsync($"https://movies-backend.azurewebsites.net/api/v1/movies/getRatingOverYears");

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");

            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            var ratingOverYears = JsonSerializer.Deserialize<List<DataItem>>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (ratingOverYears == null) throw new Exception("Fetching rating went wrong");
            
            return ratingOverYears;
        }
        
        public async Task<List<Person>> GetBestRatingDirectors()
        {
            _client = new HttpClient();

            var responseMessage = await _client.GetAsync($"https://movies-backend.azurewebsites.net/api/v1/movies/getBestRatedDirectors");

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");

            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            var bestRatedDirectors = JsonSerializer.Deserialize<List<Person>>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (bestRatedDirectors == null) throw new Exception("Fetching best rated directors went wrong");
            
            return bestRatedDirectors;
        }
        
        public async Task<List<Person>> GetBestRatingActors()
        {
            _client = new HttpClient();

            var responseMessage = await _client.GetAsync($"https://movies-backend.azurewebsites.net/api/v1/movies/getBestRatedActors");

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");

            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            var bestRatedActors = JsonSerializer.Deserialize<List<Person>>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (bestRatedActors == null) throw new Exception("Fetching best rated actors went wrong");
            
            return bestRatedActors;
        }
    }
}