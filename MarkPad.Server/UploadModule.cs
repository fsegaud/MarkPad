// UploadModule.cs

namespace MarkPad.Server
{
    using System.Linq;
    using Nancy.Security;

    public class UploadModule : Nancy.NancyModule
    {
        public UploadModule()
            : base("/upload")
        {
            if (Program.RequireAuth)
            {
                this.RequiresAuthentication();
            }

            this.Get(
                "/",
                args =>
                {
                    return View["upload", new UploadModel(this.Context, CssHelper.Page.Site)];
                });

            this.Post(
                "/",
                args =>
                {
                    Nancy.HttpFile file = this.Request.Files.Single();

                    string fileExtension = System.IO.Path.GetExtension(file.Name).TrimStart('.');
                    if (!System.IO.Path.HasExtension(file.Name) || !Program.UploadExtensions.Contains(fileExtension))
                    {
                        return View["upload", new UploadModel(true, this.Context, CssHelper.Page.Site)];
                    }

                    string filepath = System.IO.Path.Combine("content/uploads", $"{System.Guid.NewGuid().ToString().Substring(0, 8)}-{file.Name}");

                    if (!System.IO.Directory.Exists("content/uploads"))
                    {
                        System.IO.Directory.CreateDirectory("content/uploads");
                    }

                    using (System.IO.FileStream fs = System.IO.File.OpenWrite(filepath))
                    {
                        file.Value.CopyTo(fs);
                    }

                    return View["upload", new UploadModel($"![](/{filepath.FixPathSlashes()})", this.Context, CssHelper.Page.Site)];
                });
        }

        public class UploadModel : BaseModel
        {
            public UploadModel(Nancy.NancyContext context, CssHelper.Page page)
                : base(context, page)
            {
            }

            public UploadModel(string link, Nancy.NancyContext context, CssHelper.Page page)
                : base(context, page)
            {
                this.Link = link;
            }

            public UploadModel(bool error, Nancy.NancyContext context, CssHelper.Page page)
                : base(context, page)
            {
                this.Error = error;
            }

            public override string Subtitle => "Upload";

            public bool Uploaded => !string.IsNullOrEmpty(this.Link);

            public bool Error
            {
                get;
            }

            public string Link
            {
                get;
            }
        }
    }
}