/*
Copyright 2020 Google Inc

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    https://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebApplicationTest
{
    public class Startup
    {
        // Set the TEST_WEB_CLIENT_SECRET_FILENAME configuration key to point to the client ID json file.
        // This can be set on appsettings.json or as an environment variable.
        // You can read more about configuring ASP.NET Core applications here:
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1
        private const string ClientSecretFilenameKey = "Test1:Api:ClientSecretFile";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSession();
            // This loads the OAuth 2.0 client ID used by this application from a client ID json file.
            // You can use any mechanism you want to store and retrieve your client ID information, as long
            // as it is secured. If your client ID information is leaked any other app can pose as your own.
            var clientSecrets = GoogleClientSecrets.FromFile(Configuration[ClientSecretFilenameKey]).Secrets;
            //services.AddCors(x => x.AddPolicy(name: "CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddCors(x => 
                x.AddPolicy(
                    name: "FrontEndCorsPolicy", 
                    builder => 
                        builder
                        .WithOrigins("http://localhost:49730")
                        //.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        ));
            // This configures Google.Apis.Auth.AspNetCore3 for use in this app.
            services
                .AddAuthentication(o =>
                {
                    // This forces challenge results to be handled by Google OpenID Handler, so there's no
                    // need to add an AccountController that emits challenges for Login.
                    o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                    // This forces forbid results to be handled by Google OpenID Handler, which checks if
                    // extra scopes are required and does automatic incremental auth.
                    o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                    // Default scheme that will handle everything else.
                    // Once a user is authenticated, the OAuth2 token info is stored in cookies.
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddGoogleOpenIdConnect(options =>
                {
                    //gmail-sender-n-receiver credentials
                    //options.ClientId = clientSecrets.ClientId;
                    //options.ClientSecret = clientSecrets.ClientSecret;
                    //front-en-email-auth Credentials
                    options.ClientId = "280034515082-b648p37avjlb8n16jjci95ghuge7rra0.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // This is not a production app so we always use the developer exception page.
            // You should ensure that your app uses the correct error page depending on the environment
            // it runs in.
            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseSession();
            app.UseCors("FrontEndCorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            app.UseHttpsRedirection();
        }
    }
}
