using UnityEngine;
using MeshConstructor;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralGrid: MonoBehaviour
{
    int _width = 1;
    int _height = 1;
    Mesh _mesh;
    Vector3[] _vertices;
    int[] _triangles;

    public int Width
    {
        get { return _width; }
        set
        {
            value = Mathf.Max(1, value);
            if (value != _width)
            {
                _width = value;
            }
        }
    }

    public int Height
    {
        get { return _height; }
        set
        {
            value = Mathf.Max(1, value);
            if (value != _height)
            {
                _height = value;
            }
        }
    }

    void Awake() 
    {
        //using shared vertex colors is not my case
        _mesh = GetComponent<MeshFilter>().mesh;
    }

    public void Generate()
    {
        _mesh.Clear();
        
        _mesh.vertices = GridXZ.GenerateVertices(Width, Height);
        _mesh.triangles = GridXZ.GenerateTriangles(Width, Height);
        _mesh.normals = GridXZ.GenerateNormals(Width, Height, Vector3.up);
    }

    void Reset() 
    {
        _width = 1;
        _height = 1;
        if (_mesh) { _mesh.Clear(); }
    }
}
