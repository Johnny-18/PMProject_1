using RecipeLibrary.Interfaces;

namespace RecipeLibrary.Models
{
    public class Ingredients :IModel
    {
        public int Id { get; set; }
        
        public int Count { get; set; }

        public string UnitsOfMeasurement { get; set; }
        
        public string Name { get; set; }

        public Ingredients(int id, int count, string unitsOfMeasurement, string name)
        {
            Id = id;
            Count = count;
            UnitsOfMeasurement = unitsOfMeasurement;
            Name = name;
        }
    }
}