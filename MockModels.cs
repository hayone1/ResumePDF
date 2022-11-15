using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ResumePDF
{
    public record Duration(string start, string end);
    public record BasicItem(string Item, string Child);
    //public record ContactItem(string start, string end);

    public class BulletModelSimple
    {
        public string? Title { get; init; }
        public string? Headline { get; init; }
        public string? Paragraph { get; init; }

    }
    public class BulletModel
    {
        //public string? Title { get; init; }
        [Required]
        public string? Qualification { get; init; }
        [Required]
        public string? Company { get; init; }
        //public (string start, string end) Duration { get; init; }
        [Required]
        public Duration? Duration { get; init; }
        public string? Location { get; init; }
        public string? SubTitle { get; init; }
        [Required]
        public string[]? paragraphs { get; init; }
    }


    public class InvoiceModel
    {
        public int InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        public Address SellerAddress { get; set; }
        public Address CustomerAddress { get; set; }

        public List<OrderItem> Items { get; set; }
        public string Comments { get; set; }
    }

    public class OrderItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Address
    {
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public object Email { get; set; }
        public string Phone { get; set; }
    }

    public record ContactModel
    {
        public string? Text { get; init; }
        public string? Link { get; init; }
        public string? IconLink { get; init; }
    }

    public class HighlightModel
    {
        public string Title { get; init; } = "Skills";
        public string Color { get; init; } = Colors.Black;
        public bool IsStroke { get; init; } = false;
        public string[]? Items { get; init; }

        public string this[int i]
        {
            get { return Items[i]; }
            set { Items[i] = value; }
        }

        }
    
    public class ListModel
    {
        public string Title { get; init; } = "SKILLS";
        public BasicItem[]? ListItems { get; init; }
    }
    


}
