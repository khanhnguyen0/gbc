using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public static class Atlas
{
    private const int _padding = 2;
    private const string _path = "Textures/";

    private static Texture2D _atlas;
    private static Dictionary<string, Rect> textureAtlas;
    private static bool initialized;

    /// <summary>
    /// The atlas texture reference for chunks
    /// </summary>
    public static Texture2D atlas { get { return _atlas; } }

    public static void initialize()
    {
        if (initialized)
        {
            //throw new Exception("Already initialized!");
        }
        else
        {
            initialized = true;
            textureAtlas = new Dictionary<string, Rect>();
            loadTiles();
        }
    }

    private static void loadTiles()
    {
        Texture2D[] textures = Resources.LoadAll<Texture2D>(_path);

        _atlas = new Texture2D(1, 1);
        Rect[] rects = _atlas.PackTextures(textures, _padding, 2048);

        // Fix edge bleeding with atlas for mipmapping
        uPaddingBleed.BleedEdges(_atlas, _padding, rects, false);

        // Set to point filtering
        _atlas.filterMode = FilterMode.Point;

        // Index rects into atlas
        for(int i = 0; i < rects.Length; i++)
        {
            Debug.Log("Adding texture " + textures[i].name);
            textureAtlas.Add(textures[i].name, rects[i]);
        }
    }

    public static Rect getTexture(string identifier)
    {
        return textureAtlas[identifier];
    }
}