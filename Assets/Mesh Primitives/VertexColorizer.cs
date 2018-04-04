using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class VertexColorizer : MonoBehaviour 
{
	Mesh _mesh;
	Color _color;

	public Color Color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
			GenerateVertexColors();
		}
	}

	void Awake() 
	{
		_mesh = GetComponent<MeshFilter>().sharedMesh;
		_color = Color.white;
	}

	void GenerateVertexColors()
	{
		Color[] colors = new Color[_mesh.vertexCount];
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i] = Color;
		}

		_mesh.colors = colors;
	}

	void Reset()
	{
		Color = Color.white;
	}
}
