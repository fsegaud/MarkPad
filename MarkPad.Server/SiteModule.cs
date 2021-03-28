// SiteModule.cs

namespace MarkPad.Server
{
    using System.Collections.Generic;
    using System.Linq;
    using Nancy;

    public class SiteModule : Nancy.NancyModule
    {
        public SiteModule()
        {
            this.Get(
                "/",
                args =>
                {
                    Database.GetPosts();

                    string[] css = CssHelper.GetCssStyles(this.Context, CssHelper.Page.Site);
                    return View["index", new IndexModel(Database.GetPosts(), css)];
                });

            this.Get(
                "/skin/{skin}",
                args =>
                {
                    this.Context.Request.Session["skin"] = (string)args.skin;
                    return Response.AsRedirect($"/");
                });
        }
    }

    public abstract class BaseModel
    {
        public string Title => Program.Title;
        public string Version => Program.Version;

        protected BaseModel(string[] css)
        {
            this.Css = css;
        }

        public string[] Css
        {
            get;
        }
    }

    public class IndexModel : BaseModel
    {
        public IndexModel(IEnumerable<Post> posts, string[] css)
            : base(css)
        {
            this.Posts = posts;
        }

        public IEnumerable<Post> Posts
        {
            get;
        }

        public IO.Directory RootDir
        {
            get
            {
                IO.Directory root = new IO.Directory("/");
                root.Populate(this.Posts.ToArray());
                return root;
            }
        }
    }
}
