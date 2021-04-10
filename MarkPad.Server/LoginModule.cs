// LoginModule.cs

using Nancy.Authentication.Forms;
using Nancy.Extensions;

namespace MarkPad.Server
{
    public class LoginModule : Nancy.NancyModule
    {
        public LoginModule()
        {
            this.Get(
                "/login",
                args =>
                {
                    return View["login", new { Title = Program.Title, Errored = this.Request.Query.error.HasValue }];
                });

            this.Post(
                "/login",
                args =>
                {
                    System.Guid? guid = MarkPadUserMapper.ValidateUser((string)this.Request.Form.Username, (string)this.Request.Form.Password);
                    if (guid == null)
                    {
                        return this.Context.GetRedirect($"~/login?error=true");
                    }

                    System.DateTime? expiry = null;
                    if (this.Request.Form.RememberMe.HasValue)
                    {
                        expiry = System.DateTime.Now.AddDays(7);
                    }

                    return this.LoginAndRedirect(guid.Value, expiry);
                });

            this.Get("/logout", args => this.LogoutAndRedirect(this.Context.Request.Headers.Referrer));
        }
    }
}
