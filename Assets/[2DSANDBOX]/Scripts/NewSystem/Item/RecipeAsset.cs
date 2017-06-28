using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
//using UnityEditor;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class RecipeAsset : ScriptableObject
{
    [System.Serializable]
    public struct Require
    {
        public ItemAsset itemAsset;
        public int quantity;
    }

    [SerializeField]
    private Require[] requirements;
    [SerializeField]
    private ItemAssetTile result;

    public Require[] Requirements { get { return requirements; } }

    public ItemAssetTile Result { get { return result; } }

    public RecipeAsset(Require[] requirements)
    {
        this.requirements = requirements;
    }
}