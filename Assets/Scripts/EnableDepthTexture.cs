using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDepthTexture : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Camera camera = GetComponent<Camera>();
        camera.depthTextureMode |= DepthTextureMode.Depth;
    }
}
