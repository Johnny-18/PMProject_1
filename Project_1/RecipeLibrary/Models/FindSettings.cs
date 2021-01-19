namespace RecipeLibrary.Models
{
    public class FindSettings
    {
        public string FindBy { get; set; }
        
        public string SortedBy { get; set; }
        
        public NutritionalValue FilterValue { get; set; }

        public bool IsAsc { get; set; }
        
        public bool IsMore { get; set; }
    }
}