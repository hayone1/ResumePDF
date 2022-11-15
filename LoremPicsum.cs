using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ResumePDF
{
    internal class LoremPicsum : IComponent
    {
        public LoremPicsum(bool greyScale)
        {
            GreyScale = greyScale;
        }

        public bool GreyScale { get; init; }
        public void Compose(IContainer container)
        {
            //var url = "http://picsum.photos/300/200";
            //var url = "https://t3.ftcdn.net/jpg/03/31/21/08/360_F_331210846_9yjYz8hRqqvezWIIIcr1sL8UB4zyhyQg.jpg";
            var url = "https://img.icons8.com/glyph-neue/96/null/github.png";

            //if (GreyScale) { url += "?grayscale"; }

            //using (var client = new WebClient())
            //{
            //    var response = client.DownloadData(url);
            //    container.Image(response);
            //}

            var response = HttpHelper.DownloadFileAsync(url).Result;
            container.Image(response, ImageScaling.FitArea);
            //container.Image(Placeholders.Image, ImageScaling.FitArea);

        }
    }

}
