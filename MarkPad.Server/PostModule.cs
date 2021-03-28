// PostModule.cs

namespace MarkPad.Server
{
    public class PostModule : Nancy.NancyModule
    {
        public PostModule()
            : base("/post")
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

                    System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                    string html = MarkPad.Parser.Markdown.ToHTML(post.Read());

                    string[] css = CssHelper.GetCssStyles(this.Context, CssHelper.Page.Post);
                    return View["post", new PostModel(post, html, sw.ElapsedMilliseconds, css)];
                });

            this.Get(
                "/{id:int}",
                args =>
                {
                    Post post = Database.GetPost((int)args.id);
                    if (post == null)
                    {
                        return Nancy.HttpStatusCode.NotFound;
                    }

                    System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                    string html = MarkPad.Parser.Markdown.ToHTML(post.Read());

                    string[] css = CssHelper.GetCssStyles(this.Context, CssHelper.Page.Post);
                    return View["post", new PostModel(post, html, sw.ElapsedMilliseconds, css)];
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

                    return post.Read();
                });

            this.Get(
                "/{id:int}/raw",
                args =>
                {
                    Post post = Database.GetPost((int)args.id);
                    if (post == null)
                    {
                        return Nancy.HttpStatusCode.NotFound;
                    }

                    return post.Read();
                });
        }
    }

    public class PostModel : BaseModel
    {
        public PostModel(Post post, string content, long generatedTime, string[] css)
            : base(css)
        {
            this.Post = post;
            this.Content = content;
            this.GeneratedTime = generatedTime;
        }

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
