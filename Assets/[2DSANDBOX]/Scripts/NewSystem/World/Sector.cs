using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Assertions;

public class Sector : ISaveable
{
    public const int chunkDimension = 8;
    private Chunk[,] _chunks;
    private Vector2I _position;
    private World _world;

    // Is this sector used as a dummy?
    private bool _nullSector;

    /// <summary>
    /// The position of the sector in 'Sector Space' which is 
    /// based around multiples of 8 chunks at a time. 
    /// </summary>
    public Vector2I position { get { return _position; } }

    /// <summary>
    /// Is the sector classed as a NULL sector?
    /// </summary>
    public bool isNullSector { get { return _nullSector; } set { _nullSector = value; } }

    public Sector(World world)
    {
        _world = world;
        _chunks = new Chunk[chunkDimension, chunkDimension];
    }

    public void setChunks(Chunk[,] chunks)
    {
        _chunks = chunks;
    }

    /// <summary>
    /// Get a chunk relative to its 'chunk grid' space
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Chunk getChunk(int x, int y)
    {
        for(int i = 0; i < chunkDimension; i++)
        {
            for(int j = 0; j < chunkDimension; j++)
            {
                Chunk chunk = _chunks[i, j];

                // Only deal with non NULL chunks
                if(chunk != null)
                {
                    // Found the chunk desired
                    if (chunk.position == new Vector2I(x, y))
                    {
                        return _chunks[i, j];
                    }
                }
            }
        }

        return null;
    }

    public void setPosition(Vector2I position)
    {
        _position = position;
    }

    public void loadData(StreamReader reader)
    {
        for (int i = 0; i < chunkDimension; i++)
        {
            for (int j = 0; j < chunkDimension; j++)
            {
                Vector2I pos = new Vector2I((_position.x * chunkDimension) + i, (_position.y * chunkDimension) + j);
                Chunk chunk = new Chunk(_world, pos);
                chunk.loadData(reader);

                _chunks[i, j] = chunk;
            }
        }
    }

    public void saveData(StreamWriter writer)
    {
        if (!_nullSector) // Only perform saving of a NON-NULL sector
        {

            if (_chunks == null)
            {
                throw new System.Exception("NULL CHUNKS");
            }
            else
            {
                for (int i = 0; i < chunkDimension; i++)
                {
                    for (int j = 0; j < chunkDimension; j++)
                    {
                        Chunk chunk = _chunks[i, j];

                        // Only save non-NULL chunks
                        if (chunk == null)
                        {
                            throw new System.Exception("Chunk at " + i + "," + j + " is NULL");
                        }
                        else
                        {
                            chunk.saveData(writer);
                        }
                    }
                }
            }
        }
    }
}