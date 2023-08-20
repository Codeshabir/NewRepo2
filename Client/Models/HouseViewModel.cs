namespace Client.Models
{
   public class HouseViewModel
    {
        public List<HouseDTO> Houses { get; set; }
        public List<Category> Categories { get; set; }
        public int? SelectedCategoryId { get; set; } // For storing the selected category
    }
}
