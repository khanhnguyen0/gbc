using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Vector3I
{
    [SerializeField]
    private int _x;
    [SerializeField]
    private int _y;
    [SerializeField]
    private int _z;

    public int x { get { return _x; } }
    public int y { get { return _y; } }
    public int z { get { return _z; } }

    public Vector3I(int x, int y, int z)
    {
        this._x = x;
        this._y = y;
        this._z = z;
    }

    public Vector3I(float x, float y, float z)
    {
        this._x = Mathf.RoundToInt(x);
        this._y = Mathf.RoundToInt(y);
        this._z = Mathf.RoundToInt(z);
    }

    public Vector3I(Vector3 pos) : this(pos.x, pos.y, pos.z) { }

    public static Vector3I operator +(Vector3I lhs, Vector3I rhs)
    {
        return new Vector3I(lhs._x + rhs._x, lhs._y + rhs._y, lhs.z + rhs.z);
    }

    public static Vector3I operator -(Vector3I lhs, Vector3I rhs)
    {
        return new Vector3I(lhs._x - rhs._x, lhs._y - rhs._y, lhs.z - rhs.z);
    }

    public override string ToString()
    {
        return string.Format("({0},{1},{2})", _x, _y, _z);
    }
}