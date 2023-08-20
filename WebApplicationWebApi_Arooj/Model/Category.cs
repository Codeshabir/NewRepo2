using System.ComponentModel.DataAnnotations;

namespace WebApplicationWebApi_Arooj.Model
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
