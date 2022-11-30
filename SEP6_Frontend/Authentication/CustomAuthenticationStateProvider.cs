using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SEP6_Frontend.Data;
using SEP6_Frontend.Models;

namespace SEP6_Frontend.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IUserService _userService;

        private User _cachedUser;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, IUserService userService)
        {
            this._jsRuntime = jsRuntime;
            this._userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (_cachedUser == null)
            {
                string userAsJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userAsJson))
                {
                    User tmp = JsonSerializer.Deserialize<User>(userAsJson);
                    await ValidateLogin(tmp.username, tmp.password);
                    identity = SetupClaimsForUser(tmp);
                }
            }
            else
            {
                identity = SetupClaimsForUser(_cachedUser);
            }

            ClaimsPrincipal cachedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        public async Task ValidateLogin(string username, string password) {
            //if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password)) throw new Exception("Enter credentials");
            //if (string.IsNullOrEmpty(username)) throw new Exception("Enter username");
            //if (string.IsNullOrEmpty(password)) throw new Exception("Enter password");

            ClaimsIdentity identity = new ClaimsIdentity();
            try {
                User user = await _userService.ValidateLogin(username, password);
                identity = SetupClaimsForUser(user);
                _cachedUser = user;
                User userToSerialize = new User
                {
                    username = user.username,
                    password = user.password
                };
                string serialisedData = JsonSerializer.Serialize(userToSerialize);
                await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
            } 
            catch (Exception e){
                throw new Exception("Wrong credentials");
            }

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public async Task<string> Register(User user) {
            try {
                return await _userService.Register(user);
            }
            catch (Exception e) {
                throw new Exception("Wrong credentials ");
            }
        }

        public void Logout() {
            _cachedUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public ClaimsIdentity SetupClaimsForUser(User user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.username));
            //claims.Add(new Claim("userId", user.userId.ToString()));
            //claims.Add(new Claim("role", user.role.ToString()));

            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }

        //public int GetUserId() {
        //    return _cachedUser.userId;
        //}
        
        public string GetDisplayName() {
            return _cachedUser.username;
        }

        public async Task<User> GetUser()
        {
            return _cachedUser;
        }

        public async Task<string> UpdateUser(User user)
        {
          var response = await _userService.UpdateUser(user);
          if (response.Equals("Update of the profile successful"))
          {
              await ValidateLogin(user.username, user.password);
          }
          return response;
        }
    }
}