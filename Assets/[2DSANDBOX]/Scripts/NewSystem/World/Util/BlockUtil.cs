using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public static class BlockUtil
{
    public static Vector2I chunkPositionFromWorld(Vector2 pos)
    {
        return new Vector2I(Mathf.FloorToInt(pos.x) / Chunk.tileDimension, Mathf.FloorToInt(pos.y) / Chunk.tileDimension);
    }

    public static Vector2I blockPositionFromWorld(Vector2I chunkPos, Vector2 pos)
    {
        return new Vector2I(Mathf.FloorToInt(pos.x) - (chunkPos.x * Chunk.tileDimension), Mathf.FloorToInt(pos.y) - (chunkPos.y * Chunk.tileDimension));
    }

    public static Vector2I localBlockToWorldBlock(Vector2I localBlock, Vector2I chunkPos)
    {
        return new Vector2I(Chunk.tileDimension * chunkPos.x + localBlock.x, Chunk.tileDimension * chunkPos.y + localBlock.y);
    }

    public static Vector2I SectorToChunk(Vector2I sectorPosition)
    {
        return new Vector2I(Sector.chunkDimension * sectorPosition.x, Sector.chunkDimension * sectorPosition.y);
    }

    /// <summary>
    /// Convert a chunk position to a sector position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Vector2I SectorFromChunk(Vector2I pos)
    {
        return new Vector2I(Mathf.FloorToInt((float)pos.x / Sector.chunkDimension), Mathf.FloorToInt((float)pos.y / Sector.chunkDimension));
    }
}