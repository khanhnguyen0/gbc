using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Builder
{
    private int _index;
    private List<Vector3> _vertices;
    private List<int> _triangles;
    private List<Vector2> _uvs;

    private int _indexB;
    private List<int> _trianglesB;

    private int _dimension;
    private Chunk _chunk;

    private Dictionary<int, TileAsset> _tileStoreCache;

    public Builder()
    {
        _tileStoreCache = TileStore.tiles;

        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _uvs = new List<Vector2>();

        _trianglesB = new List<int>();
    }

    public void buildMesh(Chunk chunk)
    {
        reset();

        _chunk = chunk;
        _dimension = chunk.dimension;

        Tile[,,] tiles = _chunk.tiles;

        for(int i = 0; i < _dimension; i++)
        {
            for(int j = 0; j < _dimension; j++)
            {
                Vector2I pos = new Vector2I(i, j);
                TileAsset tile = _tileStoreCache[tiles[i, j, 0].id];
                TileAsset background = _tileStoreCache[tiles[i, j, 1].id];

                // is the tile visible?
                if(tile.visible)
                {
                    setTile(tiles[i, j, 0], pos);

                    if (!tile.opaque && background.visible) // can you see through it?
                    {
                        setBackground(background, pos);
                    }
                }
                else if(background.visible) // is the background tile visible?
                {
                    setBackground(background, pos);
                }
            }
        }

        Mesh mesh = chunk.container.mesh;
        mesh.Clear();

        mesh.vertices = _vertices.ToArray();

        mesh.SetIndices(_triangles.ToArray(), MeshTopology.Triangles, 0);

        mesh.uv = _uvs.ToArray();

        mesh.RecalculateNormals();
        ;
    }

    private void setTile(Tile tile, Vector2I pos)
    {
        _vertices.Add(new Vector3(-0.5f + pos.x, -0.5f + pos.y, 0));
        _vertices.Add(new Vector3(0.5f + pos.x, -0.5f + pos.y, 0));
        _vertices.Add(new Vector3(0.5f + pos.x, 0.5f + pos.y, 0));
        _vertices.Add(new Vector3(-0.5f + pos.x, 0.5f + pos.y, 0));

        Rect rect = Atlas.getTexture(tile.getTexture());

        _uvs.Add(new Vector2(rect.xMin, rect.yMin));
        _uvs.Add(new Vector2(rect.xMax, rect.yMin));
        _uvs.Add(new Vector2(rect.xMax, rect.yMax));
        _uvs.Add(new Vector2(rect.xMin, rect.yMax));

        _triangles.Add(_index + 0);
        _triangles.Add(_index + 3);
        _triangles.Add(_index + 2);
        _triangles.Add(_index + 2);
        _triangles.Add(_index + 1);
        _triangles.Add(_index + 0);

        _index += 4;
    }

    private void setBackground(TileAsset tile, Vector2I pos)
    {
        _vertices.Add(new Vector3(-0.5f + pos.x, -0.5f + pos.y, 1));
        _vertices.Add(new Vector3(0.5f + pos.x, -0.5f + pos.y, 1));
        _vertices.Add(new Vector3(0.5f + pos.x, 0.5f + pos.y, 1));
        _vertices.Add(new Vector3(-0.5f + pos.x, 0.5f + pos.y, 1));

        Rect rect = Atlas.getTexture(tile.texture.name);

        _uvs.Add(new Vector2(rect.xMin, rect.yMin));
        _uvs.Add(new Vector2(rect.xMax, rect.yMin));
        _uvs.Add(new Vector2(rect.xMax, rect.yMax));
        _uvs.Add(new Vector2(rect.xMin, rect.yMax));

        _triangles.Add(_index + 0);
        _triangles.Add(_index + 3);
        _triangles.Add(_index + 2);
        _triangles.Add(_index + 2);
        _triangles.Add(_index + 1);
        _triangles.Add(_index + 0);

        _index += 4;
    }

    private void reset()
    {
        _index = 0;
        _vertices.Clear();
        _triangles.Clear();
        _uvs.Clear();

        _indexB = 0;
        _trianglesB.Clear();
    }
}