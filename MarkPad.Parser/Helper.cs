// Helper.cs

namespace MarkPad.Parser
{
    public static class Helper
    {
        public static string FormatCode(string code, string language)
        {
            switch (language.ToLowerInvariant())
            {
                case "c#":
                case "cs":
                case "csharp":
                    return CSharp.ToHTML(code);

                case "json":
                    return Json.ToHTML(code);

                default:
                    return code;
            }
        }
    }
}
