using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public static class SimpleCharacterAnimatorControllerCreatorExample
{
    [MenuItem("Assets/CharacterAnimatorCreator/Create Simple Character AnimatorController")]
    public static void Execute()
    {
        List<Texture> textures = Selection.objects.OfType<Texture>().ToList();
        if (!textures.Any())
        {
            Debug.LogWarning("Please selecting texture.");
            return;
        }

        Texture targetTexture = textures.First();

        string assetPath = AssetDatabase.GetAssetPath(targetTexture);
        SpriteDivider.Execute(assetPath, 3, 4);

        List<Sprite> sprites = AssetDatabase
            .LoadAllAssetRepresentationsAtPath(assetPath)
            .OfType<Sprite>()
            .ToList();

        SimpleCharacterAnimatorControllerDefinition definition = new SimpleCharacterAnimatorControllerDefinition
        {
            ResulutPath = "Assets/SimpleAnimator.controller",
            DefaultAnimationClip = CreateSpriteAnimationClip(true, "Default", 0.2F, sprites[0], sprites[1], sprites[2], sprites[1]),
            PinchAnimationClip = CreateSpriteAnimationClip(true, "Pinch", 0.2F, sprites[6], sprites[7], sprites[8], sprites[7]),
            HitAnimationClip = CreateSpriteAnimationClip(false, "Hit", 0.1F, sprites[3], sprites[4], sprites[5]),
            WalkAnimationClip = CreateSpriteAnimationClip(true, "Walk", 0.2F, sprites[9], sprites[10], sprites[11], sprites[10])
        };

        RuntimeAnimatorController animatorController = SimpleCharacterAnimatorControllerCreator.Create(definition);

        AssetDatabase.AddObjectToAsset(definition.DefaultAnimationClip, animatorController);
        AssetDatabase.AddObjectToAsset(definition.PinchAnimationClip, animatorController);
        AssetDatabase.AddObjectToAsset(definition.HitAnimationClip, animatorController);
        AssetDatabase.AddObjectToAsset(definition.WalkAnimationClip, animatorController);

        AssetDatabase.SaveAssets();
    }

    static AnimationClip CreateSpriteAnimationClip(bool isLoop, string name, float frameDuration, params Sprite[] sprites)
    {
        List<SpriteKeyframeDefinition> spriteKeyframes = sprites
            .Select((sprite, index) => new SpriteKeyframeDefinition
            {
                Value = sprite,
                Time = frameDuration * index
            }).ToList();

        return SpriteAnimationClipCreator.Create(new SpriteAnimationClipDefinition
        {
            SpriteKeyframes = spriteKeyframes,
            WrapMode = isLoop ? WrapMode.Loop : WrapMode.Default,
            IsLoop = isLoop,
            FrameRate = 4F,
            Name = name,
        });
    }
}
