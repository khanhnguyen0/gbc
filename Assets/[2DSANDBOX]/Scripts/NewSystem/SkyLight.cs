using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SkyLight : MonoBehaviour
{
    [SerializeField]
    private Color _topColour;
    [SerializeField]
    private Color _bottomColour;
    private bool _flag;

    private Mesh _mesh;
    private MeshFilter _filter;

    public Color topColour
    {
        get { return _topColour; }
        set
        {
            _topColour = value;
            _flag = true;
        }
    }

    public Color bottomColour
    {
        get { return _bottomColour; }
        set
        {
            _bottomColour = value;
            _flag = true;
        }
    }

    void Awake()
    {
        _mesh = new Mesh();
        _filter = GetComponent<MeshFilter>();
        _filter.sharedMesh = _mesh;
        _flag = true;
    }

    void Update()
    {
        if (_flag)
        {
            _flag = false;
            rebuild();
        }
    }

    private void rebuild()
    {
        Vector3[] v = new Vector3[4];
        int[] t = new int[6];
        Color[] c = new Color[4];

        v[0] = new Vector3(-0.5f, -0.5f, 0);
        v[1] = new Vector3(0.5f, -0.5f, 0);
        v[2] = new Vector3(0.5f, 0.5f, 0);
        v[3] = new Vector3(-0.5f, 0.5f, 0);

        t[0] = 0;
        t[1] = 3;
        t[2] = 2;
        t[3] = 2;
        t[4] = 1;
        t[5] = 0;

        c[0] = _bottomColour;
        c[1] = _bottomColour;
        c[2] = _topColour;
        c[3] = _topColour;

        _mesh.vertices = v;
        _mesh.triangles = t;
        _mesh.colors = c;

        _mesh.RecalculateNormals();
        ;
    }
}