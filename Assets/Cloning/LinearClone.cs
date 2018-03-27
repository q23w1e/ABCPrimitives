using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LinearClone: CloneBase
{
    public Vector3 start;
    public Vector3 end;

    public Vector3 StartPosition
    {
        get { return this.transform.TransformPoint(start); }
        set 
        {
            if (this.transform.InverseTransformPoint(value) != start) 
            {
                start = this.transform.InverseTransformPoint(value);
                recalculatePositions(); 
            }
        }
    }

    public Vector3 EndPosition
    {
        get { return this.transform.TransformPoint(end); }
        set 
        {
            if (this.transform.InverseTransformPoint(value) != end) 
            {
                end = this.transform.InverseTransformPoint(value);
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

        Count = 4;
        start = 4 * Vector3.left;
        EndPosition = -4 * Vector3.left;
    }
}