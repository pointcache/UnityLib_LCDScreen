using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class MakeRenderTexture : MonoBehaviour {
    public RenderTexture tex;
    public int w, h;
    // Use this for initialization
    void Start () {
        Camera cam = GetComponent<Camera>();
        tex = new RenderTexture(w, h, 24);
        cam.targetTexture = tex;    
	}
}
