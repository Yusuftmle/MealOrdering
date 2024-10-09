using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace MealOrdering.Client.Utils
{
	public class AuthStateProvider : AuthenticationStateProvider
	{
		private readonly ILocalStorageService localStorageService;
		private readonly HttpClient client;
		private readonly AuthenticationState anonymous;
        public AuthStateProvider(ILocalStorageService LocalStorageService,HttpClient client)
		{
			localStorageService = LocalStorageService;
			this.client = client;
			anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
		}

		public async override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			string apiToken = await localStorageService.GetItemAsStringAsync("token");

			if (string.IsNullOrEmpty(apiToken))
				return anonymous;
			String Email = await localStorageService.GetItemAsStringAsync("email");

			var cp = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, Email) }, "JwtAuthType"));
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",apiToken);

			return new AuthenticationState(cp);
		}

		public void NotifyUserLogin(string Email)
		{
	    var cp = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, Email) }, "JwtAuthType"));
		var authState=Task.FromResult(new AuthenticationState(cp));

			NotifyAuthenticationStateChanged(authState);
		}

		public void NotifyUserLogOut()
		{
			var authState = Task.FromResult(anonymous);
			NotifyAuthenticationStateChanged(authState);
		}
	}
}
