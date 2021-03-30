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

                    return View["edit", new EditModel(this.Context, post)];
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
            public EditModel(NancyContext context, Post post)
                : base(context, CssHelper.Page.Site)
            {
                this.Post = post;
            }

            public Post Post
            {
                get;
            }

            public string Content => Post.Read();
        }
    }
}
