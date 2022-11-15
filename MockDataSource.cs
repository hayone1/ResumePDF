using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
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
        //public static BulletComponent[] ContentBullets = new BulletComponent[]
        //{
        //    new BulletComponent(){ bulletModel = TecnotreeSystems },
        //    new BulletComponent(){ bulletModel = TecnotreeSystems }
        //};
        //public static BulletModel TecnotreeSystems =>
        //new BulletModel
        //{
        //    //Title = "WORK EXPERIENCE",
        //    Qualification = "Systems Integrator Intern",
        //    Company = "TecnoTree Nigeria",
        //    Duration = new("11 / 2021", "Present"),
        //    Location = "Victoria Island, Lagos",
        //    SubTitle = "Achievements/Tasks",
        //    paragraphs = new string[]
        //    {
        //        "Currently create value with Tecnotree Nigeria via\r\nprovisioning and maintenance of various backend and\r\ncustomer-facing services for MTN Nigeria including CRM, CLM and whole sale billing services.\n",
        //        "Collaborated with teammates to design a future proof\r\nand scalable solution that provides and automatic and\r\nseamless payment option for MTN postpaid customers.\n",
        //        "Facilitated the launch of a new blended learning solution\r\nactively participating in diagnosis and debugging while\r\nconducting UAT tests, resulting in a well rounded\r\nsolution which we names MOMENTS.\n",
        //        "Productively Contributed to various updates, clarification\r\nand stakeholders meetings, generally resulting in better\r\nstakeholder management and client relations."
        //    }
        //};

        
        private static BulletModelSimple? Brief = Utils.ReadJson<BulletModelSimple>("Brief.json");
        public static BulletSimpleComponent BriefComponent = new BulletSimpleComponent() { SimpleBulletModel = Brief };

        private static BulletModel[]? Education = Utils.ReadJson<BulletModel[]>("Education.json");
        public static BulletComponent[]? EducationComponent
                => Education?.Select(model => new BulletComponent() { bulletModel = model }).ToArray();


        private static BulletModel[]? WorkExperience = Utils.ReadJson<BulletModel[]>("WorkExperience.json");
        public static BulletComponent[]? WorkExperienceComponent 
                => WorkExperience?.Select(model => new BulletComponent() { bulletModel = model }).ToArray();

        private static ContactModel[]? Contacts = Utils.ReadJson<ContactModel[]>("Contacts.json");
        public static ContactComponent ContactsComponent = new() { contactItems = Contacts };

        //public static BulletModel[]? WorkExperience => JsonSerializer.Deserialize<BulletModel[]>("WorkExperience.json");
        private static HighlightModel? Skills = Utils.ReadJson<HighlightModel>("Skills.json");
        public static HighlightedItemsComponent SkillsComponent = new() { highlightItems = Skills };

        private static ListModel? Certificates = Utils.ReadJson<ListModel>("Certificates.json");
        public static BasicListComponent CertificatesComponent = new() { BasicList = Certificates };

            //static string fileName = ;
        }

 }

