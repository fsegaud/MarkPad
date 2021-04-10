// EditModule.cs

namespace MarkPad.Server
{
    using Nancy;
    using Nancy.Security;

    public class EditModule : Nancy.NancyModule
    {
        public EditModule()
            : base("/edit")
        {
            if (Program.RequireAuth)
            {
                this.RequiresAuthentication();
            }

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

                    string content = this.Request.Form.Content;
                    
                    post.Modified = System.DateTime.Now;
                    post.Write(content);
                    Database.UpdatePost(post);

                    return Response.AsRedirect($"/{post.FullPath}");
                });
        }

        public class EditModel : BaseModel
        {
            public EditModel(NancyContext context, Post post)
                : base(context, CssHelper.Page.Site)
            {
                this.Post = post;
            }

            public override bool EditMode => true;

            public override string Subtitle => Post.Name;

            public Post Post
            {
                get;
            }
            
            public string Content => Post.Read();
        }
    }
}
