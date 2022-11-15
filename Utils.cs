using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ResumePDF
{
    internal static class Utils
    {
        public static T? ReadJson<T>(string fileName)
        {
            // open the file contents
            string jsonContents = File.ReadAllText(fileName);
            JsonSerializerOptions options = new JsonSerializerOptions { AllowTrailingCommas = true };
        // deserialize the file contents, not the fileName
            T? result = JsonSerializer.Deserialize<T>(jsonContents, options);
            return result;
        }

        public static void ForEach<T>(this IEnumerable<T> list, System.Action<T> action)
        {
            foreach (T item in list)
                action(item);
        }
    }

    // single typography class can help with keeping document look&feel consistent
    internal static class TypographyStyles
    {
        public static TextStyle Normal => TextStyle
            .Default
            .FontSize(9)
            .FontColor(Colors.Black)
            .FontFamily("Ubuntu")
            .FontColor(Colors.Black)
            .Fallback(x => x.FontFamily("Segoe UI Emoji"));

        public static TextStyle Headline => Normal
            .FontSize(12).Bold();
        public static TextStyle Headline2 => Normal
            .FontSize(12).NormalWeight();
        public static TextStyle AccentStyle => Normal
            .FontColor(Colors.Blue.Accent2);
        public static TextStyle AccentStyle2 => AccentStyle
            .Italic().FontSize(8);
        
        public static TextStyle Title => Normal
            .FontSize(23);
        public static TextStyle Title2 => Normal
            .FontSize(14).Bold();

    }


}
