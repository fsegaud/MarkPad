// CssHelper.cs

namespace MarkPad.Server
{
    public static class CssHelper
    {
        public enum Skin
        {
            Light = 0,
            Dark
        }

        public enum Page
        {
            Site = 0,
            Post
        }

        public static string[] GetCssPaths(Nancy.NancyContext context, Page page)
        {
            CssHelper.Skin skin = CssHelper.Skin.Light;
            if (context.Request.Session["skin"] != null)
            {
                string skinName = (string)context.Request.Session["skin"];
                skin = System.Enum.Parse<CssHelper.Skin>(skinName, true);
            }

            switch (skin)
            {
                case Skin.Light:
                    switch (page)
                    {
                        case Page.Site:
                            return new[] { "styles/site-light.css" };

                        case Page.Post:
                            return new[] { "styles/site-light.css", "styles/markdown-light.css" };

                        default:
                            throw new System.ArgumentOutOfRangeException(nameof(page), page, null);
                    }

                case Skin.Dark:
                    switch (page)
                    {
                        case Page.Site:
                            return new[] { "styles/site-dark.css" };

                        case Page.Post:
                            return new[] { "styles/site-dark.css", "styles/markdown-dark.css" };

                        default:
                            throw new System.ArgumentOutOfRangeException(nameof(page), page, null);
                    }

                default:
                    throw new System.ArgumentOutOfRangeException(nameof(skin), skin, null);
            }
        }

        public static string[] GetCssStyles(Nancy.NancyContext context, Page page)
        {
            string[] cssPaths = CssHelper.GetCssPaths(context, page);
            string[] cssStyles = new string[cssPaths.Length];
            for (int index = 0; index < cssPaths.Length; index++)
            {
                cssStyles[index] = System.IO.File.ReadAllText(cssPaths[index]);
            }

            return cssStyles;
        }
    }
}
