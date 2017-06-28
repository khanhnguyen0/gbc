using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    private const string _path = "ChunkMaterial";
    private ChunkWorldObject[,] _localChunks;
    private List<ChunkWorldObject> _activeChunks;
    private WorldProperties _properties;

    private Pool<ChunkWorldObject> _chunkPool;

    private MapHandler _mapHandler;
    private ChunkLoader _loader;
    private Vector2I _position;

    private GameObject _playerObj;

    public static World instance { get; private set; }
   
    public List<ChunkWorldObject> activeChunks { get { return _activeChunks; } }
    public WorldProperties properties { get { return _properties; } }
    public MapHandler mapHandler { get { return _mapHandler; } }
    public Vector2I loadedPosition { get { return _position; } }

    public GameObject playerObj
    {
        get { return _playerObj; }
    }

    void Awake()
    {
        //  MapLoader.loadSector(new Vector2I(0, 0));
		//SaveGame.Instance.RefreshScene();
        instance = this;
        _properties = Resources.Load<WorldProperties>("World Properties");

        _chunkPool = new Pool<ChunkWorldObject>();

        Atlas.initialize();
        ItemStore.initialize();
        EntityStore.initialize();
        RecipeManager.loadRecipes();
        CollisionGrid.initialize(this, _properties.worldDimension * _properties.chunkDimension);
        TileStore.initialize();
        Mesher.initialize();

        int half = (_properties.worldDimension - 1) / 2;

        Vector2I spawnPos = new Vector2I(3, 3);

        _position = spawnPos - new Vector2I(half,half);

        // Create and initialize the map handler behaviour
        _mapHandler = new MapHandler(this, spawnPos);

        //_mapHandler.setPosition(new Vector2I(0, 0));
        //_mapHandler.populateSectors();

        //_mapHandler.updatePosition(new Vector2I(0, 0));

        createWorld();
		createLoader(spawnPos);
        //createLoader(spawnPos);
		if(SaveGame.Instance.IsSaveGame)
		getPlayerPos ();
        // TEMP
        CollisionGrid.rebuildMap();

        return;

        for (int i = 0; i < 4; i++)
        {
            EntityItem entity = (EntityItem)EntityStore.createEntity(0);
            entity.itemID = i;
            entity.transform.position = new Vector3(5 + i, 10, 5);
        }
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	public void getPlayerPos()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		Vector3 playerpos = new Vector3 (PlayerPrefs.GetFloat("x"),PlayerPrefs.GetFloat("y"),PlayerPrefs.GetFloat("z"));

		player.transform.position = playerpos;
	}
    public void saveMap()
    {
        _mapHandler.forceSaveAllSectors();
    }

    // Catch the quit event
    //void OnApplicationQuit()
    //{
        //saveMap();
    //}

    void savePlayer()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        Mesher.meshLoop();
        _mapHandler.update();

        // Save the game manually
        if(Input.GetKeyDown(KeyCode.P))
        {
           // _mapHandler.saveAllSectors();
        }
	}

    private ChunkWorldObject getContainer()
    {
        ChunkWorldObject obj = _chunkPool.get();
        obj.toggleVisiblity(true);
     

		return obj;
    }

    private void storeContainer(ChunkWorldObject obj)
    {
        obj.toggleVisiblity(false);
        _chunkPool.store(obj);
    }

    public void loaderMoved(Vector2I centre)
    {
        int dimension = _properties.worldDimension;

        // Work out the new bottom left corner position
        int half = (dimension - 1) / 2;
        Vector2I newCorner = centre + new Vector2I(-half, -half);

        // Tell the sector map to update its loaded sectors incase we're going out of its bounds
        _mapHandler.updatePosition(centre);

        List<Vector2I> newPoints = new List<Vector2I>();
        List<Vector2I> oldPoints = new List<Vector2I>();

        List<ChunkWorldObject> unallocated = new List<ChunkWorldObject>();
        List<ChunkWorldObject> cache = new List<ChunkWorldObject>();
        List<ChunkWorldObject> final = new List<ChunkWorldObject>();

        // Collect all the initial and new chunk positions
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                newPoints.Add(newCorner + new Vector2I(i, j));
                oldPoints.Add(_position + new Vector2I(i, j));
            }
        }

        // Set the new position
        _position = newCorner;

        // Determine which chunks need to be unloaded and which need to be kept
        // by intersecting the two sets to find the matching chunks
        for (int i = 0; i < _activeChunks.Count; i++)
        {
            ChunkWorldObject chunkObj = _activeChunks[i];
            Chunk chunk = chunkObj.chunk;

            // Ideally chunks shouldn't be loaded NULL like this but catch the case where it for some
            // reason happens
            if (chunk != null)
            {
                if (newPoints.Contains(chunk.position)) // Check for intersection
                {
                    cache.Add(chunkObj);
                    final.Add(chunkObj);
                }
                else // The chunk is going to be unloaded
                {
                    // Clear the mesh and store it
                    chunkObj.clearMesh();
                    storeContainer(chunkObj);
                }
            }
            else // The current chunk isn't loaded so it can be re-pooled
            {
                chunkObj.clearMesh();
                storeContainer(chunkObj);
            }
        }

        // Figure out which chunks need to be loaded and re-use the previous NULL chunks
        // for the new chunk data to be loaded into
        for (int i = 0; i < newPoints.Count; i++)
        {
            Vector2I pos = newPoints[i];

            // The new point isn't already loaded so it needs to be generated
            if (!oldPoints.Contains(pos))
            {
                // Fetch the chunk data
                Chunk chunk = _mapHandler.getChunk(pos);

                // Make sure the chunk is not NULL
                if (chunk != null)
                {
                    // Get a spare chunk container
                    ChunkWorldObject obj = getContainer();

                    obj.setChunk(chunk);
                    chunk.setContainer(obj);
                    chunk.setDirtyState(true);

                    // Update the collision map
               //     CollisionGrid.setChunk(chunk);

                    // Store the container
                    final.Add(obj);
                }
            }
        }

        // Overwrite the active chunks
        _activeChunks = final;

        //// Re-add the cached chunks to the final list
        //for (int i = 0; i < cache.Count; i++)
        //{
        //    final.Add(cache[i]);
        //}

        //// Re-add any unallocated chunks
        //for (int i = 0; i < unallocated.Count; i++)
        //{
        //    _activeChunks.Add(unallocated[i]);
        //}

        //// Check for lost chunks
        //if(final.Count != _activeChunks.Count)
        //{
        //    throw new System.Exception("Mismatching chunk count detected " + final.Count + " chunks were found but expected " + _activeChunks.Count);
        //}

        //_activeChunks = final;

        // Set the position after chunk data has been re-arranged
    }
    /*
    public void loaderMoved2(Vector2I centre)
    {
        int dimension = _properties.worldDimension;

        // Work out the new bottom left corner position
        int half = (dimension - 1) / 2;
        Vector2I newCorner = centre + new Vector2I(-half, -half);
        _position = newCorner;

        // Tell the sector map to update its loaded sectors incase we're going out of its bounds
        _mapHandler.updatePosition(_position);

        List<Vector2I> newPoints = new List<Vector2I>();

        // Determine where the new chunks are going to be
        for(int i = 0; i < dimension; i++)
        {
            for(int j = 0; j < dimension; j++)
            {
                newPoints.Add(newCorner + new Vector2I(i, j));
            }
        }

        List<ChunkWorldObject> preservedChunks = new List<ChunkWorldObject>();
        List<ChunkWorldObject> replacedChunks = new List<ChunkWorldObject>();

        // Find all chunks which will be preserved
        for (int i = 0; i < _activeChunks.Count; i++)
        {
            ChunkWorldObject obj = _activeChunks[i];
            Chunk chunk = obj.chunk;

            // Does the chunk actually exist?
            if (chunk != null)
            {
                Vector2I pos = chunk.position;
                // Will this chunk be in the new set of chunks?
                if (newPoints.Contains(pos))
                {
                    preservedChunks.Add(_activeChunks[i]);
                }
                else // This chunk will be unloaded 
                {
                    replacedChunks.Add(_activeChunks[i]);
                }
            }
            else // The chunk is NULL, queue it for re-use in-case
            {
                replacedChunks.Add(_activeChunks[i]);
            }
        }

        for(int i = 0; i < newPoints.Count; i++)
        {
            Vector2I pos = newPoints[i];

            for(int j = 0; j < preservedChunks.Count; j++)
            {

            }

            Chunk chunk = _mapHandler.getChunk(pos);

            if(chunk != null)
            {

            }
        }

        // determine chunks to be loaded
        .

    }*/

    /// <summary>
    /// Called when the loader has moved to a different chunk, re-adjust the chunks
    /// to fit the zone around it.
    /// </summary>
    /// <param name="centre"></param>
    public void loaderMoved2(Vector2I centre)
    {
        // add chunk behaviour here
        // adjust chunks
        // tell sectors to load if need be

        // Work out the new bottom left corner position
        int half = (_properties.worldDimension - 1) / 2;
        Vector2I newCorner = centre + new Vector2I(-half, -half);
        _position = newCorner;

        // Tell the sector map to update its loaded sectors incase we're going out of its bounds
        _mapHandler.updatePosition(_position);

        // Re-adjust the chunks
        for (int i = 0; i < _properties.worldDimension; i++)
        {
            for(int j = 0; j < _properties.worldDimension; j++)
            {
                // Load the desired chunk from the mapping handler
                Chunk chunk = _mapHandler.getChunk(newCorner + new Vector2I(i,j));
                ChunkWorldObject container = _localChunks[i, j];

                // Check whether or not the desired chunk exists yet
                if (chunk != null)
                {  
                  //  Debug.Log("Loading chunk at pos " + (newCorner + new Vector2I(i, j)) + " which is" + chunk.position);

                    container.setChunk(chunk);
                    chunk.setContainer(container);
                    // Flag the chunk as dirty as its moved
                    chunk.setDirtyState(true);
                }
                else // The chunk is NULL (not inside the map)
                {
                    container.name = "Empty Chunk";
                    container.setChunk(null);
                  //  Debug.Log("Chunk is NULL at " + (new Vector2I(i, j) + newCorner));
                }
            }
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Loading: " + _mapHandler.loadCount);
        GUILayout.Label("Corner: " + _mapHandler.cornerPosition);
    }

    private void createLoader(Vector2I position)
    {
		
        int half = (_properties.worldDimension-1) /2;

        // Create the loader object
        GameObject obj = new GameObject();
        _loader = obj.AddComponent<ChunkLoader>();
        _loader.setWorld(this);

        GameObject loadedPlayer = Resources.Load<GameObject>("Prefabs/" + PlayerPrefs.GetString("PlayerAsset", "Player"));

        if(loadedPlayer == null)
        {
            throw new System.Exception("Failed to find player prefab!");
        }

        // Create the player instance
        _playerObj = GameObject.Instantiate(loadedPlayer);
        _playerObj.name = "Player";
        obj.transform.parent = _playerObj.transform;

        // Set the position relative to the middle
        _loader.setPosition(position);
    }

    /// <summary>
    /// Generate the worlds chunks at a loaded position
    /// </summary>
    public void createWorld()
    {
        int dimension = _properties.worldDimension;

        _localChunks = new ChunkWorldObject[dimension, dimension];
        _activeChunks = new List<ChunkWorldObject>();

        Material material = Resources.Load<Material>(_path);
        material.mainTexture = Atlas.atlas;

        // Iterate and create the initial chunks
        for(int i = 0; i < dimension; i++)
        {
            for(int j = 0; j < dimension; j++)
            {
                // Create the chunk container object
                Vector2I pos = _position + new Vector2I(i, j);
                GameObject obj = new GameObject(string.Format("Chunk {0}", pos));
                ChunkWorldObject container = obj.AddComponent<ChunkWorldObject>();
                container.setMaterial(material);
                _activeChunks.Add(container);

                Chunk chunk = _mapHandler.getChunk(pos);

                if(chunk != null)
                {
                    chunk.setContainer(container);
                    container.setChunk(chunk);
                    chunk.setDirtyState(true);

                    // Update the collision map
               //     CollisionGrid.setChunk(chunk);
                }
                else
                {
                    container.name = "Empty Chunk";
                    container.setChunk(null);
                }
            }
        }

        return;

        // Create all the chunks and initialize them
        for (int i = 0; i < _properties.worldDimension; i++)
        {
            for (int j = 0; j < _properties.worldDimension; j++)
            {
                GameObject obj = new GameObject();
                ChunkWorldObject container = obj.AddComponent<ChunkWorldObject>();
                container.setMaterial(material);
                _localChunks[i, j] = container;

                // Load the desired chunk and set its data
                Chunk chunk = _mapHandler.getChunk(_position + new Vector2I(i, j));

                // Only attach the container if the chunk requested isn't NULL
                if (chunk != null)
                {
                    chunk.setContainer(container);
                    chunk.setDirtyState(true);
                    container.setChunk(chunk);
                }
                else
                {
                    container.name = "Empty Chunk";
                    container.setChunk(null);
                }
            }
        }
    }


    public void setBlock(Tile tile, Vector2 position)
    {
        position += new Vector2(0.5f, 0.5f);

        Vector2I chunkPosition = chunkPositionFromWorld(position);
        Vector2I blockPosition = blockFromChunk(chunkPosition, position);

        for(int i = 0; i < _activeChunks.Count; i++)
        {
            Chunk chunk = _activeChunks[i].chunk;
            if(chunk != null)
            {
                // Found the correct chunk?
                if (chunk.position == chunkPosition)
                {
                    // Callback to previous tile to indicate its destruction
                    Tile old = chunk.getTile(blockPosition.x, blockPosition.y);

                    old.decreaseHealth();

                    if (old.health <= 0)
                    {
                        old.onDestroyed(position);

                        chunk.setTile(tile, blockPosition);

                        //Debug.Log(string.Format("Setting block at chunk {0} block {1} from world {2}", chunkPosition, blockPosition, position));
                    }

                    break;
                }
            }
        }

      //  chunks[chunkPosition.x, chunkPosition.y].setBlock(tile, blockPosition);
    }

    public void setBackground(Tile tile, Vector2 position)
    {
        position += new Vector2(0.5f, 0.5f);

        Vector2I chunkPosition = chunkPositionFromWorld(position);
        Vector2I blockPosition = blockFromChunk(chunkPosition, position);

        for (int i = 0; i < _activeChunks.Count; i++)
        {
            Chunk chunk = _activeChunks[i].chunk;
            if (chunk != null)
            {
                // Found the correct chunk?
                if (chunk.position == chunkPosition)
                {
                    // Callback to previous tile to indicate its destruction
                    Tile old = chunk.getBackground(blockPosition.x, blockPosition.y);

                    old.decreaseHealth();

                    if (old.health <= 0)
                    {
                        old.onDestroyed(position);

                        chunk.setBackgroundTile(tile, blockPosition);
                    }

                    break;
                }
            }
        }
    }

    public bool blockEmpty(Vector2 position)
    {
        position += new Vector2(0.5f, 0.5f);

        Vector2I chunkPosition = chunkPositionFromWorld(position);
        Vector2I blockPosition = blockFromChunk(chunkPosition, position);

        for (int i = 0; i < _activeChunks.Count; i++)
        {
            Chunk chunk = _activeChunks[i].chunk;
            if (chunk != null)
            {
                // Found the correct chunk?
                if (chunk.position == chunkPosition)
                {
                    Tile tile = chunk.getTile(blockPosition.x, blockPosition.y);

                    // Check if the tile is AIR
                    if(tile.id == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        return false;
    }

    public bool backgroundEmpty(Vector2 position)
    {
        position += new Vector2(0.5f, 0.5f);

        Vector2I chunkPosition = chunkPositionFromWorld(position);
        Vector2I blockPosition = blockFromChunk(chunkPosition, position);

        for (int i = 0; i < _activeChunks.Count; i++)
        {
            Chunk chunk = _activeChunks[i].chunk;
            if (chunk != null)
            {
                // Found the correct chunk?
                if (chunk.position == chunkPosition)
                {
                    Tile tile = chunk.getBackground(blockPosition.x, blockPosition.y);

                    // Check if the tile is AIR
                    if (tile.id == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        return false;
    }

    public Tile getTile(Vector2 position)
    {
        position += new Vector2(0.5f, 0.5f);

        Vector2I chunkPosition = chunkPositionFromWorld(position);
        Vector2I blockPosition = blockFromChunk(chunkPosition, position);

        for (int i = 0; i < _activeChunks.Count; i++)
        {
            Chunk chunk = _activeChunks[i].chunk;
            if (chunk != null)
            {
                // Found the correct chunk?
                if (chunk.position == chunkPosition)
                {

                    Tile tile = chunk.getTile(blockPosition.x, blockPosition.y);
                    return tile;
                }
            }
        }

        return null;
    }

    public void rightClick(Vector2 position)
    {
        Tile tile = getTile(position);
        if (tile != null)
        {
            tile.onRightClicked();

            // Force a redraw to show any effects (TODO: Change it to only redraw if need be)
            position += new Vector2(0.5f, 0.5f);

            Vector2I chunkPosition = chunkPositionFromWorld(position);
            Vector2I blockPosition = blockFromChunk(chunkPosition, position);

            for (int i = 0; i < _activeChunks.Count; i++)
            {
                Chunk chunk = _activeChunks[i].chunk;
                if (chunk != null)
                {
                    // Found the correct chunk?
                    if (chunk.position == chunkPosition)
                    {
                        chunk.setDirtyState(true);
                    }
                }
            }
        }
    }

    public void leftClick(Vector2 position)
    {
        Tile tile = getTile(position);
        if(tile != null)
        {
            //tile.onRightClicked();
        }
    }

    /// <summary>
    /// OLD VERSION
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Tile getBlock(Vector2 position)
    {
        Vector2I chunkPosition = chunkPositionFromWorld(position);
        Vector2I blockPosition = blockPositionFromWorld(chunkPosition, position);
        return null;
       // return chunks[chunkPosition.x, chunkPosition.y].getBlock(blockPosition);
    }

    private Vector2I chunkPositionFromWorld(Vector2 pos)
    {
        return new Vector2I(Mathf.FloorToInt((pos.x / Chunk.tileDimension)), Mathf.FloorToInt((pos.y / Chunk.tileDimension)));
    }

    private Vector2I blockPositionFromWorld(Vector2I chunkPos, Vector2 pos)
    {
        return new Vector2I(Mathf.FloorToInt(pos.x - (chunkPos.x * Chunk.tileDimension)), Mathf.FloorToInt(pos.y - (chunkPos.y * Chunk.tileDimension)));
    }

    private Vector2I blockFromChunk(Vector2I chunk, Vector2 raw)
    {
        Vector2I cut = new Vector2I(chunk.x * Chunk.tileDimension, chunk.y * Chunk.tileDimension);
        return new Vector2I(Mathf.FloorToInt(raw.x) - cut.x, Mathf.FloorToInt(raw.y) - cut.y);
    }
}
