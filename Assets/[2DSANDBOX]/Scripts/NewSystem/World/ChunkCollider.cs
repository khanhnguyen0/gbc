using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class ChunkCollider : MonoBehaviour
{
    private bool _initialized;
    private bool[,] _map;
    private ComponentPool<BoxCollider2D> _pool;
    private List<BoxCollider2D> _activeBoxes;

    public void initialize(int dimension)
    {
        if (_initialized)
        {
            throw new System.Exception("Already initialized!");
        }
        else
        {
            _initialized = true;
            _map = new bool[dimension, dimension];
            _pool = new ComponentPool<BoxCollider2D>(gameObject);
            _activeBoxes = new List<BoxCollider2D>();
        }
    }

    public void setData(Tile[,,] data)
    {
        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                // Set foreground tile
                //TileAsset asset = TileStore.getTile(data[i, j, 0].id);
                _map[i, j] = data[i,j,0].isBlockSolid();
            }
        }

        rebuildMap();
    }

    public void rebuildMap()
    {
        // Recall the active colliders
        for (int i = 0; i < _activeBoxes.Count; i++)
        {
            _pool.store(_activeBoxes[i]);
            _activeBoxes[i].transform.position = transform.position;
        }

        _activeBoxes.Clear();

        int dimension = _map.GetLength(0);

        bool[,] searched = new bool[dimension, dimension];

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++) // Find a starting point
            {
                if (!searched[i, j]) // Not already searched?
                {
                    if (_map[i, j]) // Solid tile?
                    {
                        Vector2I start = new Vector2I(i, j);

                        int width = 0;

                        for (int k = i; k < dimension; k++) // Check how wide the box will be
                        {
                            if (_map[k, j] && !searched[k, j]) // Is the tile valid for the box?
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

                        for (int k = j + 1; k < dimension; k++) // Search every row above until a non-complete row is found
                        {
                            for (int l = i; l < width + i; l++) // Search the row
                            {
                                if (!_map[l, k] || searched[l, k]) // Have an invalid tile for the row
                                {
                                    flag = true;
                                    break;
                                }
                            }

                            if (flag)
                            {
                                break;
                            }

                            height++;

                            for (int m = i; m < width + i; m++) // The row is full, flag its tiles as searched
                            {
                                searched[m, k] = true;
                            }
                        }

                        // Create the specified box
                        Vector3 pos = new Vector3(width / 2.0f, height / 2.0f, 1f) + new Vector3(i, j) + new Vector3(-0.5f, -0.5f);

                        BoxCollider2D collider = _pool.get();

                        _activeBoxes.Add(collider);

                        collider.transform.localScale = new Vector3(width, height, 0);
                        collider.transform.localPosition = pos;// + worldCorner;

                    }
                    else // Non-solid, flag as searched
                    {
                        searched[i, j] = true;
                    }
                }
            }
        }
    }

}
