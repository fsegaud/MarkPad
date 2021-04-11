// NewModule.cs

namespace MarkPad.Server
{
    using Nancy;
    using Nancy.Security;

    public class NewModule : Nancy.NancyModule
    {
        public NewModule()
            : base("/new")
        {
            if (Config.RequireAuth)
            {
                this.RequiresAuthentication();
            }

            this.Get(
                "/",
                args =>
                {
                    Database.GetPosts();

                    return View["new", new NewModel(this.Context)];
                });

            this.Get(
                "/{path*}",
                args =>
                {
                    Database.GetPosts();

                    return View["new", new NewModel(this.Context, args.path)];
                });

            this.Post(
                "/",
                args =>
                {
                    string path = this.Request.Form.Path;
                    string name = this.Request.Form.Name;
                    string content = this.Request.Form.content;

                    if (string.IsNullOrEmpty(name))
                    {
                        return View["new", new NewModel(this.Context, "A name is required.", name, path, content)];
                    }

                    Post post = Database.GetPost(path, name);
                    if (post != null)
                    {
                        return View["new", new NewModel(this.Context, $"Post '{post.FullPath}' already exists!", name, path, content)];
                    }

                    post = new Post(path, name);
                    post.Write(content);
                    Database.InsertPost(post);

                    return Response.AsRedirect($"/{post.FullPath}");
                });

            this.Post(
                "/{path*}",
                args =>
                {
                    string path = this.Request.Form.Path;
                    string name = this.Request.Form.Name;
                    string content = this.Request.Form.content;

                    if (string.IsNullOrEmpty(name))
                    {
                        return View["new", new NewModel(this.Context, "A name is required.", name, path, content)];
                    }

                    Post post = Database.GetPost(path, name);
                    if (post != null)
                    {
                        return View["new", new NewModel(this.Context, $"Post '{post.FullPath}' already exists!", name, path, content)];
                    }

                    post = new Post(path, name);
                    post.Write(content);
                    Database.InsertPost(post);

                    return Response.AsRedirect($"/{post.FullPath}");
                });
        }

        public class NewModel : BaseModel
        {
            public NewModel(Nancy.NancyContext context)
                : base(context, CssHelper.Page.Site)
            {
            }

            public NewModel(NancyContext context, string path)
                : base(context, CssHelper.Page.Site)
            {
                this.Path = path;
            }

            public NewModel(NancyContext context, string message, string name, string path, string content)
                : base(context, CssHelper.Page.Site)
            {
                this.Message = message;
                this.Name = name;
                this.Path = path;
                this.Content = content;
            }

            public override bool EditMode => true;

            public override string Subtitle => this.HasName ? this.Name : "New";

            public string Name
            {
                get;
            }

            public string Path
            {
                get;
            }

            public string Content
            {
                get;
            }

            public bool HasName => !string.IsNullOrEmpty(this.Name);

            public bool HasPath=> !string.IsNullOrEmpty(this.Path);

            public bool HasContent => !string.IsNullOrEmpty(this.Content);

            public string Message
            {
                get;
            }
        }
    }
}
