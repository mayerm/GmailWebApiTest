using Microsoft.AspNetCore.Mvc;
//using Google.Apis.Auth.AspNetCore3;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Drive.v3;
//using Google.Apis.Services;
//using Google.Apis.Gmail.v1;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Google;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authorization;


namespace WebApplicationTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
/*
        //[HttpGet("GetDrive")]
        //[GoogleScopedAuthorize(DriveService.ScopeConstants.Drive)]
        //public Task<string> GetDrive()
        //{


        //    return Task.FromResult("");
        //}

        [HttpGet("GetEmail")]
        [GoogleScopedAuthorize(GmailService.ScopeConstants.MailGoogleCom)]
        public async Task<string> GetEmail()
        {

            GoogleCredential credential = await _auth.GetCredentialAsync();
            var service = new GmailService(new BaseClientService.Initializer
            {
                ApplicationName = "WebApplicationTest",
                HttpClientInitializer = credential
            });
            var files = service.Users.Messages.List("me");

            return await Task.FromResult("");
        }

        [HttpGet("GetLogin")]
        public async Task GetLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        [Authorize]
        public IActionResult Login()
        {
            return Ok("Login");
        }


        private async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(x => new
            {
                x.Issuer,
                x.OriginalIssuer
            });

            return Ok(claims);
        }*/
    }
}
