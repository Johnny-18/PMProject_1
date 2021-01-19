namespace RecipeLibrary.Models
{
    public class Ingredient
    {
       public int Count { get; set; }

        public string UnitsOfMeasurement { get; set; }
        
        public string Name { get; set; }
        
        public Ingredient(){}

        public Ingredient(int count, string unitsOfMeasurement, string name)
        {
            Count = count;
            UnitsOfMeasurement = unitsOfMeasurement;
            Name = name;
        }

        public override string ToString()
        {
            return $"Ingredient: {Name}, count: {Count} {UnitsOfMeasurement}.";
        }
    }
}