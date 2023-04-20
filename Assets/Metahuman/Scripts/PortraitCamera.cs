using UnityEngine;

public class PortraitCamera : MonoBehaviour
{
    private void Awake()
    {
        var cam = GetComponent<Camera>();
        var renderTexture = cam.targetTexture;
        var newRenderTexture = new RenderTexture(renderTexture);
        cam.targetTexture = newRenderTexture;
    }
}
