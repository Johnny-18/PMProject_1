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

            if (_recipes == null)
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
            Add(new Recipe(GetUniqueId(), name.TrimStart().TrimEnd(), ingredients, nutritionalValue));
        }

        public void Update(string name, Recipe newObj)
        {
            var recipe = GetObject(name);
            if (recipe == null)
                throw new ArgumentException("Not found!");

            recipe.Name = newObj.Name.TrimStart().TrimEnd();
            recipe.Ingredients = newObj.Ingredients;
            recipe.NutritionalValue = newObj.NutritionalValue;
            
            SaveWork();
        }

        public List<Recipe> GetObjects(string name)
        {
            return GetObjects(name, null);
        }

        public List<Recipe> GetObjects(string name, FindSettings settings)
        {
            if (name == null)
                throw new ArgumentNullException();
            
            if (name == "")
                return UseFilter(_recipes, settings).ToList();

            return UseFilter(_recipes.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList(), settings).ToList();
        }

        public Recipe GetObject(string name)
        {
            if (_recipes.Count == 0)
                throw new NullReferenceException("Recipes list is empty!");
            
            return _recipes.FirstOrDefault(x => x.Name == name);
        }

        public List<Recipe> GetByIngredient(string searchStr, FindSettings settings)
        {
            if (searchStr == null)
                throw new ArgumentNullException();

            if (_recipes.Count == 0)
                throw new NullReferenceException("Recipes list is empty!");
            
            if (searchStr == "")
                return UseFilter(_recipes.Where(x => x.Ingredients != null && x.Ingredients.Count != 0).ToList(),
                    settings).ToList();

            return UseFilter(_recipes?.Where(x => x.Ingredients != null && x.Ingredients.Exists(y => y.Name
                .ToLower().Contains(searchStr.ToLower()))).ToList(), settings).ToList();
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
            if (settings?.FilterValue != null)
            {
                if (settings.IsMore)
                {
                    return recipes.Where(x => x.NutritionalValue.CompareTo(settings.FilterValue) == 1);
                }

                return recipes.Where(x => x.NutritionalValue.CompareTo(settings.FilterValue) == -1);
            }

            return recipes;
        }

        private List<Recipe> SorterByNutritionalValue(List<Recipe> recipes, FindSettings settings)
        {
            if(settings.IsAsc)
                return recipes.OrderBy(x => x.NutritionalValue.GetCalories()).ToList();
            
            return recipes.OrderByDescending(x => x.NutritionalValue.GetCalories()).ToList();
        }

        private List<Recipe> SortedByName(List<Recipe> recipes, FindSettings settings)
        {
            if(settings.IsAsc)
                return recipes.OrderBy(x => x.Name).ToList();
            
            return recipes.OrderByDescending(x => x.Name).ToList();
        }

        private int GetUniqueId()
        {
            if (_recipes == null || _recipes.Count == 0)
                return 1;

            return _recipes.Last().Id + 1;
        }
    }
}