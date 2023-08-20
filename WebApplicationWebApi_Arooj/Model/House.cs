using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationWebApi_Arooj.Model
{
    public class House
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Images { get; set; } // Store image URLs or paths
        public decimal Rent { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageInBase64 { get; set; }

        public int CategoryId { get; set; } // Foreign key to Category table
        //[ForeignKey("CategoryId")]
        //public Category Category { get; set; } // Navigation property to Category
    }

}

//public class House1
//{
//    public int Id { get; set; }
//    public string? Title { get; set; }
//    public string? Description { get; set; }
//    public string? Images { get; set; } // Store image URLs or paths
//    public decimal Rent { get; set; }
//    public bool IsAvailable { get; set; }
//    public List<IFormFile>? ImageFiles { get; set; } // List of uploaded image files

//}



