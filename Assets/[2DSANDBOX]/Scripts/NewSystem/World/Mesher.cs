using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Threading;

public static class Mesher
{
    public const int meshesPerChunk = 4;
    private static Pool<Builder> builders;
    private static Queue<Chunk> queue;
    private static bool initialized;
    private static WorldProperties properties;

    public static void queueMeshingTask(Chunk chunk)
    {
        queue.Enqueue(chunk);
    }

    public static void initialize()
    {
        if (initialized)
        {
            //throw new System.Exception("Already initialized!");
        }
        else
        {
            initialized = true;
            builders = new Pool<Builder>();
            queue = new Queue<Chunk>();
        }
    }

    /// <summary>
    /// Iterate over the job queue of meshes to be performed and initialize builders to
    /// construct the mesh of that chunk
    /// </summary>
    public static void meshLoop()
    {
        for (int i = 0; i < meshesPerChunk; i++)
        {
            if (queue.Count > 0)
            {
                Chunk chunk = queue.Dequeue();
                Builder builder = builders.get();
                builder.buildMesh(chunk);
            }
        }
    }
}