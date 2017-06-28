using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[System.Serializable]
public struct Vector2I
{
    [SerializeField]
    private int _x;
    [SerializeField]
    private int _y;

    public int x { get { return _x; } }
    public int y { get { return _y; } }

    public Vector2I(int x, int y)
    {
        this._x = x;
        this._y = y;
    }

    public Vector2I(float x, float y)
    {
        this._x = Mathf.RoundToInt(x);
        this._y = Mathf.RoundToInt(y);
    }

    public Vector2I(Vector2 pos) : this(pos.x, pos.y) { }

    public static Vector2I operator +(Vector2I lhs, Vector2I rhs)
    {
        return new Vector2I(lhs._x + rhs._x, lhs._y + rhs._y);
    }

    public static Vector2I operator -(Vector2I lhs, Vector2I rhs)
    {
        return new Vector2I(lhs._x - rhs._x, lhs._y - rhs._y);
    }

    public static bool operator ==(Vector2I lhs, Vector2I rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public static bool operator !=(Vector2I lhs, Vector2I rhs)
    {
        return !(lhs == rhs);
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", _x, _y);
    }
}
