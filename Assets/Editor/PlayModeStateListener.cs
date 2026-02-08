using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class PlayModeStateListener
{
    static PlayModeStateListener()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            AfterPlayModeStops();
        }
    }

    private static void AfterPlayModeStops()
    {
        string matPath = "Assets/Shaders/DamageMAT.mat";
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);

        if (mat != null)
        {
            mat.SetFloat("_VignetteIntensity", 0.0f);

            EditorUtility.SetDirty(mat);
            AssetDatabase.SaveAssets();
        
            Debug.Log($"[PlayModeStateListener] {mat.name} reset.");
        }
        else
        {
            Debug.LogWarning($"[PlayModeStateListener] Material at {matPath} not found.");
        }
    }
}