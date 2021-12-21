using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using LiveLines.Api.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LiveLines
{
    public static class OAuthProviderExtensions
    {
        private const string GitHubProvider = "GitHub";

        public static AuthenticationBuilder AddGitHubOAuth(this AuthenticationBuilder authBuilder, GitHubConfiguration gitHubConfiguration) =>
            authBuilder.AddOAuth(GitHubProvider, options =>
                {
                    options.ClientId = gitHubConfiguration.ClientId;
                    options.ClientSecret = gitHubConfiguration.ClientSecret;
                    options.CallbackPath = new PathString("/api/signin-github");
                    options.CorrelationCookie.SameSite = SameSiteMode.Lax;

                    options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                    options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                    options.UserInformationEndpoint = "https://api.github.com/user";

                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                    options.ClaimActions.MapJsonKey("urn:github:login", "login");
                    options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
                    options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

                    options.Events = new OAuthEvents
                    {
                        OnCreatingTicket = async context =>
                        {
                            var request =
                                new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Authorization =
                                new AuthenticationHeaderValue("Bearer", context.AccessToken);

                            var response = await context.Backchannel.SendAsync(request,
                                HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            var userJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                            context.RunClaimActions(userJson.RootElement);

                            var gitHubUsername = context.Identity.Claims.Single(x => x.Type == "urn:github:login").Value;

                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            var user = await userService.CreateUser(GitHubProvider, gitHubUsername);
                            // TODO: add users internal id to their claims
                        }
                    };
                });
    }
}