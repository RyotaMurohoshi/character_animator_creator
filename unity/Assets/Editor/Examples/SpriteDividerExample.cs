using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public static class SpriteDividerExample
{
    [MenuItem("Assets/CharacterAnimatorCreator/Example/Divide Textures")]
    public static void DivideImages()
    {
        int horizontalCount = 4;
        int verticalCount = 4;

        IEnumerable<Texture> targets = Selection.objects.OfType<Texture>();

        if (!targets.Any())
        {
            Debug.LogWarning("Please selecting textures.");
            return;
        }

        foreach (Texture target in targets)
        {
            string assetPath = AssetDatabase.GetAssetPath(target);
            SpriteDivider.Execute(assetPath, horizontalCount, verticalCount);
        }
    }
}

