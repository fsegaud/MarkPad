// MarkPadBootstrapper.cs

namespace MarkPad.Server
{
    using Nancy;

    public class MarkPadBootstrapper : Nancy.DefaultNancyBootstrapper, Nancy.IRootPathProvider
    {
        protected override Nancy.IRootPathProvider RootPathProvider => this;

        public override void Configure(Nancy.Configuration.INancyEnvironment environment)
        {
            base.Configure(environment);
            environment.Tracing(Program.EnableTraces, Program.EnableTraces);
        }


        public string GetRootPath()
        {
            return System.IO.Directory.GetCurrentDirectory();
        }

        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            Nancy.Session.CookieBasedSessions.Enable(pipelines);
        }

        //protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        //{
        //    // base not called on purpose.
        //}

        //protected override void ConfigureRequestContainer(Nancy.TinyIoc.TinyIoCContainer container, NancyContext context)
        //{
        //    base.ConfigureRequestContainer(container, context);

        //    container.Register<Nancy.Authentication.Forms.IUserMapper, MarkPadUserMapper>();
        //}

        //protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        //{
        //    base.RequestStartup(container, pipelines, context);

        //    Nancy.Authentication.Forms.FormsAuthenticationConfiguration authConfig = new Nancy.Authentication.Forms.FormsAuthenticationConfiguration()
        //    {
        //        RedirectUrl = "~/login", UserMapper = container.Resolve<Nancy.Authentication.Forms.IUserMapper>()
        //    };

        //    Nancy.Authentication.Forms.FormsAuthentication.Enable(pipelines, authConfig);
        //}
    }
}
