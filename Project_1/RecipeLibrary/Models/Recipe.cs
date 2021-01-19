using System.Collections.Generic;

namespace RecipeLibrary.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public List<Ingredient> Ingredients { get; set; }
        
        public NutritionalValue NutritionalValue { get; set; }
        
        public Recipe(){}

        public Recipe(int id, string name, List<Ingredient> ingredients, NutritionalValue nutritionalValue)
        {
            Id = id;
            Name = name;
            Ingredients = ingredients;
            NutritionalValue = nutritionalValue;
        }
        
        public Recipe(string name, List<Ingredient> ingredients, NutritionalValue nutritionalValue)
        {
            Name = name;
            Ingredients = ingredients;
            NutritionalValue = nutritionalValue;
        }

        public override string ToString()
        {
            string stringRecipe = $"\nRecipe #{Id} name: {Name}\n";
            foreach (var ingredient in Ingredients)
            {
                stringRecipe += ingredient + "\n";
            }
            
            if(NutritionalValue != null)
                stringRecipe += NutritionalValue.ToString();
            
            return stringRecipe + "\n";
        }
    }
}