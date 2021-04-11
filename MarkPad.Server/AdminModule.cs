// AdminModule.cs

using Nancy.Security;

namespace MarkPad.Server
{
    public class AdminModule : Nancy.NancyModule
    {
        public AdminModule()
            : base("/admin")
        {
            if (Config.RequireAuth)
            {
                this.RequiresAuthentication();
            }

            this.Get(
                "/",
                args =>
                {
                    return this.View["admin", new AdminModel(this.Context)];
                });
        }
    }

    public class AdminModel : BaseModel
    {
        public AdminModel(Nancy.NancyContext context)
            : base(context, CssHelper.Page.Site)
        {
        }
    }
}
