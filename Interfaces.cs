using QuestPDF.Drawing;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumePDF
{
    //internal interface IDocument
    //{
    //    DocumentMetadata GetMetadata();
    //    void Compose(IDocumentContainer container);
    //}

    //internal interface IComponent
    //{
    //    void Compose(IContainer container);
    //}
    public static class HttpHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<byte[]> DownloadFileAsync(string uri)
        {
            Uri? uriResult;

            if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
                throw new InvalidOperationException("URI is invalid.");

             return await _httpClient.GetByteArrayAsync(uri);
            //File.WriteAllBytes(outputPath, fileBytes);
        }
    }
}
