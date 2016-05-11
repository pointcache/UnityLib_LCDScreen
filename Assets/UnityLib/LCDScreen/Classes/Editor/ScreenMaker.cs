using UnityEngine;
using UnityEditor;
using System.Collections;

public class ScreenMaker : EditorWindow
{
    public int ScreenHeight = 360, ScreenWidth = 480;

    public void OnGUI()
    {
        EditorGUILayout.Separator();
        ScreenWidth = EditorGUILayout.IntField("Width", ScreenWidth);
        ScreenHeight = EditorGUILayout.IntField("Height", ScreenHeight);

        

        if (GUILayout.Button("Make"))
        {
            LCDEditor.MakeScreen(ScreenWidth, ScreenHeight);
        }
        if (GUILayout.Button("Get size"))
        {
            var id = LCDEditor.GetSize();
            if (id)
            {
                ScreenHeight = id.H;
                ScreenWidth = id.W;
                Debug.Log("ScreenSize is :" + id.W + "x" + id.H);
            }

        }
        EditorGUILayout.Separator();
        if (GUILayout.Button("Resize"))
        {

            LCDEditor.ResizeScreen(ScreenWidth, ScreenHeight);
        }
        if (GUILayout.Button("Reset material"))
        {
            LCDEditor.ResetMaterial();
        }
    }
}