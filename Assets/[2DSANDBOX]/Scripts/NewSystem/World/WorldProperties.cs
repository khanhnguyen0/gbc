using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(fileName = "World Properties", menuName = "World Properties")]
public class WorldProperties : ScriptableObject
{
    [SerializeField]
    private int _worldDimension;
    [SerializeField]
    private int _chunkDimension;
    [SerializeField]
    private int _worldHeight;

    public int worldDimension { get { return _worldDimension; } }
    public int chunkDimension { get { return _chunkDimension; } }
    public int worldHeight { get { return _worldHeight; } }
}
