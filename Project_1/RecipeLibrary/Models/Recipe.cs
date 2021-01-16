using System.Collections.Generic;
using RecipeLibrary.Interfaces;

namespace RecipeLibrary.Models
{
    public class Recipe : IModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public List<Ingredients> Ingredients { get; set; }
        
        public NutritionalValue NutritionalValue { get; set; }
    }
}