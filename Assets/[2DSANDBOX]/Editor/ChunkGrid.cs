using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

namespace Assets.NewSystem
{
    public class ChunkGrid
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.NonSelected)]
        public static void drawChunks(World world, GizmoType type)
        {
            return; // Temporary to fix issues...

            WorldProperties properties = world.properties;
            List<ChunkWorldObject> chunks = world.activeChunks;

            if (chunks != null && world != null)
            {
                // Draw the chunks
                for (int i = 0; i < chunks.Count; i++)
                {
                    ChunkWorldObject obj = chunks[i];
                    Chunk chunk = obj.chunk;

                    if (chunk != null)
                    {
                        int size = Chunk.tileDimension;

                        if (chunk.dirty)
                        {
                            Gizmos.color = Color.red;
                        }
                        else
                        {
                            Gizmos.color = Color.green;
                        }

                 //       Gizmos.DrawWireCube(obj.transform.position + new Vector3(size / 2, size / 2, 0) - new Vector3(0.5f, 0.5f, 0.0f), new Vector3(size, size, 0));

                        Vector2I pos = chunk.position;

                    }
                }
                
                MapHandler handler = world.mapHandler;
                List<Sector> sectors = handler.activeSectors;

                // Draw the sectors
                for (int i = 0; i < sectors.Count; i++)
                {
                    Sector sector = sectors[i];

                    if (sector != null)
                    {
                        Vector2I pos = sector.position;

                        int size = Sector.chunkDimension * Chunk.tileDimension;

                        Vector3 wPos = Vector3.zero;

                        Gizmos.color = Color.white;

                        Gizmos.DrawWireCube(new Vector3((pos.x * size) + (size/2.0f), (pos.y * size) + (size / 2.0f), 0) - new Vector3(0.5f, 0.5f, 0.0f), new Vector3(size, size, 0));
                    }
                }
            }

        }
    }
}