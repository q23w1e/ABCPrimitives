using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearClone: MonoBehaviour
{
    public GameObject prefab;
   
    int _count = 4;
    Vector3 _end = 4f * Vector3.right;
    Stack<GameObject> clones = new Stack<GameObject>{};

    public Vector3 StartPosition
    {
        get { return this.transform.position; }
    }
    public Vector3 EndPosition
    {
        get { return this.transform.TransformPoint(_end); }
        set 
        {
            if (this.transform.InverseTransformPoint(value) != _end) 
            {
                _end = this.transform.InverseTransformPoint(value);
                recalculatePositions(); 
            }
        }
    }

    public int Count
    {
        get { return _count; }
        set 
        { 
            if ( _count == value) { return; };
            _count = Mathf.Max(1, value);
        }
    }

    public void updateClonesList()
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

    public void recalculatePositions()
    {
        int n = (clones.Count - 1) == 0 ? 1 : (clones.Count - 1);
        Vector3 d = (EndPosition - StartPosition) / (float)n;
        int i = 0;
        foreach (var clone in clones)
        {
            if (clone) { clone.transform.position = this.transform.position + (d * i); } 
            i++;
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