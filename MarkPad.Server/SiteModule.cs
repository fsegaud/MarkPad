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
                    Post post = Database.GetPost((int)args.id);
                    if (post != null)
                    {
                        post.Shared = args.enabled > 0;
                    }

                    Database.UpdatePost(post);

                    return Response.AsRedirect(this.Request.Headers.Referrer);
                });
        }
    }

    public abstract class BaseModel
    {
        protected BaseModel(NancyContext context, CssHelper.Page page)
        {
            this.Css = CssHelper.GetCssStyles(context, page);
            this.IsLight = CssHelper.GetSkin(context) == CssHelper.Skin.Light;
        }

        public virtual bool HasPost => false;
        public virtual bool Printable => false;

        public string Title => Program.Title;
        public string Version => Program.Version;
        public bool IsLogged => true; // TODO: implement.

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

        public IEnumerable<Post> Posts
        {
            get;
        }

        public IO.Directory RootDir => IO.Directory.CreateHierarchy(this.Posts.ToArray());
    }
}
