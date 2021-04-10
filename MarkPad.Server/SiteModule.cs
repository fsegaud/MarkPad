// SiteModule.cs

namespace MarkPad.Server
{
    using System.Collections.Generic;
    using System.Linq;
    using Nancy;
    using Nancy.Security;

    public class SiteModule : Nancy.NancyModule
    {
        public SiteModule()
        {
            this.Get(
                "/",
                args =>
                {
                    if (Program.RequireAuth)
                    {
                        this.RequiresAuthentication();
                    }

                    Database.GetPosts();

                    return View["index", new IndexModel(this.Context, Database.GetPosts())];
                });

            this.Get(
                "/skin/{skin}",
                args =>
                {
                    this.Context.Request.Session["skin"] = (string)args.skin;

                    return Response.AsRedirect(this.Request.Headers.Referrer);
                });

            this.Get(
                "/share/{id:int}/{enabled:int}",
                args =>
                {
                    if (Program.RequireAuth)
                    {
                        this.RequiresAuthentication();
                    }

                    Post post = Database.GetPost((int)args.id);
                    if (post != null)
                    {
                        post.Shared = args.enabled > 0;
                        Database.UpdatePost(post);
                    }

                    return Response.AsRedirect(this.Request.Headers.Referrer);
                });

            this.Get(
                "/archive/{id:int}",
                args =>
                {
                    if (Program.RequireAuth)
                    {
                        this.RequiresAuthentication();
                    }

                    Post post = Database.GetPost((int)args.id);
                    if (post != null)
                    {
                        post.Shared = false;
                        post.Archived = true;
                        post.Name = $"{post.ShortGuid}-{post.Name}";
                        Database.UpdatePost(post);
                    }

                    return Response.AsRedirect("/");
                });
        }
    }

    public abstract class BaseModel
    {
        protected BaseModel(NancyContext context, CssHelper.Page page)
        {
            this.Css = CssHelper.GetCssStyles(context, page);
            this.IsLight = CssHelper.GetSkin(context) == CssHelper.Skin.Light;
            this.IsLogged = !Program.RequireAuth || context.IsAuthenticated();
        }

        public virtual bool EditMode => false;
        public virtual bool HasPost => false;
        public virtual bool Printable => false;

        public virtual string Subtitle => string.Empty;

        public string Title => Program.Title;
        public string Version => Program.Version;
        
        public bool IsLogged
        {
            get;
        }

        public string[] Css
        {
            get;
        }

        public bool IsLight
        {
            get;
        }
    }

    public class IndexModel : BaseModel
    {
        public IndexModel(NancyContext context, IEnumerable<Post> posts)
            : base(context, CssHelper.Page.Site)
        {
            this.Posts = posts;
        }

        public override string Subtitle => "Index";

        public IO.Directory RootDir => IO.Directory.CreateHierarchy(this.Posts.ToArray());

        public IEnumerable<Post> Posts
        {
            get;
        }
    }
}
