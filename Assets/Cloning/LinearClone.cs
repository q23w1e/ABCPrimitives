using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LinearClone: CloneBase
{
    Vector3 _start;
    Vector3 _end;

    public Vector3 StartPosition
    {
        get { return this.transform.TransformPoint(_start); }
        set 
        {
            if (this.transform.InverseTransformPoint(value) != _start) 
            {
                _start = this.transform.InverseTransformPoint(value);
                recalculatePositions(); 
            }
        }
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

    public override void recalculatePositions()
    {
        int n = (clones.Count - 1) == 0 ? 1 : (clones.Count - 1);
        Vector3 d = (EndPosition - StartPosition) / (float)n;
        int i = 0;
        foreach (var clone in clones)
        {
            if (clone) { clone.transform.position = StartPosition + d * i; } 
            i++;
        }
    }

    void Awake()
    {
        Init();

        StartPosition = 4 * Vector3.left;
        EndPosition = -4 * Vector3.left;
    }
}