// CSharp.cs

namespace MarkPad.Parser
{
    using System.Text.RegularExpressions;

    public class CSharp
    {
        public static string ToHTML(string code)
        {
            /* CSS *****************
             * span.csharp-string
             * span.csharp-comment
             * span.csharp-keyword
             * span.csharp-class
             * span.csharp-function
             ***********************/

            // Mark keywords.
            code = Regex.Replace(code, @"(?<=^|[^\w\d@])(public|protected|private|internal|class|struct|interface|enum|new|if|else|for|foreach|in|while|continue|break|return|switch|in|out|ref|value|this|base|override|virtual|abstract)(?=[^\w\d])", m => $"%KW%{m}", RegexOptions.Multiline);
            code = Regex.Replace(code, @"(?<=^|[^\w\d@])(int|uint|byte|sbyte|long|ulong|float|double|signed|unsigned|bool|true|false|string|null|var|default)(?=[^\w\d])", m => $"%KW%{m}", RegexOptions.Multiline);

            // Mark functions.
            code = Regex.Replace(code, @"[\w\d_]+(?=\()", m => $"%FUNC%{m}", RegexOptions.Multiline);

            // Mark classes.
            code = Regex.Replace(code, @"(?<=(class|interface|struct|enum)\s)[\w\d_]+", m => $"%CLASS%{m}", RegexOptions.Multiline);

            // Strings.
            code = Regex.Replace(code, "\\\".*?\\\"", m => $"<span class=\"csharp-string\">{m.Value.Replace("%KW%", string.Empty).Replace("%FUNC%", string.Empty).Replace("%CLASS%", string.Empty)}</span>", RegexOptions.Multiline);

            // Comments.
            code = Regex.Replace(code, "//.*?(?=$)", m => $"<span class=\"csharp-comment\">{m.Value.Replace("%KW%", string.Empty).Replace("%FUNC%", string.Empty).Replace("%CLASS%", string.Empty)}</span>", RegexOptions.Multiline);

            // Keywords.
            code = Regex.Replace(code, @"%KW%(?<label>\w+)", m => $"<span class=\"csharp-keyword\">{m.Groups["label"]}</span>", RegexOptions.Multiline);

            // Functions.
            code = Regex.Replace(code, @"%FUNC%(?<label>[\w\d_]+)", m => $"<span class=\"csharp-function\">{m.Groups["label"]}</span>", RegexOptions.Multiline);

            // Classes.
            code = Regex.Replace(code, @"%CLASS%(?<label>[\w\d_]+)", m => $"<span class=\"csharp-class\">{m.Groups["label"]}</span>", RegexOptions.Multiline);

            // Cleanup.
            code = Regex.Replace(code, @"%(KW|FUNC|CLASS)%", m => string.Empty);

            return code;
        }
    }
}