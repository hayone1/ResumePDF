using Newtonsoft.Json;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ResumePDF
{
    internal static class MockDataSource
    {
        private static Random random = new Random();

        public static InvoiceModel GetInvoiceDetails()
        {
            var items = Enumerable
                .Range(0, 60)
                .Select(i => GenerateRandomOrderItem())
                .ToList();

            return new InvoiceModel
            {
                InvoiceNumber = random.Next(1_000, 10_000),
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now + TimeSpan.FromDays(12),

                SellerAddress = GenerateRandomAddress(),
                CustomerAddress = GenerateRandomAddress(),

                Items = items,
                Comments = Placeholders.Paragraph()
            };
        }

        private static OrderItem GenerateRandomOrderItem()
        {
            return new OrderItem
            {
                Name = Placeholders.Label(),
                Price = (decimal)Math.Round(random.NextDouble() * 100, 2),
                Quantity = random.Next(1, 20)
            };
        }
        private static Address GenerateRandomAddress() => new Address
        {
            CompanyName = Placeholders.Name(),
            Street = Placeholders.Label(),
            City = Placeholders.Label(),
            State = Placeholders.Label(),
            Email = Placeholders.Email(),
            Phone = Placeholders.PhoneNumber()
        };
    }

    internal static class DataSource
    {
        public static IComponent LeftHeader
            = Utils.JsonToComponent(Utils.ReturnFirstFile(@"Data\HeaderLeft\", "*.json"));
            //= Utils.JsonToComponent("Data/HeaderLeft/Brief.json");

        public static IComponent RightHeader
            = Utils.JsonToComponent(Utils.ReturnFirstFile(@"Data\HeaderRight\", "*.json"));

        public static IEnumerable<(string Title, IComponent component)> ReadLeftContent()
        {
            string[] files = Directory.GetFiles(@"Data\ContentLeft\", "*.json");
            Array.Sort(files);
            foreach (var file in files)
            {
                //add component to collection if it is not null
                if (Utils.JsonToComponent(file) is IComponent component && component is not null)
                {
                    Console.WriteLine("Null check: " + (component == null));
                    Console.WriteLine("Null check is: " + (component is null));
                    var JsonName = file.BetweenStrings("-", ".json");
                    Console.WriteLine("Left Json Name : " + JsonName);
                    yield return (JsonName, component);

                }
                else
                {
                    Console.WriteLine("We got a wrong format file on the left side");
                }
            }

        }

        public static IEnumerable<(string Title, IComponent component)> ReadRightContent()
        {
            string[] files = Directory.GetFiles(@"Data\ContentRight\", "*.json");
            Array.Sort(files);
            foreach (var file in files)
            {
                //add component to collection if it is not null
                if (Utils.JsonToComponent(file) is IComponent component && component is not null)
                {
                    Console.WriteLine("Right Null check: " + (component == null));
                    Console.WriteLine("Right Null check is: " + (component is null));
                    var JsonName = file.BetweenStrings("-", ".json");
                    Console.WriteLine("Right Json Name : " + JsonName);
                    yield return (JsonName, component);

                }
                else
                {
                    Console.WriteLine("We got a wrong format file on the right side");
                }
            }

        }

        //private static BulletModelSimple? Brief = Utils.ReadJson<BulletModelSimple>("Data/Brief.json");
        //public static BulletSimpleComponent BriefComponent = new BulletSimpleComponent() { SimpleBulletModel = Brief };

        //private static BulletsModel? Education = Utils.ReadJson<BulletsModel>("Data/ContentLeft/1-Education.json");
        //public static BulletComponent[]? EducationComponent
        //        => Education?.Select(model => new BulletComponent() { bulletModel = model }).ToArray();


        //private static BulletModel[]? WorkExperience = Utils.ReadJson<BulletModel[]>("Data/WorkExperience.json");
        //public static BulletComponent[]? WorkExperienceComponent 
        //        => WorkExperience?.Select(model => new BulletComponent() { bulletModel = model }).ToArray();

        //private static ContactModel[]? Contacts = Utils.ReadJson<ContactModel[]>("Data/Contacts.json");
        //public static ContactComponent ContactsComponent = new() { contactItems = Contacts };

        ////public static BulletModel[]? WorkExperience => JsonSerializer.Deserialize<BulletModel[]>("WorkExperience.json");
        //private static HighlightModel? Skills = Utils.ReadJson<HighlightModel>("Data/Skills.json");
        //public static HighlightedItemsComponent SkillsComponent = new() { highlightItems = Skills };

        //private static HighlightModel? Interests = Utils.ReadJson<HighlightModel>("Data/Interests.json");
        //public static HighlightedItemsComponent InterestsComponent = new() { highlightItems = Interests };

        //private static ListModel? Certificates = Utils.ReadJson<ListModel>("Data/Certificates.json");
        //public static BasicListComponent CertificatesComponent = new() { BasicList = Certificates };

        //private static ListModel? Projects = Utils.ReadJson<ListModel>("Data/Projects.json");
        //public static BasicListComponent ProjectsComponent = new() { BasicList = Projects };

        //static string fileName = ;
    }

 }

