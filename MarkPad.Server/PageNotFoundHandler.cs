// PageNotFoundHandler.cs

using Nancy.Extensions;

namespace MarkPad.Server
{
    public class PageNotFoundHandler : Nancy.ErrorHandling.IStatusCodeHandler
    {
        private Nancy.ViewEngines.IViewFactory factory;

        public PageNotFoundHandler(Nancy.ViewEngines.IViewFactory factory)
        {
            this.factory = factory;
        }

        public bool HandlesStatusCode(Nancy.HttpStatusCode statusCode, Nancy.NancyContext context)
        {
            return statusCode == Nancy.HttpStatusCode.NotFound;
        }

        public void Handle(Nancy.HttpStatusCode statusCode, Nancy.NancyContext context)
        {
            context.Response = this.factory.RenderView("404", new { Exception = context.GetExceptionDetails(), StatusCode = (int)statusCode }, new Nancy.ViewEngines.ViewLocationContext { Context = context });
        }
    }
}
