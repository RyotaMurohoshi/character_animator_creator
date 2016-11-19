using UnityEngine;
using UnityEditor;

public static class SimplePrefabCreatorExample
{
    [MenuItem("Assets/CharacterAnimatorCreator/Create Empty Prefab")]
    public static void Execute()
    {
        string name = "target";
        string outputPath = "Assets/Prefab.prefab";

        GameObject gameObject = EditorUtility.CreateGameObjectWithHideFlags(name, HideFlags.HideInHierarchy);

        PrefabUtility.CreatePrefab(outputPath, gameObject);

        Editor.DestroyImmediate(gameObject);
    }
}

