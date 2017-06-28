using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public static class CollisionGrid
{
    private static bool _initialized;
    private static bool[,] _map;
    private static World _world;
    private static ComponentPool<BoxCollider2D> _pool;
    private static List<BoxCollider2D> _activeBoxes;

    private static GameObject _parentObj;

    public static void initialize(World world, int dimension)
    {
        if (_initialized)
        {
            //throw new System.Exception("Already initialized!");
        }
        else
        {
            _parentObj = new GameObject();
            _parentObj.name = "Pooled Colliders";

            _initialized = true;
            _map = new bool[dimension, dimension];
            _pool = new ComponentPool<BoxCollider2D>(_parentObj);
            _activeBoxes = new List<BoxCollider2D>();
            _world = world;
        }
    }

    public static void setTile(Vector2I position, bool solid)
    {
        _map[position.x, position.y] = solid;
        rebuildMap();
    }

    public static void setChunk(Chunk chunk)
    {
        Vector2I corner = _world.loadedPosition;
        Vector2I position = chunk.position;

        // Determine the Local Array based position of the chunk
        Vector2I localPos = position - corner;
        Vector2I blockPos = new Vector2I(localPos.x * Chunk.tileDimension, localPos.y * Chunk.tileDimension);

        Tile[,,] data = chunk.tiles;

        //try
        //{
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    // Set foreground tile
                    TileAsset asset = TileStore.getTile(data[i, j, 0].id);
                    _map[i + blockPos.x, j + blockPos.y] = asset.solid;
                }
            }

            rebuildMap();
        //}
        //catch
        //{
        //    Debug.Log(string.Format("OOR: {0},{1},{2},{3}", corner, position, localPos, blockPos));
        //}

    }

    public static void rebuildMap()
    {
        Vector2I corner = _world.loadedPosition;
        Vector3 worldCorner = new Vector3(corner.x * Chunk.tileDimension, corner.y * Chunk.tileDimension, 0);

        // Recall the active colliders
        for(int i = 0; i < _activeBoxes.Count; i++)
        {
            _pool.store(_activeBoxes[i]);
            _activeBoxes[i].transform.position = new Vector3(0, -100);
        }

        _activeBoxes.Clear();

        int dimension = _map.GetLength(0);

        bool[,] searched = new bool[dimension, dimension];

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++) // Find a starting point
            {
                if(!searched[i,j]) // Not already searched?
                {
                    if(_map[i,j]) // Solid tile?
                    {
                        Vector2I start = new Vector2I(i, j);

                        int width = 0;

                        for(int k = i; k < dimension; k++) // Check how wide the box will be
                        {
                            if(_map[k,j] && !searched[k,j]) // Is the tile valid for the box?
                            {
                                searched[k, j] = true;
                                width++;
                            }
                            else // Non-valid tile, stop extending width
                            {
                                break;
                            }
                        }

                        int height = 1;
                        bool flag = false;

                        for(int k = j + 1; k < dimension; k++) // Search every row above until a non-complete row is found
                        {
                            for(int l = i; l < width + i; l++) // Search the row
                            {
                                if(!_map[l,k] || searched[l,k]) // Have an invalid tile for the row
                                {
                                    flag = true;
                                    break;
                                }
                            }

                            if(flag)
                            {
                                break;
                            }

                            height++;

                            for(int m = i; m < width + i; m++) // The row is full, flag its tiles as searched
                            {
                                searched[m, k] = true;
                            }
                        }

                        // Create the specified box
                        Vector3 pos = new Vector3(width / 2.0f, height / 2.0f, 1f) + new Vector3(i, j) + new Vector3(-0.5f, -0.5f);

                        BoxCollider2D collider = _pool.get();

                        _activeBoxes.Add(collider);

                        collider.transform.localScale = new Vector3(width, height, 0);
                        collider.transform.position = pos + worldCorner;

                    }
                    else // Non-solid, flag as searched
                    {
                        searched[i, j] = true;
                    }
                }
            }
        }
    }

    public static void rebuildMap2()
    {
        int dimension = _map.GetLength(0);

        bool[,] searched = new bool[dimension, dimension];

        for (int i = 0; i < dimension; i++)
        {
            for(int j = 0; j < dimension; j++)
            {
                // Only perform bounding box searches on un-searched tiles
                if (!searched[i, j])
                {
                    // Is the section on the map solid?
                    if(_map[i,j] && !searched[i,j])
                    {
                        int width = 1;
                        int height = 1;

                        // Test for how far the row can go
                        for (int k = i; k < dimension - 1; k++)
                        {
                            searched[k, j] = true;

                            if (!_map[k,j])
                            {
                                break;
                            }
                            else
                            {
                                width++;
                            }
                        }

                        // Iterate over each succeeding row to scale the rectangle as far as it can
                        for (int k = j + 1; k < dimension; k++)
                        {
                            int successive = 0;
                            bool flag = false;

                            for (int l = i; l < width; l++) // Search over the row and determine if its entirely clear
                            {
                                searched[l, k] = true;

                                if (!_map[l, k] || searched[l, k]) // Intersecting another box or non-solid?
                                {

                                    for (int m = l; m >= i; m--) // Incomplete row, remove search flags on partial row
                                    {
                                        Debug.Log(l - m);
                                        searched[m, k] = false;
                                    }

                                    // Create the box

                                    Vector2 start = new Vector3(i, j, 20);
                                    Vector2 pos = new Vector2(width / 2.0f, height / 2.0f) + start;

                                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                    obj.transform.localScale = new Vector3(width, height, 1);
                                    obj.transform.position = pos;

                                    flag = true;

                                    break;
                                }

                                successive++;

                            }

                            if(flag)
                            {
                                break;
                            }

                            height++;
                        }

                        //// Search each row
                        //for (int k = i; k < width; k++)
                        //{
                        //    for (int l = j + 1; l < dimension; l++) // start searching along 
                        //    {
                        //        if (!_map[k, l] || searched[k,l]) // Incomplete row, backtrack a row and finalize the rectangle dimensions
                        //        {
                        //            height--;



                        //            break;
                        //        }
                        //    }

                        //    height++;

                        //    // Flag the row as searched
                        //    for (int m = k; m < width; m++)
                        //    {
                        //        searched[m, l] = true;
                        //    }

                        //}

                    }
                    else // The section isn't solid, mark as checked and continue
                    {
                        searched[i, j] = true;
                    }
                }
            }
        }
    }
}
