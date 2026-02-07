using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Linq;

public class TilesetAutobuilder : EditorWindow
{
    [MenuItem("Tools/Build 25 Animated Tiles")]
    public static void ShowWindow() => GetWindow<TilesetAutobuilder>("Tile Builder");

    public Sprite tilesetTexture;
    public string folderName = "AnimatedTiles_Output";

    void OnGUI()
    {
        tilesetTexture = (Sprite)EditorGUILayout.ObjectField("100-Sprite Texture", tilesetTexture, typeof(Sprite), false);
        folderName = EditorGUILayout.TextField("Output Folder", folderName);

        if (GUILayout.Button("Generate Tiles") && tilesetTexture != null)
        {
            Generate();
        }
    }

    void Generate()
    {
        string path = $"Assets/{folderName}";
        if (!AssetDatabase.IsValidFolder(path)) AssetDatabase.CreateFolder("Assets", folderName);

        // Load all 100 sprites and sort them correctly: Top-to-Bottom, then Left-to-Right
        string texPath = AssetDatabase.GetAssetPath(tilesetTexture);
        Sprite[] allSprites = AssetDatabase.LoadAllAssetsAtPath(texPath)
            .OfType<Sprite>()
            .OrderByDescending(s => s.rect.y)
            .ThenBy(s => s.rect.x)
            .ToArray();

        if (allSprites.Length < 100) {
            Debug.LogError("Texture doesn't have 100 sprites! Check your slicing.");
            return;
        }

        for (int i = 0; i < 25; i++)
        {
            // row and col within the 5x5 area
            int row = i / 5;
            int col = i % 5;

            // Jump across the 20-tile wide strip to grab matching frames
            int f1 = (row * 20) + col;
            int f2 = f1 + 5;
            int f3 = f1 + 10;
            int f4 = f1 + 15;

            AnimatedTile tile = ScriptableObject.CreateInstance<AnimatedTile>();
            tile.m_AnimatedSprites = new Sprite[] { allSprites[f1], allSprites[f2], allSprites[f3], allSprites[f4] };
            tile.m_MinSpeed = 1f;
            tile.m_MaxSpeed = 1f;

            AssetDatabase.CreateAsset(tile, $"{path}/Tile_{i}.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Created 25 Animated Tile Assets.");
    }
}