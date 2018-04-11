using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomEmitter
{
    Vector3 _baseDirection;

    public Quaternion Rotation { get; set; }
    
    public Vector3 Position { get; set; }
    
    public Vector3 Direction
    {
        get
        {
            Matrix4x4 m = Matrix4x4.Rotate(Rotation);
            return m.MultiplyVector(_baseDirection);
        }
    }

    public CustomEmitter(Vector3 baseDirection)
    {
        _baseDirection = baseDirection;
        Rotation = Quaternion.identity;
        Position = Vector3.zero;
    }
}
