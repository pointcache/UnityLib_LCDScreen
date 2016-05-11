using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;

public static class LCDEditor  {

    [MenuItem("UnityLib/LCDScreen/ScreenTool")]
	public static void StartMakeScreen()
    {
        ScreenMaker window = EditorWindow.GetWindow<ScreenMaker>(true, "ScreenTool");
        window.ShowPopup();

    }

    static public void MakeScreen(int w, int h)
    {
        GameObject root = new GameObject("LCD_Screen");
        GameObject screenGO = new GameObject("screen");

        var LCDID = root.AddComponent<LCDScreenIdentifier>();

        screenGO.transform.SetParent(root.transform);
        LCDID.screen = screenGO.AddComponent<LCDScreen>();
        LCDID.renderer = screenGO.AddComponent<MeshRenderer>();
        LCDID.meshFilter = screenGO.AddComponent<MeshFilter>();
        LCDID.meshFilter.sharedMesh = ((GameObject)Resources.Load("UnityLib_LCDScreen/lcdScreenPlane")).GetComponent<MeshFilter>().sharedMesh;

        LCDID.screen.rootGO = root;
        LCDID.renderer.material = Material.Instantiate(Resources.Load("UnityLib_LCDScreen/LCD_material") as Material);

        GameObject canvasGO = new GameObject("Canvas");
        LCDID.canvas = canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        canvasGO.transform.SetParent(root.transform);

        LCDID.canvas.renderMode = RenderMode.ScreenSpaceCamera;
        LCDID.canvas.planeDistance = .5f;
        LCDID.canvas.gameObject.layer = LayerMask.NameToLayer("UI");
        GameObject cameraGO = new GameObject("ScreenCamera");
        LCDID.camera = cameraGO.AddComponent<Camera>();

        LCDID.makeRdTx = LCDID.camera.gameObject.AddComponent<MakeRenderTexture>();
        LCDID.makeRdTx.w = w;
        LCDID.makeRdTx.h = h;

        LCDID.camera.orthographic = true;
        LCDID.camera.farClipPlane = 1f;
        LCDID.camera.useOcclusionCulling = false;
        LCDID.camera.hdr = true;
        LCDID.camera.transform.SetParent(root.transform);
        LCDID.camera.orthographicSize = .5f;
        LCDID.camera.transform.localPosition = new Vector3(LCDID.camera.transform.localPosition.x, LCDID.camera.transform.localPosition.y, LCDID.camera.transform.localPosition.z - .5f);
        LCDID.camera.backgroundColor = Color.black;
        LCDID.camera.cullingMask = LayerMask.GetMask("UI");

        LCDID.canvas.worldCamera = LCDID.camera;
        
        LCDID.screen.maincam = Camera.main.transform;
        LCDID.screen.uiCamera = LCDID.camera;
        LCDID.screen.canvas = LCDID.canvas;

        var tex = new RenderTexture(w, h, 24);
        LCDID.camera.targetTexture = tex;
        LCDID.H = h;
        LCDID.W = w;
        Vector3 scale = new Vector3();
        scale.y = LCDID.camera.orthographicSize * 2;
        scale.x = scale.y * LCDID.camera.aspect;
        scale.z = 1f;

        screenGO.transform.localScale = scale;
        
        Selection.activeGameObject = root;  
    }

    public static void ResizeScreen(int w, int h)
    {
        var LCDID = Selection.activeGameObject.GetComponent<LCDScreenIdentifier>();
        if (!LCDID)
        {
            Debug.LogError("Select root of the Screen rig.");
            return;
        }
        var tex = new RenderTexture(w, h, 24);
        
        LCDID.camera.targetTexture = tex;

        Vector3 scale = new Vector3();
        scale.y = LCDID.camera.orthographicSize * 2;
        scale.x = scale.y * LCDID.camera.aspect;
        scale.z = 1f;

        LCDID.H = h;
        LCDID.W = w;
        LCDID.screen.gameObject.transform.localScale = scale;
        
    }

    static public void ResetMaterial()
    {
        var LCDID = Selection.activeGameObject.GetComponent<LCDScreenIdentifier>();
        if (!LCDID)
        {
            Debug.LogError("Select root of the Screen rig.");
            return;
        }

        LCDID.renderer.material = Material.Instantiate(Resources.Load("UnityLib_LCDScreen/LCD_material") as Material);
    }

    static public LCDScreenIdentifier GetSize()
    {
        var LCDID = Selection.activeGameObject.GetComponent<LCDScreenIdentifier>();
        if (!LCDID)
        {
            Debug.LogError("Select root of the Screen rig.");
            return null;
        }
        return LCDID;
    }

    
}
