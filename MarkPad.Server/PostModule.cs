// PostModule.cs

using Nancy;

namespace MarkPad.Server
{
    using Nancy.Security;

    public class PostModule : Nancy.NancyModule
    {
        public PostModule()
            : base("/")
        {

            this.Get(
                "/{fullpath*}",
                args =>
                {
                    Post post = Database.GetPost((string)args.fullpath);
                    if (post == null)
                    {
                        return Nancy.HttpStatusCode.NotFound;
                    }

                    if (!post.Shared && Config.RequireAuth)
                    {
                        this.RequiresAuthentication();
                    }

                    System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                    string html = MarkPad.Parser.Markdown.ToHTML(post.Read());

                    return View["post", new PostModel(this.Context, post, html, sw.ElapsedMilliseconds)];
                });

            this.Get(
                "/id/{id:int}",
                args =>
                {
                    Post post = Database.GetPost((int)args.id);
                    if (post == null)
                    {
                        return Nancy.HttpStatusCode.NotFound;
                    }

                    if (!post.Shared && Config.RequireAuth)
                    {
                        this.RequiresAuthentication();
                    }

                    System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                    string html = MarkPad.Parser.Markdown.ToHTML(post.Read());

                    return View["post", new PostModel(this.Context, post, html, sw.ElapsedMilliseconds)];
                });

            this.Get(
                "/{fullpath*}/raw",
                args =>
                {
                    Post post = Database.GetPost((string)args.fullpath);
                    if (post == null)
                    {
                        return Nancy.HttpStatusCode.NotFound;
                    }

                    if (!post.Shared && Config.RequireAuth)
                    {
                        this.RequiresAuthentication();
                    }

                    return post.Read();
                });

            this.Get(
                "/id/{id:int}/raw",
                args =>
                {
                    Post post = Database.GetPost((int)args.id);
                    if (post == null)
                    {
                        return Nancy.HttpStatusCode.NotFound;
                    }

                    if (!post.Shared && Config.RequireAuth)
                    {
                        this.RequiresAuthentication();
                    }

                    return post.Read();
                });

            this.Get(
                "/{fullpath*}/print",
                args =>
                {
                    Post post = Database.GetPost((string)args.fullpath);
                    if (post == null)
                    {
                        return Nancy.HttpStatusCode.NotFound;
                    }

                    if (!post.Shared && Config.RequireAuth)
                    {
                        this.RequiresAuthentication();
                    }

                    System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                    string html = MarkPad.Parser.Markdown.ToHTML(post.Read());

                    return View["post", new PostModel(this.Context, post, html, sw.ElapsedMilliseconds, true)];
                });

            this.Get(
                "/id/{id:int}/print",
                args =>
                {
                    Post post = Database.GetPost((int)args.id);
                    if (post == null)
                    {
                        return Nancy.HttpStatusCode.NotFound;
                    }

                    if (!post.Shared && Config.RequireAuth)
                    {
                        this.RequiresAuthentication();
                    }

                    System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                    string html = MarkPad.Parser.Markdown.ToHTML(post.Read());

                    return View["post", new PostModel(this.Context, post, html, sw.ElapsedMilliseconds, true)];
                });
        }
    }

    public class PostModel : BaseModel
    {
        private bool printable;

        public PostModel(Nancy.NancyContext context, Post post, string content, long generatedTime, bool printable = false)
            : base(context, CssHelper.Page.Post)
        {
            this.printable = printable;
            this.Post = post;
            this.Content = content;
            this.GeneratedTime = generatedTime;
        }

        public override bool HasPost => this.Post != null;

        public override bool Printable => this.printable && this.HasPost;

        public override string Subtitle => this.HasPost ? this.Post.Name : "New";

        public Post Post
        {
            get;
        }

        public string Content
        {
            get;
        }

        public long GeneratedTime
        {
            get;
        }
    }
}
