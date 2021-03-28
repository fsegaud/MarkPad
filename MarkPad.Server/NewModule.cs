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

                    string[] css = CssHelper.GetCssStyles(this.Context, CssHelper.Page.Site);
                    return View["new", new NewModel(css)];
                });

            this.Post(
                "/",
                args =>
                {
                    string path = this.Request.Form.Path;
                    string name = this.Request.Form.Name;
                    string content = this.Request.Form.content;

                    string[] css = CssHelper.GetCssStyles(this.Context, CssHelper.Page.Site);

                    if (string.IsNullOrEmpty(name))
                    {
                        return View["new", new NewModel("A name is required.", name, path, content, css)];
                    }

                    Post post = Database.GetPost(path, name);
                    if (post != null)
                    {
                        return View["new", new NewModel($"Post '{post.FullPath}' already exists!", name, path, content, css)];
                    }

                    post = new Post(path, name);
                    post.Write(content);
                    Database.InsertPost(post);

                    return Response.AsRedirect($"/");
                });
        }

        public class NewModel : BaseModel
        {
            public NewModel(string[] css)
                : base(css)
            {
            }

            public NewModel(string message, string name, string path, string content, string[] css)
                : base(css)
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
