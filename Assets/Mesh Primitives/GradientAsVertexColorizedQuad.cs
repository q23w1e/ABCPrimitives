using UnityEngine;
using MeshConstructor;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GradientAsVertexColorizedQuad : MonoBehaviour 
{
    Mesh _mesh;
    public CustomGradient _gradient;

    void Awake()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
    }
 
    public void GenerateVertexColorsFromGradient()
    {
        _mesh.Clear();
        
        int width = _gradient.ColorKeys.Count - 1;
        int height = 1;

        _mesh.vertices = GridXZ.GenerateVertices(width, height, unitQuad: true);
        _mesh.triangles = GridXZ.GenerateTriangles(width, height);
        _mesh.normals = GridXZ.GenerateNormals(width, height, Vector3.up);

        Color[] colors = new Color[_mesh.vertices.Length];
        for (int i = 0; i <= width; i++)
        {
            Color color = _gradient.ColorKeys[i].Color;
            colors[i] = color;
            colors[i + (width + 1)] = color;
        }
        _mesh.colors = colors;
    }

    void Start() {
        GenerateVertexColorsFromGradient();
    }
}