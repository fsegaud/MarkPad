// AdminModule.cs

namespace MarkPad.Server
{
    using System.Linq;
    using Nancy.Security;

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
                    return this.View["admin", new AdminModel(this.Context, Database.GetPosts(true))];
                });
        }
    }

    public class AdminModel : BaseModel
    {
        public AdminModel(Nancy.NancyContext context, System.Collections.Generic.IEnumerable<Post> posts)
            : base(context, CssHelper.Page.Post)
        {
            this.Posts = posts;
        }

        public override string Subtitle => "Admin";

        public string ConfigContent => MarkPad.Parser.Json.ToHTML(Config.GetConfigFileContent());

        public System.Collections.Generic.IEnumerable<Post> Posts
        {
            get;
        }
    }
}
