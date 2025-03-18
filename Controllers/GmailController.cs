using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
//using Http = System.Web.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmailController : ControllerBase
    {
        private readonly IGoogleAuthProvider _auth;
        public GmailController([FromServices] IGoogleAuthProvider auth)
        {
            _auth = auth;
        }

        [HttpGet("GetMailList/{userId}")]
        [GoogleScopedAuthorize(GmailService.ScopeConstants.MailGoogleCom)]
        public async Task<IActionResult> GetMailList(string userId)
        {
            //Change GoogleCredential cred for UserCredential
            GoogleCredential cred = await Task.Run(() =>_auth.GetCredentialAsync(), CancellationToken.None);
            var service = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = cred
            });
            var listRequest = service.Users.Messages.List(userId ?? "me");
            listRequest.LabelIds = "INBOX";
            listRequest.IncludeSpamTrash = false;
            listRequest.Q = "is:unread";
            var listResponse = listRequest.Execute();
            var msg = service.Users.Messages.Get("me", listResponse.Messages[0].Id).Execute();
            //List of unread mails' headers from inbox
            return new JsonResult(msg.Payload.Headers);
        }

        
    }
}
