﻿using System;
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
                email = "gosia@gmail.com",
                username = username,
                password = password
            };
            
            string userAsJson = JsonSerializer.Serialize(user);
            
            StringContent content = new StringContent(
                userAsJson, Encoding.UTF8, "application/json"
            );
            
            HttpResponseMessage responseMessage = await _client.PostAsync("http://localhost:8080/api/v1/users/login", content);

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

        public async Task<bool> Register(User user)
        {
            _client = new HttpClient();

            string userAsJson = JsonSerializer.Serialize(user);

            StringContent content = new StringContent(
                userAsJson, Encoding.UTF8, "application/json"
            );

            HttpResponseMessage responseMessage =
                await _client.PostAsync("http://localhost:8080/auth/register", content);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");

            string responseContent = await responseMessage.Content.ReadAsStringAsync();

            bool finalUser = JsonSerializer.Deserialize<bool>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (!finalUser) throw new Exception("Could not register the user.");

            return finalUser;
        }

        public async Task<string> UpdateUser(User updatedUser)
        {
           _client = new HttpClient();
            var content = new StringContent(
                JsonSerializer.Serialize(updatedUser), Encoding.UTF8, "application/json");
            var responseBack = "";
            try
            {
                var response = await _client.PostAsync("http://localhost:8080/auth/updateuser", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                responseBack = responseContent;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return responseBack;
        }
        
    }
}