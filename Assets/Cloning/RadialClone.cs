using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RadialClone : CloneBase 
{
	float _radius;

	public float Radius
	{
		get { return _radius; }
		set { _radius = Mathf.Max(0, value); }
	}

    void Awake()
    {
        Init();

        Radius = 10f;
    }

	public override void recalculatePositions()
	{
		int n = clones.Count;
		float phi = 2 * Mathf.PI / n;
		int i = 1;

		foreach (var clone in clones)
		{
			if (clone) 
			{ 
				clone.transform.localPosition = Radius * rotateAroundY(phi * i);
			}
			i++;
		}
	}

    Vector3 rotateAroundY(float theta)
    {
        Vector3 v = Vector3.forward;
        float cos = Mathf.Cos(theta),
              sin = Mathf.Sin(theta);
        
        // it's a sign to start learning about quaternions...
        return new Vector3(
            v.x * cos + v.z * sin,
            v.y,
            v.x * (-sin) + v.z * cos
        );
    }
}
