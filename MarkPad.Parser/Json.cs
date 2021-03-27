// Json.cs

namespace MarkPad.Parser
{
    using System.Text.RegularExpressions;

    public class Json
    {
        public static string ToHTML(string code)
        {
            /* CSS **************
             * span.json-key
             * span.json-string
             * span.json-numeric
             * span.json-keyword
             ********************/

            // Keys.
            code = Regex.Replace(code, "\\\".+?\\\"(?=\\s?:)", m => $"<span class=\"json-key\">{m}</span>", RegexOptions.Multiline);

            // String.
            code = Regex.Replace(code, "(?<=:\\s?.*?)\\\".+?\\\"", m => $"<span class=\"json-string\">{m}</span>", RegexOptions.Multiline);

            // Numeric
            code = Regex.Replace(code, @"(?<=:\s?)[\d.]+", m => $"<span class=\"json-numeric\">{m}</span>", RegexOptions.Multiline);

            // Boolean
            code = Regex.Replace(code, @"(?<=:\s?)(true|false|null)", m => $"<span class=\"json-keyword\">{m}</span>", RegexOptions.Multiline);

            return code;
        }
    }
}
