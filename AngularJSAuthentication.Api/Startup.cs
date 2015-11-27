using System;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using AngularJSAuthentication.Api.Repository;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(AngularJSAuthentication.Api.Startup))]
namespace AngularJSAuthentication.Api
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions GoogleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();


            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider(new AccountRepository())
            };


            GoogleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "592613624399-a3gr6vveaocnptgvv6738rmnk0pb5cev.apps.googleusercontent.com",
                ClientSecret = "FqNKKib_BP7dsNYBoJa8NwUC",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(GoogleAuthOptions);


            FacebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "806191272841558",
                AppSecret = "1a8241e9d46c4a5e393ae51f265a3489",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(FacebookAuthOptions);

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
}