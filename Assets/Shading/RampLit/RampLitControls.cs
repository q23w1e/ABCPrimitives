using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class RampLitControls : MonoBehaviour 
{
    MeshRenderer renderer;
    [SerializeField]
    CustomGradient _gradient;
    
    public CustomEmitter Emitter;

    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        Emitter = new CustomEmitter(Vector3.forward);
        Emitter.Position = renderer.bounds.max * 1.5f;
    }

    public void UpdateShaderProperties()
    {
        Vector3 direction = Emitter.Direction;
        Texture2D texture = _gradient.GetTexture(64);
        texture.wrapMode = TextureWrapMode.Clamp;

        renderer.material.SetVector("_EmitterDir", new Vector4(direction.x, direction.y, direction.z, 0));
        renderer.material.SetTexture("_RampTex", texture);
    }
}