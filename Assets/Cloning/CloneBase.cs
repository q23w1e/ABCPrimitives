using System.Collections.Generic;
using UnityEngine;

public abstract class CloneBase : MonoBehaviour 
{
    public GameObject prefab;
   
    public Transform containerTransform;
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
        if (this.transform.parent) 
        {
            containerTransform = this.transform.parent;
            return; 
        }
        
        containerTransform = new GameObject(this.GetType().Name).transform;
        containerTransform.transform.position = this.transform.position;
        this.transform.parent = containerTransform.transform;
        this.name = "Tool";
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
                GameObject clone = Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
                clone.transform.parent = containerTransform;
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

