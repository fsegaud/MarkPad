// EditModule.cs

using Nancy;

namespace MarkPad.Server
{
    public class EditModule : Nancy.NancyModule
    {
        public EditModule()
            : base("/edit")
        {
            this.Get(
                "/{id:int}",
                args =>
                {
                    Post post = Database.GetPost((int)args.id);
                    if (post == null)
                    {
                        return Nancy.HttpStatusCode.NotFound;
                    }

                    string[] css = CssHelper.GetCssStyles(this.Context, CssHelper.Page.Site);
                    return View["edit", new EditModel(post, css)];
                });

            this.Post(
                "/{id:int}",
                args =>
                {
                    Post post = Database.GetPost((int)args.id);
                    if (post == null)
                    {
                        return Nancy.HttpStatusCode.NotFound;
                    }

                    string path = this.Request.Form.Path;
                    string content = this.Request.Form.Content;
                    
                    post.Path = path;
                    post.Modified = System.DateTime.Now;
                    post.Write(content);
                    Database.UpdatePost(post);

                    return Response.AsRedirect($"/");
                });
        }

        public class EditModel : BaseModel
        {
            public EditModel(string[] css)
                : base(css)
            {
            }

            public EditModel(Post post, string[] css)
                : this(post, string.Empty, css)
            {
            }

            public EditModel(Post post, string message, string[] css)
                : base(css)
            {
                this.Post = post;
                this.Message = message;
            }

            public Post Post
            {
                get;
            }

            public string Content => Post.Read();

            public string Message
            {
                get;
            }
        }
    }
}
