using System.Collections.Generic;
using UnityEngine;

public abstract class CloneBase : MonoBehaviour 
{
	public GameObject prefab;
   
    protected GameObject container;
    protected Stack<GameObject> clones = new Stack<GameObject>{};
    
    int _count;

	public int Count
    {
        get { return _count; }
        set 
        { 
            if (_count == value) { return; };
            _count = Mathf.Max(0, value);
        }
    }

    protected void Init()
    {
        if (!container)
        {
            container = new GameObject();  
            container.transform.position = Vector3.zero;
            container.name = this.GetType().Name;
            this.transform.parent = container.transform;
            this.name = "Tool";
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
                --diff;
                GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                clone.transform.parent = container.transform;
                clones.Push(clone);
            }
            else
            {
                ++diff;
                DestroyImmediate(clones.Pop());
            }
        }
    }

	public void Clear()
    {
        foreach (var clone in clones)
        {
            if (clone) { DestroyImmediate(clone); }
        }
        clones.Clear();
        Count = 0;
    }

    public abstract void recalculatePositions();
}

