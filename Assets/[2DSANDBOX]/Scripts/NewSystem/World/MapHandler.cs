using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MapHandler
{
    /// <summary>
    /// The dimension of the active sectors to be loaded at one time
    /// </summary>
    private const int _loadedDimension = 5;
    private const int _worldLength = 16;
    private const int _worldHeight = 8;
    private const int _loadedPerFrame = 1;
    private const int _savedPerFrame = 1;

    private List<Sector> _activeSectors;
    private World _world;
    private Vector2I _position;

    private ChunkGenerator _generator;

    private Queue<Sector> _saving;
    private Queue<Vector2I> _loading;

    public int loadCount { get { return _loading.Count; } }
    public Vector2I cornerPosition { get { return _position; } }
    public List<Sector> activeSectors { get { return _activeSectors; } }

    public MapHandler(World world, Vector2I startingPosition)
    {
        _activeSectors = new List<Sector>();
        _generator = new ChunkGenerator();
        _saving = new Queue<Sector>();
        _loading = new Queue<Vector2I>();
        _world = world;

        // Work out the bottom left corner from the starting chunk
        int half = (_loadedDimension -1) /2;
        Vector2I cornerSector = BlockUtil.SectorFromChunk(startingPosition) + new Vector2I(-half, -half);

        Debug.Log("Setting corner to " + cornerSector);

        // Set the starting position
        _position = cornerSector;

        // Create the initial sectors
        for (int i = 0; i < _loadedDimension; i++)
        {
            for (int j = 0; j < _loadedDimension; j++)
            {
                Vector2I pos = _position + new Vector2I(i, j);
                Sector sector = getSector(pos);
                _activeSectors.Add(sector);
            }
        }

        updatePosition(startingPosition);
    }

    public void update()
    {
        for (int i = 0; i < _loadedPerFrame; i++)
        {
            if (_loading.Count > 0)
            {
                Vector2I pos = _loading.Dequeue();
                Sector sector = getSector(pos);

                _activeSectors.Add(sector);
            }
        }

        for (int i = 0; i < _savedPerFrame; i++)
        {
            if (_saving.Count > 0)
            {
                Sector sector = _saving.Dequeue();

                if (sector.isNullSector)
                {
                    Debug.Log("Tried to save a NULL sector");
                }
                else
                {
                    //Debug.Log("Saving " + sector.position);
                    MapLoader.saveSector(sector);
                }
            }

        }
    }

    /// <summary>
    /// Run a manual save of all sectors
    /// </summary>
    public void saveAllSectors()
    {
        for (int i = 0; i < _activeSectors.Count; i++)
        {
            queueSector(_activeSectors[i]);
        }
    }

    /// <summary>
    /// Force the map to save all its loaded sectors immediately
    /// </summary>
    public void forceSaveAllSectors()
    {
        Debug.Log("Forcing Sector Save");

        saveAllSectors();
        forceQueue();
    }

    /// <summary>
    /// Force the queue to save everything
    /// </summary>
    private void forceQueue()
    {
        for (int i = 0; i < _saving.Count; i++)
        {
            Sector s = _saving.Dequeue();
            MapLoader.saveSector(s);
        }
    }

    private void queueSector(Sector sector)
    {
        if(!_saving.Contains(sector))
        {
            _saving.Enqueue(sector);
        }
    }

    public Chunk getChunk(Vector2I chunkPos)
    {
        Vector2I sector = BlockUtil.SectorFromChunk(chunkPos);

        int sectorX = Mathf.FloorToInt((float)chunkPos.x / Sector.chunkDimension);
        int sectorY = Mathf.FloorToInt((float)chunkPos.y / Sector.chunkDimension);

        int chunkX = chunkPos.x - (sectorX * Sector.chunkDimension);
        int chunkY = chunkPos.y - (sectorY * Sector.chunkDimension);

        for(int i = 0; i < _activeSectors.Count; i++)
        {
            // Has the sector been found?
            if(_activeSectors[i].position == new Vector2I(sectorX, sectorY))
            {
                return _activeSectors[i].getChunk(chunkPos.x, chunkPos.y);
            }
        }

        return null;
    }

    /// <summary>
    /// Determine whether or not to load a sector or generate a new one
    /// </summary>
    /// <param name="pos"></param>
    private Sector getSector(Vector2I pos)
    {
        Sector sector;

        // Check whether the current sector has already been generated
        if (MapLoader.sectorExists(pos))
        {
            sector = MapLoader.loadSector(_world, pos);
        }
        else // the sector has not yet been generated, queue it for generation
        {
            sector = _generator.generateSector(_world, pos.x, pos.y);

            // Queue the sector to save as its newly generated
            queueSector(sector);
        }

        Debug.Log(sector);

        return sector;
    }

    /// <summary>
    /// Set the position of the maphandlers bottom left corner sector based on 'sector space'.
    /// </summary>
    /// <param name="position"></param>
    public void setPosition(Vector2I position)
    {
        _position = position;
    }

    public void updatePosition(Vector2I chunkPosition)
    {
        // Convert chunk coordinates to sector coordinates
        Vector2I currentSector = BlockUtil.SectorFromChunk(chunkPosition);

        // Determine the min and max points of the current zone
        Vector2I min = _position;
        Vector2I max = _position + new Vector2I(_loadedDimension - 1, _loadedDimension - 1);

        Debug.Log("Min: " + min + ", Max: " + max + ", Current: " + currentSector);

        // Is the sector out of the bounds or on the edge?
        if (currentSector.x >= max.x || currentSector.x <= min.x || currentSector.y >= max.y || currentSector.y <= min.y)
        {
            int half = (_loadedDimension - 1) / 2;
            Vector2I newCorner = currentSector - new Vector2I(half, half);

            List<Vector2I> newPositions = new List<Vector2I>();
            List<Vector2I> oldPositions = new List<Vector2I>();
            List<Sector> cachedPositions = new List<Sector>();

            // Find the new sectors to be part of the zone
            for (int i = 0; i < _loadedDimension; i++)
            {
                for (int j = 0; j < _loadedDimension; j++)
                {
                    newPositions.Add(newCorner + new Vector2I(i, j));
                    oldPositions.Add(_position + new Vector2I(i, j));
                }
            }

            int saving = 0;

            // Compare the old and new positions to determine which shall
            // remain cached and which need saving
            for (int i = 0; i < _activeSectors.Count; i++)
            {
                Sector sector = _activeSectors[i];

                // Only sectors which are existent (to prevent un-bounded sectors from being saved)
                if (sector != null)
                {
                    Vector2I pos = sector.position;

                    // This sector is going to be part of the new zone
                    if (newPositions.Contains(pos))
                    {
                        cachedPositions.Add(sector);
                    }
                    else // This sector needs saving as it will be despawned
                    {
                        // Push the sector for saving
                        queueSector(sector);
                        saving++;
                    }
                }
            }

            // Remove the cached positions from the new list
            for (int i = 0; i < cachedPositions.Count; i++)
            {
                for (int j = 0; j < newPositions.Count; j++)
                {
                    // Do they match? Remove the new position as it doesn't need loading
                    if (cachedPositions[i].position == newPositions[j])
                    {
                        newPositions.RemoveAt(j);
                        break;
                    }
                }
            }

            // Load the desired chunks
            for(int i = 0; i < newPositions.Count; i++)
            {
                _loading.Enqueue(newPositions[i]);
            }

            //// A flag used to keep state for the following logical loop
            //bool flag = false;

            //int loading = 0;
            //int caching = 0;

            //// Iterate over all the new positions and determine 
            //// which new positions need to be loaded 
            //for (int i = 0; i < newPositions.Count; i++)
            //{
            //    Vector2I pos = newPositions[i];

            //    // Iterate over the cache to find the position to see if it matches
            //    for (int j = 0; j < cachedPositions.Count; j++)
            //    {
            //        if (cachedPositions[j].position == pos)
            //        {
            //            flag = true;
            //            break;
            //        }
            //    }

            //    // The position isn't in the cache so it means it will need to be loaded
            //    if (!flag)
            //    {
            //        loading++;
            //        _loading.Enqueue(pos);
            //    }
            //    else
            //    {
            //        caching++;
            //    }
            //}

            //Debug.Log("Enqueued " + loading + " sectors to load and caching " + caching + " / " + newPositions.Count);

            // Clean up the active sectors ready to re-place them
            _activeSectors.Clear();

            // Re-add the sectors
            for (int i = 0; i < cachedPositions.Count; i++)
            {
                _activeSectors.Add(cachedPositions[i]);
            }

            //// Iterate over and re-position all cached sectors
            //for (int i = 0; i < _loadedDimension; i++)
            //{
            //    for (int j = 0; j < _loadedDimension; j++)
            //    {
            //        Vector2I pos = newCorner + new Vector2I(i, j);

            //        // Search through the cache to find the specified sector
            //        for (int k = 0; k < cachedPositions.Count; k++)
            //        {
            //            // The position has been found, set the sector
            //            if (cachedPositions[k].position == pos)
            //            {
            //                _sectors[i, j] = cachedPositions[k];
            //            }
            //        }

            //        // The sector was not found, allocate the sector as NULL
            //        _sectors[i, j] = null;
            //    }
            //}
            //}

            _position = newCorner;

        }
    }
    
}