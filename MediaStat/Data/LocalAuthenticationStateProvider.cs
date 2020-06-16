using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MediaStat.Data
{
    public class LocalAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _storageService;

        public LocalAuthenticationStateProvider(ILocalStorageService storageService)
        {
            _storageService = storageService;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (_storageService != null && await _storageService.ContainKeyAsync("User"))
                {
                    var userInfo = await _storageService.GetItemAsync<LocalUserInfo>("User");

                    var claims = new[]
                    {
                    new Claim("Email",userInfo.Email),
                    new Claim("FirstName",userInfo.FirstName),
                    new Claim("LastName",userInfo.LastName ),
                    new Claim("AccessToken",userInfo.AccessToken),
                    new Claim(ClaimTypes.NameIdentifier,userInfo.Id),
                    new Claim("IsAdmin",(userInfo.IsAdmin)?"true":"false"),
                };

                    var identity = new ClaimsIdentity(claims, "BearerToken");
                    var user = new ClaimsPrincipal(identity);
                    var state = new AuthenticationState(user);
                    //Services.AppState appState = new Services.AppState();
                    //appState.IsAdmin = userInfo.IsAdmin;
                    Services.AppState.PutData(userInfo.Email, true);
                    NotifyAuthenticationStateChanged(Task.FromResult(state));

                    return state;
                }


            }
            catch(Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
            finally
            {
                //Services.AppState.PutData("J1@Live.com", true);
            }
            return new AuthenticationState(new ClaimsPrincipal());

            ////////            var identity1 = new ClaimsIdentity(new[]
            ////////{
            ////////                        new Claim(ClaimTypes.Name, "mrfibuli"),
            ////////                    }, "Fake authentication type");
            ////////            return new AuthenticationState(new ClaimsPrincipal(identity1));

            //            var identity = new ClaimsIdentity(new[]
            //{
            //            new Claim(ClaimTypes.Name, "mrfibuli"),
            //        }, "Fake authentication type");

            //            var user = new ClaimsPrincipal(identity);
            //            var state = new AuthenticationState(user);
            //            NotifyAuthenticationStateChanged(Task.FromResult(state));

            //            return state;
        }

        public async Task LogoutAsync()
        {
            await _storageService.RemoveItemAsync("User");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }
    }
}
