using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public static class RecipeManager
{
    private static List<RecipeAsset> recipies = new List<RecipeAsset>();

    public static List<RecipeAsset> Recipies { get { return recipies; } }

    public static void registerRecipe(RecipeAsset recipe)
    {
        recipies.Add(recipe);
    }

    public static void loadRecipes()
    {
		
		RecipeAsset[] assets = Resources.LoadAll<RecipeAsset>("Recipes/");			
        Debug.Log("Registering " + assets.Length + " Recipes");

        for (int i = 0; i < assets.Length; i++)
        {
            RecipeAsset asset = assets[i];
            registerRecipe(asset);
        }
    }
}
