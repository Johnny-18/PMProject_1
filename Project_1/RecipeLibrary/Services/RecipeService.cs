using System;
using System.Collections.Generic;
using System.Linq;
using RecipeLibrary.Interfaces;
using RecipeLibrary.Models;

namespace RecipeLibrary.Services
{
    public class RecipeService : IService<Recipe>, ISaveWork
    {
        private List<Recipe> _recipes;
        private readonly FileWorker _fw;

        public RecipeService()
        {
            _fw = new FileWorker();
            _recipes = _fw.Deserialize<Recipe>();
            
            if(_recipes == null)
                _recipes = new List<Recipe>();
        }
        
        public void Add(Recipe obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
            
            _recipes.Add(obj);
            SaveWork();
        }

        public void Delete(Recipe obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
            
            _recipes.Remove(obj);
            SaveWork();
        }

        public void Add(string name, List<Ingredient> ingredients, NutritionalValue nutritionalValue)
        {
            Add(new Recipe( GetUniqueId(), name, ingredients, nutritionalValue));
        }

        public void Delete(string name)
        {
            var recipe = GetObject(name);
            if (recipe == null)
                throw new ArgumentException("Not found!");
            
            Delete(recipe);
        }

        public void Update(string name, Recipe newObj)
        {
            var recipe = GetObject(name);
            if (recipe == null)
                throw new ArgumentException("Not found!");

            recipe.Name = newObj.Name;
            recipe.Ingredients = newObj.Ingredients;
            recipe.NutritionalValue = newObj.NutritionalValue;
            
            SaveWork();
        }

        public List<Recipe> GetObjects(string name)
        {
            if (name == null)
                throw new ArgumentNullException();
            
            if (name == "")
                return _recipes;

            return _recipes.Where(x => x.Name == name).ToList();
        }

        public Recipe GetObject(string name)
        {
            return _recipes.FirstOrDefault(x => x.Name == name);
        }

        public List<Recipe> GetByIngredient(string searchStr)
        {
            if (searchStr == null)
                throw new ArgumentNullException();

            if (searchStr == "")
                return _recipes;
            
            return _recipes.Where(x => x.Ingredients.Exists(y => y.Name == searchStr)).ToList();
        }

        public void SaveWork()
        {
            _fw.Serialize(_recipes);
        }

        private int GetUniqueId()
        {
            if (_recipes == null || _recipes.Count == 0)
                return 1;

            return _recipes.Last().Id + 1;
        }
    }
}