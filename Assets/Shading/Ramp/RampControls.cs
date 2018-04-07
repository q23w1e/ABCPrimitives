using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class RampControls : MonoBehaviour 
{
    MeshRenderer renderer;

    [SerializeField]
    CustomGradient _gradient;
    
    public CustomEmitter Emitter;

    void OnEnable() 
    {
        renderer = GetComponent<MeshRenderer>();
        Emitter.Position = renderer.bounds.max * 1.5f;
    }

    void Awake()
    {
        Emitter = new CustomEmitter();
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