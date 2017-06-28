using UnityEngine;
using System.Collections;
using System;
using System.IO;

[System.Serializable]
public class Chunk : ISaveable
{
    public const int tileDimension = 8;
    public const int tileDepth = 2;
    private Tile[,,] _tiles;

    [SerializeField]
    private Vector2I _position;

    private World _world;
    [SerializeField]
    private bool _dirty;
    private ChunkWorldObject _container;

    public int dimension { get { return tileDimension; } }
    public Vector2I position { get { return _position; } }
    public Tile[,,] tiles { get { return _tiles; } }
    public bool dirty { get { return _dirty; } }
    public ChunkWorldObject container { get { return _container; } }

    public Chunk(World world, Vector2I position)
    {
        _world = world;
        _position = position;
    }

    public void setContainer(ChunkWorldObject container)
    {
        _container = container;
    }

    public void setData(Tile[,,] data)
    {
        _tiles = data;
        setDirtyState(true);
    }

    public void setDirtyState(bool state)
    {
        _dirty = state;
    }

    public void setTile(Tile tile, Vector2I position)
    {
        try
        {
            _tiles[position.x, position.y, 0] = tile;
            setDirtyState(true);
        }
        catch
        {
            Debug.Log("ERROR: " + position);
        }
    }

    public void setBackgroundTile(Tile tile, Vector2I position)
    {
        try
        {
            _tiles[position.x, position.y, 1] = tile;
            setDirtyState(true);
        }
        catch
        {
            Debug.Log("ERROR: " + position);
        }
    }

    public Tile getTile(int x, int y)
    {
        return _tiles[x, y, 0];
    }

    public Tile getBackground(int x, int y)
    {
        return _tiles[x, y, 1];
    }

    public void saveData(StreamWriter writer)
    {
        // TODO:
        // Chunk cache for metadata blocks

        writer.WriteLine(_position.x);
        writer.WriteLine(_position.y);

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                for (int k = 0; k < tileDepth; k++)
                {
                    _tiles[i, j, k].saveData(writer);
                }
            }
        }
    }

    public void loadData(StreamReader reader)
    {
        // TODO:
        // Chunk cache for metadata blocks

        _tiles = new Tile[tileDimension, tileDimension, 2];

        int x = int.Parse(reader.ReadLine());
        int y = int.Parse(reader.ReadLine());

        _position = new Vector2I(x, y);

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                for (int k = 0; k < tileDepth; k++)
                {
                    // Read in the ID value
                    int id = int.Parse(reader.ReadLine());

                    // Load the specified tile from the store
                    TileAsset asset = TileStore.getTile(id);

                    // Create the new instance and load any extra custom data
                    Tile tile = asset.getTile();
                    tile.loadData(reader);

                    _tiles[i, j, k] = tile;
                }
            }
        }
    }

    /*
    public void initialize(World world, Vector2I position, int dimension, Material material)
    {
        if (_initialized)
        {
            throw new System.Exception("Already initialized!");
        }
        else
        {
            _initialized = true;

            // Fetch the components
            _renderer = GetComponent<MeshRenderer>();
            _filter = GetComponent<MeshFilter>();

            // Apply the chunks material to the renderer
            _renderer.material = material;

            _world = world;
            _position = position;
            _tiles = new Tile[dimension, dimension, 2];

            // Set the name and world position based on the chunks position
            name = string.Format("Chunk ({0},{1})", _position.x, _position.y);
            transform.position = new Vector3(_position.x * dimension, _position.y * dimension, 5);

            // Initialize the new mesh for the chunk to use
            _mesh = new Mesh();
            _filter.sharedMesh = mesh;

            // Mark the mesh as dirty as it requires an initial build
            this._dirty = true;

            test();
        }
    }

    /// <summary>
    /// Alternate initialization through data reading
    /// </summary>
    /// <param name="world"></param>
    /// <param name="material"></param>
    /// <param name="reader"></param>
    public void initialize(World world, Material material, StreamReader reader)
    {
        // todo : link saving / loading to chunk initialization.
        // remove chunk neighbours (dont need really)
        // add saving for everything..

        if (_initialized)
        {
            throw new System.Exception("Already initialized!");
        }
        else
        {
            _initialized = true;

            // Fetch the components
            _renderer = GetComponent<MeshRenderer>();
            _filter = GetComponent<MeshFilter>();

            // Apply the chunks material to the renderer
            _renderer.material = material;

            // Load the chunk data from the file
            loadData(reader);

            _world = world;

            // Set the name and world position based on the chunks position
            name = string.Format("Chunk ({0},{1})", _position.x, _position.y);
            transform.position = new Vector3(_position.x * dimension, _position.y * dimension, 5);

            // Initialize the new mesh for the chunk to use
            _mesh = new Mesh();
            _filter.sharedMesh = mesh;

            // Mark the mesh as dirty as it requires an initial build
            this._dirty = true;
        }
    }

    private void test()
    {
        for (int i = 0; i < dimension; i++)
        {
            // Grass, stone, etc.
            if (_position.y == 0)
            {
                setBlock(new Tile(2, 0), new Vector2I(i, 0));
                setBlock(new Tile(1, 0), new Vector2I(i, 1));
                setBlock(new Tile(1, 0), new Vector2I(i, 2));
                setBlock(new Tile(1, 0), new Vector2I(i, 3));
                setBlock(new Tile(4, 0), new Vector2I(i, 4));

                setBackground(new Tile(6, 0), new Vector2I(i, 0));
                setBackground(new Tile(6, 0), new Vector2I(i, 1));
                setBackground(new Tile(6, 0), new Vector2I(i, 2));
                setBackground(new Tile(6, 0), new Vector2I(i, 3));
                setBackground(new Tile(6, 0), new Vector2I(i, 4));

                if (Random.Range(0, 5) > 2)
                {
                    if (Random.Range(0, 5) > 2)
                    {
                        setBlock(new Tile(3, 0), new Vector2I(i, 5));
                    }
                    else
                    {
                        setBlock(new Tile(5, 0), new Vector2I(i, 5));
                    }
                }
            }
        }
    }

    public void setBlock(Tile tile, Vector2I position)
    {
        _tiles[position.x, position.y, 0] = tile;

        TileAsset asset = TileStore.getTile(tile.id);
        CollisionGrid.setTile(new Vector2I(position.x + (_position.x * dimension), position.y + (_position.y * dimension)), asset.solid);

        _dirty = true;
    }

    public void setBackground(Tile tile, Vector2I position)
    {
        _tiles[position.x, position.y, 1] = tile;
        _dirty = true;
    }

    public Tile getBlock(Vector2I position)
    {
        return _tiles[position.x, position.y, 0];
    }

    public Tile getBackground(Vector2I position)
    {
        return _tiles[position.x, position.y, 1];
    }

    private void Update()
    {
        // Check if the mesh needs updating
        if(_dirty)
        {
            Mesher.queueMeshingTask(this);
            _dirty = false;
        }
    }*/
}
