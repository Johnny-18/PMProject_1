using System;

namespace RecipeLibrary.Models
{
    public class NutritionalValue : IComparable<NutritionalValue>
    {
        public double Proteins { get; set; }
        
        public double Fats { get; set; }
        
        public double Carbohydrates { get; set; }
        
        public NutritionalValue(){}
        
        public NutritionalValue(double proteins, double fats, double carbohydrates)
        {
            Proteins = proteins;
            Fats = fats;
            Carbohydrates = carbohydrates;
        }

        public double GetCalories()
        {
            return Proteins * 4 + Fats * 9 + Carbohydrates * 4;
        }

        public override string ToString()
        {
            return $"Proteins: {Proteins}, fats: {Fats}, carbohydrates: {Carbohydrates}.";
        }

        public int CompareTo(NutritionalValue other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            
            var thisCalories = GetCalories();
            var otherCalories = other.GetCalories();

            if (thisCalories > otherCalories)
                return 1;

            if (thisCalories < otherCalories)
                return -1;

            return 0;
        }
    }
}