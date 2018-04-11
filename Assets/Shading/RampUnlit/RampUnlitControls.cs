using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class RampUnlitControls : MonoBehaviour 
{
    MeshRenderer renderer;
    [SerializeField]
    CustomGradient _gradient;
    
    public CustomEmitter Emitter;
    
    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        Emitter = new CustomEmitter(Vector3.right);
        Emitter.Position = renderer.bounds.max * 1.5f;
    }

    public void UpdateShaderProperties()
    {
        Texture2D texture = _gradient.GetTexture(64);
        texture.wrapMode = TextureWrapMode.Clamp;

        renderer.material.SetTexture("_RampTex", texture);
        renderer.material.SetMatrix("_GradientRotation", Matrix4x4.Rotate(Emitter.Rotation));
    }

    void Reset()
    {
        Emitter = new CustomEmitter(Vector3.right);
        Emitter.Position = renderer.bounds.max * 1.5f;
    }
}