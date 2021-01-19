using System;
using System.Collections.Generic;
using RecipeLibrary.Interfaces;
using RecipeLibrary.Models;
using RecipeLibrary.Services;

namespace RecipeLibrary.Menu
{
    public class RecipeMenu : IMenu
    {
        private readonly RecipeService _recipeService;
        
        private FindSettings _findSettings;

        public RecipeMenu()
        {
            _recipeService = new RecipeService();

            _findSettings = new FindSettings
            {
                IsAsc = true, 
                SortedBy = "name"
            };
        }
        
        public void Start()
        {
            for (;;)
            {
                Console.WriteLine("Menu");
                Console.WriteLine("1. Create recipe");
                Console.WriteLine("2. Delete recipe");
                Console.WriteLine("3. Change recipe");
                Console.WriteLine("4. Find recipe");
                Console.WriteLine("5. Exit");

                Console.WriteLine("Choose your action:");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreateMenu();
                        break;
                    case "2":
                        DeleteMenu();
                        break;
                    case "3":
                        ChangeMenu();
                        break;
                    case "4":
                        FindMenu();
                        break;
                    case "5":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        PrintDefault();
                        break;
                }
            }
        }

        private void FindMenu()
        {
            Console.WriteLine("Find recipe");
            while (IsGoBack("Find recipe"))
            {
                Console.WriteLine("1. Find by name");
                Console.WriteLine("2. Find by ingredient name");
                Console.WriteLine("3. Back");

                Console.WriteLine("Choose your action:");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        FindRecipesByName();
                        break;
                    case "2":
                        FindRecipesByIngredient();
                        break;
                    case "3":
                        return;
                    default:
                        PrintDefault();
                        break;
                }
            }
        }
        
        private void FindRecipesByName()
        {
            Console.WriteLine("Enter name:");
            var searchStr = Console.ReadLine();

            try
            {
                var recipes = _recipeService.GetObjects(searchStr.TrimEnd().TrimStart());
                if (recipes == null || recipes.Count == 0)
                {
                    Console.WriteLine("Not found!");
                    return;
                }

                PrintFoundRecipes(recipes);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Wrong entered value!");
            }
        }

        private void FindRecipesByIngredient()
        {
            try
            {
                Console.WriteLine("Enter ingredient name:");
                var searchStr = Console.ReadLine();
                var recipes = _recipeService.GetByIngredient(searchStr.TrimEnd().TrimStart());
                if (recipes == null || recipes.Count == 0)
                {
                    Console.WriteLine("Not found!");
                    return;
                }

                PrintFoundRecipes(recipes);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Wrong entered value!");
            }
        }

        private void PrintFoundRecipes(List<Recipe> recipes)
        {
            try
            {
                Console.WriteLine($"Found {recipes.Count} elements!");
                SetFindSettings();
                recipes = _recipeService.SortListBySettings(recipes, _findSettings);
                
                foreach (var recipe in recipes)
                {
                    Console.WriteLine(recipe.ToString());
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("No recipes!");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("No recipes!");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Wrong entered value!");
            }
        }

        private string SortedByWhat()
        {
            for (;;)
            {
                Console.WriteLine("Sort values by:");
                Console.WriteLine("1. Name");
                Console.WriteLine("2. Nutritional value");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        return "name";
                    case "2":
                        return "nutritional value";
                    default:
                        PrintDefault();
                        break;
                }
            }
        }
        
        /// <summary>
        /// The user chooses how to sort the recipes.
        /// </summary>
        /// <returns>True - ascending, false - descending</returns>
        private bool AscendingOrDescending()
        {
            for (;;)
            {
                Console.WriteLine("Sorter by");
                Console.WriteLine("1. Descending");
                Console.WriteLine("2. Ascending");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        return false;
                    case "2":
                        return true;
                    default:
                        PrintDefault();
                        break;
                }
            }
        }

        private void SetFindSettings()
        {
            string input = "";
            while(input != "2")
            {
                Console.WriteLine($"Current sorting: sorted by {_findSettings.SortedBy}, ascending: {_findSettings.IsAsc} ");
                Console.WriteLine("Change sorting?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("2. No");
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        SetSortSettings();
                        break;
                    case "2":
                        break;
                    default:
                        PrintDefault();
                        break;
                }
            }

            _findSettings.NutritionalValue = null;
            for (;;)
            {
                Console.WriteLine("Set filter by nutritional value?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("2. No");
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        SetFilterSettings();
                        return;
                    case "2":
                        return;
                    default:
                        PrintDefault();
                        break;
                }
            }
        }

        private void SetFilterSettings()
        {
            var proteins = GetDoubleNumberFromUser("Enter filter for proteins:");
            var carbohydrates = GetDoubleNumberFromUser("Enter filter for carbohydrates:");
            var fats = GetDoubleNumberFromUser("Enter filter for fats:");
            
            _findSettings.NutritionalValue = new NutritionalValue(proteins, fats, carbohydrates);
            
            for (;;)
            {
                Console.WriteLine("Are more or less than your filter value?");
                Console.WriteLine("1. More");
                Console.WriteLine("2. Less");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        _findSettings.IsMore = true;
                        return;
                    case "2":
                        _findSettings.IsMore = false;
                        return;
                    default:
                        PrintDefault();
                        break;
                }
            }
        }

        private void SetSortSettings()
        {
            _findSettings.SortedBy = SortedByWhat();
            _findSettings.IsAsc = AscendingOrDescending();
        }

        private void CreateMenu()
        {
            Console.WriteLine("Creating menu");
            
            while(IsGoBack("Create recipe"))
            {
                var recipeName = GetStringFromUser("Enter a recipe name:");
                if (recipeName == null)
                    return;

                List<Ingredient> ingredients = AddIngredients();

                var nutritionalValue = AddNutritionalValue();
                try
                {
                    _recipeService.Add(recipeName, ingredients, nutritionalValue);
                    Console.WriteLine("Recipe was created!");
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Invalid values!");
                }
            }
        }

        private bool IsGoBack(string message, string backMessage = "Back")
        {
            for (;;)
            {
                Console.WriteLine($"1. {message}");
                Console.WriteLine($"2. {backMessage}");
                Console.WriteLine("Choose your action:");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        return true;
                    case "2":
                        return false;
                    default:
                        PrintDefault();
                        break;
                }
            }
        }

        private NutritionalValue AddNutritionalValue()
        {
            Console.WriteLine("You need to enter nutritional value.");
            
            var proteins = GetDoubleNumberFromUser("Enter proteins:");
            var fats = GetDoubleNumberFromUser("Enter fats:");
            var carbohydrates = GetDoubleNumberFromUser("Enter carbohydrates:");
            
            return new NutritionalValue(proteins, fats, carbohydrates);
        }

        private List<Ingredient> AddIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            
            while (IsGoBack("Add a ingredient", "No"))
            {
                Console.WriteLine("Adding ingredient");
            
                var name = GetStringFromUser("Enter name of ingredient:");
                var count = GetIntNumberFromUser("Enter count of ingredient:");
                var measure = GetStringFromUser("Enter units of measurement:");

                ingredients.Add(new Ingredient(count, measure, name));
            }

            return ingredients;
        }

        private void ChangeMenu()
        {
            Console.WriteLine("Changing menu");
            
            while (IsGoBack("Change recipe"))
            {
                Recipe recipe;
                
                var name = GetStringFromUser("Enter name of recipe:");
                try
                {
                    recipe = _recipeService.GetObject(name);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                if (recipe == null)
                {
                    Console.WriteLine($"Not found recipe with this name: {name}!");
                    continue;
                }

                Console.WriteLine(recipe.ToString());

                for (;;)
                {
                    Console.WriteLine("What do you change?");
                    Console.WriteLine("1. Recipe name");
                    Console.WriteLine("2. All recipe");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            ChangeRecipeName(recipe);
                            return;
                        case "2":
                            ChangeAllRecipe(recipe.Name);
                            return;
                        default:
                            PrintDefault();
                            break;
                    }
                }
            }
        }

        private void ChangeRecipeName(Recipe recipe)
        {
            recipe.Name = GetStringFromUser("Enter new recipe name:");
            try
            {
                _recipeService.Update(recipe.Name, recipe);
                Console.WriteLine("Name was changed!");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ChangeAllRecipe(string name)
        {
            var recipeName = GetStringFromUser("Enter new recipe name:");
            var ingredients = AddIngredients();
            var nutritionalValue = AddNutritionalValue();

            try
            {
                _recipeService.Update(name, new Recipe(recipeName, ingredients, nutritionalValue));
                Console.WriteLine("Recipe was changed!");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void DeleteMenu()
        {
            Console.WriteLine("Deleting menu");

            while (IsGoBack("Delete recipe"))
            {
                DeleteRecipe();
            }
        }

        private void DeleteRecipe()
        {
            Console.WriteLine("Enter recipe name:");
            var name = Console.ReadLine();
                
            try
            {
                var recipe = _recipeService.GetObject(name);
                if (recipe != null)
                {
                    for (;;)
                    {
                        Console.WriteLine("Are your sure?");
                        Console.WriteLine("1. Yes");
                        Console.WriteLine("2. No");
                        var input = Console.ReadLine();
                        switch (input)
                        {
                            case "1":
                                _recipeService.Delete(name);
                                Console.WriteLine("Recipe was deleted!");
                                return;
                            case "2":
                                return;
                            default:
                                PrintDefault();
                                break;
                        }
                    }
                }

                Console.WriteLine("Recipe not found!");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid string for search!");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private double GetDoubleNumberFromUser(string message)
        {
            for (;;)
            {
                Console.WriteLine(message);
                var input = Console.ReadLine();

                if (double.TryParse(input, out var result) && result >= 0)
                    return result;
                
                Console.WriteLine("Value should be a number!");
            }
        }

        private int GetIntNumberFromUser(string message)
        {
            for (;;)
            {
                Console.WriteLine(message);
                var input = Console.ReadLine();

                if (int.TryParse(input, out var result) && result > 0)
                    return result;
                
                Console.WriteLine("Value should be a number!");
            }
        }
        
        private string GetStringFromUser(string message)
        {
            for (;;)
            {
                Console.WriteLine(message);
                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input) && input.TrimEnd().TrimStart() != "")
                    return input;
                
                Console.WriteLine("Value should not be empty!");
            }
        }

        private void PrintDefault()
        {
            Console.WriteLine("Enter correct action!");
        }
    }
}