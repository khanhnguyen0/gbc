using UnityEngine;
using System.Collections;

// Define random to be unity random and not system random
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(ChunkCollider)), System.Serializable]
public class ChunkWorldObject : MonoBehaviour
{
    [SerializeField]
    private Chunk _chunk;

    private Mesh _mesh;
    private World _world;
    private MeshRenderer _renderer;
    private MeshFilter _filter;
    private ChunkCollider _collider;

    public Mesh mesh { get { return _mesh; } }
    public Chunk chunk { get { return _chunk; } }

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _filter = GetComponent<MeshFilter>();
        _collider = GetComponent<ChunkCollider>();

        _collider.initialize(Chunk.tileDimension);

        _mesh = new Mesh();
        _filter.sharedMesh = _mesh;
    }

    void Update()
    {
        // Only perform update logic if a chunk is attached
        if (_chunk != null)
        {
            if (_chunk.dirty) // If the chunk has been flagged for meshing, queue it and unflag
            {
                Mesher.queueMeshingTask(_chunk);
                _chunk.setDirtyState(false);

                // TEMP
                // Update the collider
                _collider.setData(chunk.tiles);

            }
        }
    }

    public void toggleVisiblity(bool state)
    {
        _renderer.enabled = state;
    }

    /// <summary>
    /// Assign the material of the chunks renderer
    /// </summary>
    /// <param name="material"></param>
    public void setMaterial(Material material)
    {
        _renderer.material = material;
    }

    /// <summary>
    /// Reset the mesh of the chunk
    /// </summary>
    public void clearMesh()
    {
        _mesh.Clear();
    }

    /// <summary>
    /// Assing the chunk data to this container
    /// </summary>
    /// <param name="chunk"></param>
    public void setChunk(Chunk chunk)
    {
        _chunk = chunk;

        // Make sure the chunk isn't NULL
        if(chunk != null)
        {
            Vector2I position = _chunk.position;

            // Update the collider
            _collider.setData(chunk.tiles);

            name = position.ToString();
            transform.position = new Vector3(position.x * chunk.dimension, position.y * chunk.dimension, 5);
        }
        else // Set to a NULL chunk
        {
            name = "Empty Chunk";
        }
    }
}