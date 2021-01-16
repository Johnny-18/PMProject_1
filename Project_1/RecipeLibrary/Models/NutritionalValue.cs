using RecipeLibrary.Interfaces;

namespace RecipeLibrary.Models
{
    public class NutritionalValue : IModel
    {
        public int Id { get; set; }
        
        public double Proteins { get; set; }
        
        public double Fats { get; set; }
        
        public double Carbohydrates { get; set; }
        
        public NutritionalValue(int id, double proteins, double fats, double carbohydrates)
        {
            Id = id;
            Proteins = proteins;
            Fats = fats;
            Carbohydrates = carbohydrates;
        }
    }
}