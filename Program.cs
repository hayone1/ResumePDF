// See https://aka.ms/new-console-template for more information
using QuestPDF.Fluent;
using QuestPDF.Previewer;
using ResumePDF;
using System.Diagnostics;

Console.WriteLine("Previewer Starting...");

//var filePath = "Mockinvoice.pdf";

//var model = MockDataSource.GetInvoiceDetails();
var document = new InvoiceDocument();
//document.GeneratePdf(filePath);
document.ShowInPreviewer();

// generate images as dynamic list of images
//IEnumerable<byte[]> images = document.GenerateImages();

// generate images and save them as files with provided naming schema
//document.GenerateImages(i => $"Images/demo/page-{i}.png");

//Process.Start("explorer.exe", filePath);    