namespace RecipeLibrary.Models
{
    public class FindSettings
    {
        public string SortedBy { get; set; }
        
        public NutritionalValue NutritionalValue { get; set; }

        public bool IsAsc { get; set; }
        
        public bool IsMore { get; set; }
    }
}