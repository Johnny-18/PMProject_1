using System;
using System.Collections.Generic;
using System.Dynamic;
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

        public void Delete(Recipe obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
            
            _recipes.Remove(obj);
            SaveWork();
        }
        
        public void Delete(string name)
        {
            var recipe = GetObject(name);
            if (recipe == null)
                throw new ArgumentException("Not found!");
            
            Delete(recipe);
        }
        
        public void Add(Recipe obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
            
            _recipes.Add(obj);
            SaveWork();
        }

        public void Add(string name, List<Ingredient> ingredients, NutritionalValue nutritionalValue)
        {
            Add(new Recipe(GetUniqueId(), name, ingredients, nutritionalValue));
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
            if (_recipes.Count == 0)
                throw new NullReferenceException("Recipes list is empty!");
            
            return _recipes.FirstOrDefault(x => x.Name == name);
        }

        public List<Recipe> GetByIngredient(string searchStr)
        {
            if (searchStr == null)
                throw new ArgumentNullException();

            if (_recipes.Count == 0)
                throw new NullReferenceException("Recipes list is empty!");

            if (searchStr == "")
                return _recipes;
            
            return _recipes.Where(x => x.Ingredients.Exists(y => y.Name == searchStr)).ToList();
        }

        public List<Recipe> SortListBySettings(List<Recipe> recipes, FindSettings settings)
        {
            if (recipes == null || settings == null)
                throw new ArgumentNullException();
            if (recipes.Count == 0)
                throw new ArgumentException("Not found");
            if (recipes.Count == 1)
                return recipes;

            if (settings.SortedBy == "name")
            {
                return SortedByName(recipes, settings);
            }

            if (settings.SortedBy == "nutritional value")
            {
                return SorterByNutritionalValue(recipes, settings);
            }

            return recipes;
        }
        
        public void SaveWork()
        {
            _fw.Serialize(_recipes);
        }

        private IEnumerable<Recipe> UseFilter(List<Recipe> recipes, FindSettings settings)
        {
            if (settings.NutritionalValue != null)
            {
                if (settings.IsMore)
                {
                    return recipes.Where(x => x.NutritionalValue.CompareTo(settings.NutritionalValue) == 1);
                }

                return recipes.Where(x => x.NutritionalValue.CompareTo(settings.NutritionalValue) == -1);
            }

            return recipes;
        }

        private List<Recipe> SorterByNutritionalValue(List<Recipe> recipes, FindSettings settings)
        {
            if(settings.IsAsc)
                return UseFilter(recipes, settings).OrderBy(x => x.NutritionalValue.GetCalories()).ToList();
            
            return UseFilter(recipes, settings).OrderByDescending(x => x.NutritionalValue.GetCalories()).ToList();
        }

        private List<Recipe> SortedByName(List<Recipe> recipes, FindSettings settings)
        {
            if(settings.IsAsc)
                return UseFilter(recipes, settings).OrderBy(x => x.Name).ToList();
            
            return UseFilter(recipes, settings).OrderByDescending(x => x.Name).ToList();
        }

        private int GetUniqueId()
        {
            if (_recipes == null || _recipes.Count == 0)
                return 1;

            return _recipes.Last().Id + 1;
        }
    }
}