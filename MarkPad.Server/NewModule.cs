// NewModule.cs

using Nancy;

namespace MarkPad.Server
{
    public class NewModule : Nancy.NancyModule
    {
        public NewModule()
            : base("/new")
        {
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

                    return Response.AsRedirect($"/");
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

                    return Response.AsRedirect($"/");
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
