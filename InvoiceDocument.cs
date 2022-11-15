using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ResumePDF
{
    internal class InvoiceDocument : IDocument
    {
        public InvoiceModel? Model { get; } = default;
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public float LeftRatio = 1.1f;  //the ratio of the right side to the total space rg. 1.1/2
        string[] fonts => Directory.GetFiles(@"Fonts\Ubuntu\", "*.ttf");    //get font file names

        public InvoiceDocument()
        {
            //Model = model;
            foreach (var font in fonts)
            {
                //Console.WriteLine("Font locationo is" + font);
                FontManager.RegisterFont(File.OpenRead(font));

            }

        }


        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                //page.Margin(10);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / "); x.TotalPages();
                });
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().ShowOnce().Element(SingleHeader);
                //column.Item().SkipOnce().Background(Colors.Green.Lighten2).Height(40);
            });

            void SingleHeader(IContainer container)
            {
                container.Height(170)
                         //.Background(Colors.Grey.Lighten2)
                         .PaddingTop(10)
                         .PaddingRight(15)
                         .Row(row =>
                        {
                            //Left Side - Resume Brief
                            row.RelativeItem(LeftRatio)
                               .Component(DataSource.BriefComponent);

                            //row.ConstantItem(100).Height(50).Placeholder();
                            //Right Side - Contact information
                            row.RelativeItem()
                                //.Height(100)
                                .AlignRight()
                                //.Component(new LoremPicsum(false));
                                .Component(DataSource.ContactsComponent);

                        });

            }
            //container.Dynamic(new ProgressComponent());
            //container.Dynamic(new FibonacciHeader(3,5));
            //container.Element
            //container.Component(new DecorComponent());

        }

        void ComposeContent(IContainer container)
        {
            //container.PaddingVertical(10)
            const float left_RightPadding = 10f;
            
            container.BorderTop(1)
                     //.Height(50)
                     //.Background(Colors.Grey.Lighten3)
                     //.AlignCenter()
                     .PaddingTop(10)
                     //.AlignMiddle()
                     .Row(row =>
                     {
                         //Left side of Resume
                         row.RelativeItem(LeftRatio)
                            //.Background(Colors.Grey.Lighten1)
                            .Column(column =>
                            {
                                //For Education
                                column.Item().Decoration(decoration =>
                                {
                                    //title
                                    decoration.Before()
                                              .PaddingRight(left_RightPadding).Text("            " + "Education")
                                              .Style(TypographyStyles.Title2);
                                    //body
                                    decoration.Content()
                                              .Element(showEduBullets);

                                void showEduBullets(IContainer container)
                                {
                                    //inner column to place bullets
                                    container.Column(column =>
                                    {
                                        //DataSource.EducationComponent
                                        //          .ForEach(bitem => column.Item().ShowEntire().PaddingRight(10).Component(bitem));
                                        foreach (var bulletItem in DataSource.EducationComponent)
                                        {column.Item().ShowEntire().PaddingRight(10).Component(bulletItem);}

                                    });

                                }
                                });
                                //render education components
                                    //column.Item().PaddingRight(left_RightPadding).Text("            " +"Education").Style(TypographyStyles.Title2);
                                //foreach (var bulletItem in DataSource.EducationComponent)
                                //{
                                //    column.Item().ShowEntire().PaddingRight(10).Component(bulletItem);
                                //}


                                //For work experience
                                column.Item().PaddingRight(left_RightPadding).Text("            "+"WORK EXPERIENCE").Style(TypographyStyles.Title2);
                                foreach (var bulletItem in DataSource.WorkExperienceComponent)
                                {
                                    column.Item().PaddingRight(10).Component(bulletItem);

                                }

                            });

                         //right side of resume
                         row.RelativeItem()
                            .PaddingRight(10)
                            //.Background(Colors.Grey.Lighten1)
                            .Column(column =>
                            {
                                //Skills section
                                column.Item().Decoration(decoration =>
                                {
                                    //title
                                    decoration.Before()
                                              .Text(DataSource.SkillsComponent.highlightItems.Title)
                                              .Style(TypographyStyles.Title2);
                                    //body
                                    decoration.Content()
                                              .Component(DataSource.SkillsComponent);
                                });
                                //Certificates section
                                column.Item().Decoration(decoration =>
                                {
                                    //title
                                    decoration.Before()
                                              .Text(DataSource.CertificatesComponent.BasicList.Title)
                                              .Style(TypographyStyles.Title2);
                                    //body
                                    decoration.Content()
                                              .Component(DataSource.CertificatesComponent);
                                });
                                //column.Item().Text(DataSource.CertificatesComponent.BasicList.Title)
                                //             .Style(TypographyStyles.Title2);   //title
                                //column.Item().Component(DataSource.CertificatesComponent);  //body
                            });
                     });

        }
        void ComposeBrokenContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new MockAddressCompnent("From", Model.SellerAddress));
                    row.ConstantItem(50);
                    row.RelativeItem().Component(new MockAddressCompnent("To", Model.CustomerAddress));
                });

                column.Item().Element(ComposeTable);

                var totalPrice = Model.Items.Sum(x => x.Price * x.Quantity);
                column.Item()
                      .AlignRight()
                      .Text($"Grand total: {totalPrice}$")
                      .FontSize(14);

                if (!String.IsNullOrEmpty(Model.Comments))
                {
                    column.Item().PaddingTop(25).Element(ComposeComments);
                }
            });
        }

        void ComposeTable(IContainer container)
        {
            //container.Height(250)
            //         .Background(Colors.Grey.Lighten3)
            //         .AlignCenter()
            //         .AlignMiddle()
            //         .Text("Table Text")
            //         .FontSize(16);

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold())
                                        .PaddingVertical(5)
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Black);
                    }

                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Product");
                    header.Cell().Element(CellStyle).AlignRight().Text("Unit Price");
                    header.Cell().Element(CellStyle).AlignRight().Text("Qualtity");
                    header.Cell().Element(CellStyle).AlignRight().Text("Total");
                });

                foreach (var item in Model.Items)
                {
                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1)
                                        .BorderColor(Colors.Grey.Lighten3)
                                        .PaddingVertical(5);
                    }

                    table.Cell().Element(CellStyle).Text(Model.Items.IndexOf(item) + 1);
                    table.Cell().Element(CellStyle).Text(item.Name);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price}$");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity}$");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity * item.Price}$");
                }
            });
        }

        void ComposeComments(IContainer container)
        {
            container.Background(Colors.Grey.Lighten1)
                     .Padding(10).Column(column =>
                                 {
                                     column.Spacing(5);
                                     column.Item().Text("Comments").FontSize(14);
                                     column.Item().Text(Model.Comments);
                                 });
        }
    }

}
