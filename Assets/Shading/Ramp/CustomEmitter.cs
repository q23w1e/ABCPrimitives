using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomEmitter
{
    public Quaternion Rotation { get; set; }
    
    public Vector3 Position
    {
        get; set;
    }
    
    public Vector3 Direction
    {
        get
        {
            Matrix4x4 m = Matrix4x4.Rotate(Rotation);
            return m.MultiplyVector(new Vector3(0f, 0f, 1f));
        }
    }

    public CustomEmitter()
    {
        Rotation = Quaternion.identity;
    }
}
