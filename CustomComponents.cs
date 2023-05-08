using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Size = QuestPDF.Infrastructure.Size;

namespace ResumePDF
{
    internal class ProgressComponent : IDynamicComponent<int>
    {
        public int State { get; set; }

        public DynamicComponentComposeResult Compose(DynamicContext context)
        {
            var content = context.CreateElement(container =>
            {
                var width = context.AvailableSize.Width * context.PageNumber / context.TotalPages;

                container.Background(Colors.Blue.Lighten2)
                         .Height(25)
                         .Width(width)
                         .Background(Colors.Green.Lighten2);

            });

            return new DynamicComponentComposeResult
            {
                Content = content,
                HasMoreContent = false
            };

        }
    }

    internal class MockAddressCompnent : IComponent
    {
        public MockAddressCompnent(string title, Address address)
        {
            Title = title;
            Address = address;
        }

        private string Title { get; }
        private Address Address { get; }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item()
                      .BorderBottom(1)
                      .PaddingBottom(5)
                      .Text(Title)
                      .SemiBold();

                column.Item().Text(Address.CompanyName);
                column.Item().Text(Address.Street);
                column.Item().Text($"{Address.City}, {Address.State}");
                column.Item().Text(Address.Email);
                column.Item().Text(Address.Phone);
            });
        }

    }

    internal class FibonacciHeader : IDynamicComponent<FibonacciHEaderState>
    {
        public FibonacciHEaderState State { get; set; }

        public readonly string[] ColorsTable =
        {
            Colors.Red.Lighten2,
            Colors.Orange.Lighten2,
            Colors.Green.Lighten2,
        };

        public FibonacciHeader(int previous, int current)
        {
            State = new()
            {
                Previous = previous,
                Current = current
            };
        }

        public DynamicComponentComposeResult Compose(DynamicContext context)
        {
            var content = context.CreateElement(container =>
            {
                var colorIndex = State.Current % ColorsTable.Length;
                var color = ColorsTable[colorIndex];

                var ratio = (float)State.Current / State.Previous;

                container.Background(color)
                         .Height(50)
                         .AlignMiddle()
                         .AlignCenter()
                         .Text($"{State.Current} / {State.Previous} = {ratio:N5}");
            });

            //assign NEW state
            State = new()
            {
                Previous = State.Current,
                Current = State.Previous + State.Current
            };

            return new DynamicComponentComposeResult
            {
                Content = content,
                HasMoreContent = false //so each page will have its own content
            };
        }

    }


    internal class DecorComponent : IComponent
    {
        public void Compose(IContainer container)
        {
            container.Decoration(decoration =>
            {
                decoration.Before()
                          .Background(Colors.Grey.Medium)
                          //.Padding(10)
                          .Text("Notes");
                //.FontSize(10);

                decoration.After()
                          .Background(Colors.Grey.Medium)
                          //.Padding(10)
                          .Text("Notes bottom");
                //.FontSize(10);

                //decoration.Content()
                //          .Background(Colors.Grey.Lighten3)
                //          //.Padding(10)
                //          .ExtendVertical()
                //          .Text(Placeholders.LoremIpsum());
            });
        }
    }

    internal class BulletBase
    {
        protected const float leftRatio = 0.08f;   //ratio the left side of the bulleted component takes
        protected const float LeftpaddingRight = 5f;    //right padding for the left column
        protected const float LeftHeight = 35f; //height of bullet that is on the left side
        public string PrimaryColor { get; init; } = Colors.Black;
        public string AccentColor { get; init; } = Colors.Blue.Accent2;

    }
    internal class BulletSimpleComponent : BulletBase, IComponent
    {
        

        public BulletModelSimple? SimpleBulletModel { get; init; }
        public void Compose(IContainer container)
        {
            //var textStyleWithFallback = TextStyle.Default
            //                                    .FontFamily(Fonts.Candara)
            //                                    .FontSize(9)
            //                                    .Fallback(x => x.FontFamily("Segoe UI Emoji"));
            //left side - compose the square to represent the bullet
            container.Row(row =>
            {
                //compose the square to represent the bullet
                row.RelativeItem(leftRatio) 
                    //.Border(1)
                   //.AlignTop()
                   //.DebugArea()
                   .PaddingRight(LeftpaddingRight)
                   //.PaddingTop(20)
                   .Height(LeftHeight)
                   .Background(PrimaryColor);

                //right side - the brief
                row.RelativeItem()
                    .MaxHeight(130)
                    //.Border(1)
                //    .Background(Colors.Grey.Lighten3)
                   .Text(text =>
                   {
                       text.DefaultTextStyle(TypographyStyles.Normal);
                       text.AlignLeft();
                       //text.ParagraphSpacing(-50.1f);
                       //text.ParagraphSpacing(0.5f);

                       text.Line(SimpleBulletModel.Title).Style(TypographyStyles.Title);
                       text.Line(SimpleBulletModel.Headline).FontSize(12).FontColor(AccentColor);
                       //text.EmptyLine();
                       text.Line(SimpleBulletModel.Paragraph);

                   });
            });

        }
    }
    internal class BulletsComponent : BulletBase, IComponent
    {
        public string AccentColor { get; init; } = Colors.Blue.Accent2;

        //data model for bullet item
        public BulletsModel? bulletsModel { get; init; }
        public void Compose(IContainer container)
        {
            //column to place bullets
            container.Column(column =>
            {
                //each bullet data will take a row
                foreach (var bulletModel in bulletsModel.Bullets)
                { 
                    column.Item().ShowEntire().Element(CreateElement);
                    
                    void CreateElement (IContainer container)
                    {
                        {
                        //each row has 2 columns | left(bullet) & right(text)
                        container.Row(row =>
                        {
                            //left side - the Bullet itself
                            row.RelativeItem(leftRatio)
                            //.AlignTop()
                            //.DebugArea()
                            .PaddingRight(LeftpaddingRight)
                            .Height(LeftHeight)
                            .Background(AccentColor);
                            //.Border(1)

                            //right side - Text
                            row.RelativeItem()
                            //.Background(Colors.Grey.Lighten2)
                            .Text(text =>
                            {
                                text.DefaultTextStyle(TypographyStyles.Normal.FontSize(12));
                                text.AlignLeft();
                                text.ParagraphSpacing(1);
                                //headline
                                text.Line(bulletModel.Qualification).Style(TypographyStyles.Headline);
                                //headline2
                                text.Line(bulletModel.Company).Style(TypographyStyles.Headline2);
                                //duration
                                text.Span($"{bulletModel.Duration.start} - {bulletModel.Duration.end}")
                                    .Style(TypographyStyles.AccentStyle);
                                text.Span(" | ");
                                //location
                                text.Span(bulletModel.Location)
                                    .Style(TypographyStyles.AccentStyle);
                                text.EmptyLine();
                                text.Line(bulletModel.SubTitle)
                                    .Style(TypographyStyles.AccentStyle2);

                                //inner bullets paragraphs
                                foreach (var paragraph in bulletModel.paragraphs)
                                {
                                    //text.Span("◉ ").FontColor(AccentColor);
                                    text.Span("• ").FontColor(AccentColor);
                                    //text.Span(" ").Style(TypographyStyles.AccentStyle);
                                    text.Span(paragraph)
                                        .Style(TypographyStyles.Normal.FontSize(10.5f));
                                    text.EmptyLine();
                                }

                            });
                        });
                    }
                    }
                }


            });

        }

    }

    internal class ContactComponent : IComponent
    {
        [Required]
        public ContactModel[]? contactItems { get; init; }

        public void Compose(IContainer container)
        {
            container.ContentFromRightToLeft()
                     //.Border(1)
                     //.Height(150)
                     .Table(table =>
                     {
                         table.ColumnsDefinition(columns =>
                         {
                             //define 2 columns
                             columns.RelativeColumn(0.1f);
                             columns.RelativeColumn();
                         });
                         foreach (ContactModel item in contactItems)
                         {
                             //Console.WriteLine("Icon Text is: " + item.Text);
                             byte[] iconImage;
                             iconImage = File.ReadAllBytes(item.IconLink);
                            //iconImage = HttpHelper.DownloadFileAsync(item.IconLink).Result;   //read from internet version
                             //try
                             //{
                             //}
                             //catch (Exception ex)
                             //{
                             //    iconImage = File.ReadAllBytes("Images/web.png");
                             //}


                             table.Cell().Height(25)
                                         //.Border(1)
                                         //.Height(18)
                                         .Width(15)
                                         //.AlignCenter()
                                         .AlignTop()
                                         //.AlignMiddle()
                                         .Image(iconImage, ImageScaling.FitArea);
                             table.Cell().Hyperlink(item.Link)
                                         .Text(item.Text)
                                         .Style(TypographyStyles.Normal);
                         }
                         //for (int i = 0; i < contactItems.Length; i++)
                         //{
                         //    Console.WriteLine("Icon Text is: " + contactItems[i].Text);
                         //    var iconImage = HttpHelper.DownloadFileAsync(contactItems[i].IconLink).Result;

                         //    table.Cell().Border(1).Element(IconBlock).Image(iconImage, ImageScaling.FitArea);
                         //    table.Cell().Hyperlink(contactItems[i].Link).Text(contactItems[i].Text);
                         //}
                     });
        }

        private IContainer IconBlock(IContainer container) =>
            container.Height(18)
                     .Width(18)
                     .AlignCenter()
                     .AlignMiddle();
    
    }

    internal class HighlightedItemsComponent : IComponent
    {
        [Required]
        public HighlightModel highlightItems = new();
        //max no of characters the component can contain without wrapping
        const int maxChar = 64;
        const int columnNo = 20; //there is maximum of 2 items per row
        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                //deine column properties
                table.ColumnsDefinition(columns =>
                {
                    //create 8 columns each with 1/8 size
                    Enumerable.Range(1, columnNo)
                              .Select(no => { columns.RelativeColumn(1); return -1; })
                              .ToList();
                });

                //place the Items
                foreach (var item in highlightItems.Items)
                {
                    int charNo = item.Count();
                    //get quotient division as nnumber of columns to take
                    //float columnFloat = (float)item.Count() * (float)columnNo / (float)maxChar;
                    //int columnCount = (int)Math.Ceiling(columnFloat);
                    int columnCount = (item.Count() * columnNo / maxChar) + 2;
                    //Console.WriteLine("Column count: " + (uint)columnCount + $": {item}");

                    //fill cells with highlights
                    table.Cell()
                         .ColumnSpan((uint)columnCount)
                         //.Background(Colors.Grey.Darken2)
                         .Height(24)
                         .ShowIf(charNo <= maxChar)
                         //text with rounded rectangle background
                         .Layers(layers =>
                         {
                            var paddingAmt = 2.5f;
                            layers.Layer()
                                  .Padding(paddingAmt)
                                  //.AlignTop()
                                  .Canvas((canvas, size) =>
                            {
                                DrawRoundedRectangle(highlightItems.Color, size, canvas, highlightItems.IsStroke);
                            });
                             layers.PrimaryLayer()
                                     //.PaddingVertical(5)
                                     //.PaddingHorizontal(5)
                                     .AlignMiddle()
                                     .AlignCenter()
                                     .Text(item).Style(TypographyStyles.Normal)
                                     .FontColor(highlightItems.IsStroke ? Colors.Black : Colors.White);
                                    //.FontSize(9);
                         });
                }
            });
        }

        void DrawRoundedRectangle(string color, Size size, SKCanvas canvas, bool isStroke = false)
        {
            using var paint = new SKPaint
            {
                Color = SKColor.Parse(color),
                IsStroke = isStroke,
                StrokeWidth = 1,
                IsAntialias = true
            };

            canvas.DrawRoundRect(0, 0, size.Width, size.Height, 4,3f, paint);
        }
    }

    internal class BasicListComponent : IComponent
    {
        [Required]
        public ListModel BasicList = new();
        public void Compose(IContainer container)
        {

            container.Column(column =>
             {
                 column.Spacing(5);
                 foreach (var listItem in BasicList.ListItems)
                 {
                     column.Item().ShowEntire().Text($"{listItem.Item}\n" +
                                           listItem.Child)
                                               .Style(TypographyStyles.Normal.FontSize(10.5f));
                     //column.Item().DebugArea()
                     //             .Text(text =>
                     //{
                     //    text.DefaultTextStyle(TypographyStyles.Normal.FontSize(10));
                     //    text.Line(listItem.Item.Trim());
                     //    //text.Line(listItem.Child);
                     //    //text.ParagraphSpacing(-5f);
                     //});
                 }
             });
        }
    }


}
