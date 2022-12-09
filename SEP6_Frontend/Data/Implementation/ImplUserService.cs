using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using System.Threading.Tasks;
using SEP6_Frontend.Models;

namespace SEP6_Frontend.Data.Implementation
{
    public class ImplUserService : IUserService
    {
        private User user;
        private HttpClient _client;

        public async Task<User> ValidateLogin(string username, string password)
        {
            _client = new HttpClient();

            User user = new()
            {
                email = "",
                username = username,
                password = password
            };
            
            string userAsJson = JsonSerializer.Serialize(user);
            
            StringContent content = new StringContent(
                userAsJson, Encoding.UTF8, "application/json"
            );
            
            HttpResponseMessage responseMessage = await _client.PostAsync("https://movies-backend.azurewebsites.net/api/v1/users/login", content);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");

            string responseContent = await responseMessage.Content.ReadAsStringAsync();


            User finalUser = JsonSerializer.Deserialize<User>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = {new JsonStringEnumConverter()}
            });

            if (finalUser == null) throw new Exception("Wrong credentials.");

            return finalUser;
        }

        public async Task<string> Register(User user)
        {
            _client = new HttpClient();

            string userAsJson = JsonSerializer.Serialize(user);

            StringContent content = new StringContent(
                userAsJson, Encoding.UTF8, "application/json"
            );

            HttpResponseMessage responseMessage =
                await _client.PostAsync("https://movies-backend.azurewebsites.net/api/v1/users/register", content);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");

            string responseContent = await responseMessage.Content.ReadAsStringAsync();

            return responseContent;
        }
    }
}