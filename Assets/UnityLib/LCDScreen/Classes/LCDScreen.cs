using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

//Works with camera
public class LCDScreen : MonoBehaviour {

    [HideInInspector]
    public GameObject rootGO;
    [HideInInspector]
    public Camera uiCamera;
    [HideInInspector]
    public Transform maincam;
    [HideInInspector]
    public Canvas canvas;
    MakeRenderTexture makeRenderTexture;

    public float filteringSwitchProximity = .25f;
    Material mat;

    bool isInUi;
    RawImage rawImage;
    Vector3 trpoint;

    public void Reinit()
    {
        //transform.localScale = new Vector3(makeRenderTexture.tex.width * scale, makeRenderTexture.tex.height * scale, 1);
        mat.SetTextureScale("_PixelClose", new Vector2(makeRenderTexture.tex.width, makeRenderTexture.tex.height));
        mat.SetTextureScale("_PixelFar", new Vector2(makeRenderTexture.tex.width, makeRenderTexture.tex.height));
        SetScale();
    }

	// Use this for initialization
	void Awake () {
        Renderer rd = GetComponent<Renderer>();
        rd.enabled = true;
        if (GetComponent<RawImage>())
        {
            isInUi = true;
            rawImage = GetComponent<RawImage>();
            mat = rawImage.material;
        }
        else
            mat = rd.material;
        makeRenderTexture = uiCamera.GetComponent<MakeRenderTexture>();
        //Reinit();
        StartCoroutine(GetRenderTexture());
        
    }

    public void SetScale()
    {
        Vector3 scale = new Vector3();
        scale.y = uiCamera.orthographicSize * 2;
        scale.x = scale.y * uiCamera.aspect;
        scale.z = 1f;

        transform.localScale = scale;
    }

    void Update()
    {
        trpoint = transform.InverseTransformPoint(maincam.transform.position);
        if(Mathf.Abs(trpoint.z )< filteringSwitchProximity)
        {
            if(makeRenderTexture.tex.filterMode!= FilterMode.Point)
            makeRenderTexture.tex.filterMode = FilterMode.Point;
        }
        else
        {
            if(makeRenderTexture.tex.filterMode != FilterMode.Trilinear)
            makeRenderTexture.tex.filterMode = FilterMode.Trilinear;
        }
        mat.SetFloat("_CamDistance", Vector3.Distance(rootGO.transform.position, maincam.transform.position));
    }

    IEnumerator GetRenderTexture()
    {
        while (true)
        {
            if (!makeRenderTexture.tex)
            {
                yield return null;
            }
            else
            {
                if (isInUi)
                    rawImage.texture = makeRenderTexture.tex;
                mat.mainTexture = makeRenderTexture.tex;
                Reinit();
                yield break;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "lcdGizmo.psd");
    }
}
