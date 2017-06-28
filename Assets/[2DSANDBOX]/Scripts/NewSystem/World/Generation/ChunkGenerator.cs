using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class ChunkGenerator
{
    private int _seed;

    public int seed { get { return _seed; } set { _seed = value; } }

    public ChunkGenerator()
    {

    }

    /// <summary>
    /// Generate all the chunks within a given sector and return the sector reference
    /// </summary>
    /// <param name="world"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Sector generateSector(World world, int x, int y)
    {
        Sector sector = new Sector(world);
        sector.setPosition(new Vector2I(x, y));

        int dim = Sector.chunkDimension;
        Chunk[,] chunks = new Chunk[dim, dim];

        Vector2I chunkPos = BlockUtil.SectorToChunk(new Vector2I(x, y));

        for(int i = 0; i < dim; i++)
        {
            for(int j = 0; j < dim; j++)
            {
                // Create the corresponding chunk based on its world position
                Chunk chunk = generateChunk(world, chunkPos.x + i, chunkPos.y + j);
                chunks[i, j] = chunk;
            }
        }

        sector.setChunks(chunks);
        return sector;
    }

    /// <summary>
    /// Generate the data for a corresponding chunk depending on where it should be in the world
    /// </summary>
    /// <param name="world"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Chunk generateChunk(World world, int x, int y)
    {
        Chunk chunk = new Chunk(world, new Vector2I(x, y));

        int dim = Chunk.tileDimension;
        Tile[,,] tiles = new Tile[dim, dim, 2];

        // Very simple terrain test
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                Vector2I globalPos = BlockUtil.localBlockToWorldBlock(new Vector2I(i, j), new Vector2I(x, y));

                if (globalPos.y <= 21) // Flat terrain 20 blocks deep
                {
                    if (globalPos.y == 21) // Varying grass / flower
                    {
                        if (UnityEngine.Random.Range(0, 10) < 7)
                        {
                            if (UnityEngine.Random.Range(0, 10) < 2) // Flowers are rarer than tallgrass
                            {
                                tiles[i, j, 0] = new Tile(5, 0);
                                tiles[i, j, 1] = new Tile(0, 0);
                            }
                            else // Tallgrass is more common
                            {
                                tiles[i, j, 0] = new Tile(3, 0);
                                tiles[i, j, 1] = new Tile(0, 0);
                            }
                        }
                        else // Non-grass or flower, set air
                        {
                            tiles[i, j, 0] = new Tile(0, 0);
                            tiles[i, j, 1] = new Tile(0, 0);
                        }
                    }
                    else if (globalPos.y == 20) // Grass
                    {
                        tiles[i, j, 0] = new Tile(4, 0);
                        tiles[i, j, 1] = new Tile(6, 0);
                    }
                    else if (globalPos.y < 20 && globalPos.y > 15) // dirt
                    {
                        tiles[i, j, 0] = new Tile(1, 0);
                        tiles[i, j, 1] = new Tile(6, 0);
                    }
                    else if (globalPos.y <= 15 && globalPos.y > 11 && UnityEngine.Random.Range(0, 10) < 7) // random invariation of dirt
                    {
                        tiles[i, j, 0] = new Tile(1, 0);
                        tiles[i, j, 1] = new Tile(6, 0);
                    }
                    else // stone
                    {
                        tiles[i, j, 0] = new Tile(2, 0);
                        tiles[i, j, 1] = new Tile(10, 0);
                    }
                }
                else // Air
                {
                    tiles[i, j, 0] = new Tile(0, 0);
                    tiles[i, j, 1] = new Tile(0, 0);
                }

            }
        }

        chunk.setData(tiles);

        return chunk;

        /*  for(int i = 0; i < dim; i++)
          {
              Vector2I globalPos = BlockUtil.localBlockToWorldBlock(new Vector2I(i, 0), new Vector2I(x, 0));
              float noise = Mathf.PerlinNoise((float)globalPos.x + seed, (float)globalPos.y);


          }*/
    }

}