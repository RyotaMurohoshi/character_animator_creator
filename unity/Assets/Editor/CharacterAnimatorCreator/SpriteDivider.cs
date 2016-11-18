using UnityEditor;
using UnityEngine;
using System.Linq;

public static class SpriteDivider
{
    public static void Execute(string texturePath, int horizontalCount, int verticalCount)
    {
        var texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture)) as Texture;
        var pixelPerUnit = Mathf.Min(texture.width / horizontalCount, texture.height / verticalCount);

        Execute(texturePath, horizontalCount, verticalCount, pixelPerUnit);
    }

    public static void Execute(string texturePath, int horizontalCount, int verticalCount, int pixelPerUnit)
    {
        var importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;

        Texture texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture)) as Texture;
        importer.spritePixelsPerUnit = pixelPerUnit;
        importer.spritesheet = CreateSpriteMetaDataArray(texture, horizontalCount, verticalCount);

        AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
    }

    static SpriteMetaData[] CreateSpriteMetaDataArray(Texture texture, int horizontalCount, int verticalCount)
    {
        float spriteWidth = texture.width / horizontalCount;
        float spriteHeight = texture.height / verticalCount;

        return Enumerable
            .Range(0, horizontalCount * verticalCount)
            .Select(index =>
            {
                int x = index % horizontalCount;
                int y = index / horizontalCount;

                return new SpriteMetaData
                {
                    name = string.Format("{0}_{1}", texture.name, index),
                    rect = new Rect(spriteWidth * x, texture.height - spriteHeight * (y + 1), spriteWidth, spriteHeight)
                };
            })
            .ToArray();
    }
}

