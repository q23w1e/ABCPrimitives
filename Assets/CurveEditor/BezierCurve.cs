using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour 
{
	public Vector3[] points = new Vector3[] {
		new Vector3(-0.5f, 0f, 0f),
		new Vector3(0, 0f, 0.5f),
		new Vector3(0.5f, 0f, -0.5f),
		new Vector3(1.0f, 0f, 0f),
	};

	Vector3[] _transformedPoints = new Vector3[3];

	// each time the whole array will be recalcualted=|
	public Vector3[] TransformedPoints
	{
		get 
		{
			for (int i = 0; i < points.Length; i++)
				_transformedPoints[i] = this.transform.TransformPoint(points[i]);

			return _transformedPoints;
		}
	}

	public Vector3 getPoint(float t)
	{
		return this.transform.TransformPoint(BezierCubic.getPoint(points[0], points[1], points[2], points[3], t));
	}

	public Vector3 getVelocity(float t)
	{
		return Vector3.zero;
	}
}
