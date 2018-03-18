using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Axis
{
	public static Vector3 X = Vector3.right;
	public static Vector3 Y = Vector3.up;
	public static Vector3 Z = Vector3.forward;
}



public class RadialClone : MonoBehaviour 
{
	public GameObject prefab;
   
    int _count = 4;
	float _radius = 1f;
    Stack<GameObject> clones = new Stack<GameObject>{};

	#region Public Properties

	public float Radius
	{
		get { return _radius; }
		set { _radius = Mathf.Max(0, value); }
	}

	public int Count
    {
        get { return _count; }
        set 
        { 
            if ( _count == value) { return; };
            _count = Mathf.Max(0, value);
        }
    }

	#endregion

    Vector3 rotateAroundY(float theta)
    {
        Vector3 v = Vector3.forward;
        float cos = Mathf.Cos(theta),
              sin = Mathf.Sin(theta);
        
        // good sign to start learning about quaterions...
        return new Vector3(
            v.x * cos + v.z * sin,
            v.y,
            v.x * (-sin) + v.z * cos    
        );
    }

	public void recalculatePositions()
	{
		int n = clones.Count;
		float phi = 2 * Mathf.PI / n;
		int i = 1;

		foreach (var clone in clones)
		{
			if (clone) 
			{ 
				clone.transform.parent = this.transform;
				clone.transform.localPosition = Radius * rotateAroundY(phi * i);
				clone.transform.LookAt(this.transform.position);
			}
			i++;
		}
	}

	public void updateClonesStack()
    {
        if (!prefab) 
        {
            Debug.LogWarning("Source prefab is empty");
            return;
        }

        int diff = Count - clones.Count;
        while (diff != 0)
        {
            if (diff > 0)
            {
                diff--;
                GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                clones.Push(clone);
            }
            else
            {
                diff++;
                DestroyImmediate(clones.Pop());
            }
        }
    }

	public void Clear()
    {
        foreach (var clone in clones)
        {
            if (clone) DestroyImmediate(clone);
            
        }
        clones.Clear();
        Count = 0;
    }
}
