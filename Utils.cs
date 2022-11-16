using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
//using System.Text.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ResumePDF
{
    internal static class Utils
    {
        //public static bool TryConvertJson<T>()
        public static string ReturnFirstFile(string directory, string extension)
        {
            string[] files = Directory.GetFiles(directory, extension);
            Array.Sort(files);
            return files[0];
        }
        public static IComponent JsonToComponent(string fileName)
        {
            // open the file contents
            string jsonContents = File.ReadAllText(fileName);
            //parses the Json into the right class type or returns a null otherwise
            if (jsonContents.TryParseJson(out BulletModelSimple simpleBullet))
            {
                return new BulletSimpleComponent() { SimpleBulletModel = simpleBullet };
            }
            if (jsonContents.TryParseJson(out BulletsModel bullets))
            {
                return new BulletsComponent() { bulletsModel = bullets };
            }
            if (jsonContents.TryParseJson(out ContactModel[] Contacts))
            {
                return new ContactComponent() { contactItems = Contacts }; ;
            }
            if (jsonContents.TryParseJson(out HighlightModel highlightItems))
            {
                return new HighlightedItemsComponent() { highlightItems = highlightItems };
            }
            if (jsonContents.TryParseJson(out ListModel basicList))
            {
                return new BasicListComponent() { BasicList = basicList };
            }
            return null; //if none matches

        }
        public static bool TryParseJson<T>(this string @this, out T result)
        {
            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            //JsonSerializerOptions options = new JsonSerializerOptions { AllowTrailingCommas = true };
            // deserialize the file contents, not the fileName
            //T? result = JsonSerializer.Deserialize<T>(jsonContents, options);
            result = JsonConvert.DeserializeObject<T>(@this, settings);
            return success;
        }

        public static void ForEach<T>(this IEnumerable<T> list, System.Action<T> action)
        {
            foreach (T item in list)
                action(item);
        }

        public static string BetweenStrings(this string FullString, string LeftString, string RightString)
        {
            var from = FullString.LastIndexOf(LeftString) + LeftString.Length;
            var to = FullString.IndexOf(RightString);
            return FullString[from..to];  // THE_TARGET_STRING
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
            .Fallback(x => x.FontFamily(Fonts.SegoeUI))
            .Fallback(x => x.FontFamily(Fonts.Calibri));

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
