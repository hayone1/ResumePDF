// See https://aka.ms/new-console-template for more information
using QuestPDF.Fluent;
using QuestPDF.Previewer;
using ResumePDF;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

//var filePath = "Mockinvoice.pdf";

//var model = MockDataSource.GetInvoiceDetails();
var document = new InvoiceDocument();
//document.GeneratePdf(filePath);
document.ShowInPreviewer();

//Process.Start("explorer.exe", filePath);    