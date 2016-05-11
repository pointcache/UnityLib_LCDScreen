using UnityEngine;
using System.Collections;

//Used to locate root object
public class LCDScreenIdentifier : MonoBehaviour {

    [HideInInspector]
    public LCDScreen screen;
    [HideInInspector]
    public Renderer renderer;
    [HideInInspector]
    public MeshFilter meshFilter;
    [HideInInspector]
    public Canvas canvas;
    [HideInInspector]
    public Camera camera;
    [HideInInspector]
    public MakeRenderTexture makeRdTx;
    [HideInInspector]
    public int H, W;
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "lcdGizmo.psd");
    }

    void Start()
    {
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        makeRdTx.h = H;
        makeRdTx.w = W;
    }
}
