using UnityEngine;
using UnityEditor;
using System.Linq;

public static class SimpleTransitionAnimatorControllerExample
{
    [MenuItem("Assets/CharacterAnimatorCreator/Example/Simple Transition Create AnimatorController")]
    public static void CreateAnimatorController()
    {
        var textures = Selection.objects.OfType<Texture>();
        if (!textures.Any())
        {
            Debug.LogWarning("Please selecting texture.");
            return;
        }

        var targetTexture = textures.First();

        var assetPath = AssetDatabase.GetAssetPath(targetTexture);
        SpriteDivider.DividSprite(assetPath, 3, 4);

        var sprites = AssetDatabase
            .LoadAllAssetRepresentationsAtPath(assetPath)
            .OfType<Sprite>()
            .ToList();

        var definition = new SimpleTransitionAnimatorControllerDefinition
        {
            ResulutPath = "Assets/SimpleAnimator.controller",
            LayerName = "Base Layer",
            DefaultAnimationClip = CreateSpriteAnimationClip(true, "Default", 0.2F, sprites[0], sprites[1], sprites[2], sprites[1]),
            PinchAnimationClip = CreateSpriteAnimationClip(true, "Pinch", 0.2F, sprites[6], sprites[7], sprites[8], sprites[7]),
            HitAnimationClip = CreateSpriteAnimationClip(false, "Hit", 0.1F, sprites[3], sprites[4], sprites[5]),
            WalkAnimationClip = CreateSpriteAnimationClip(true, "Walk", 0.2F, sprites[9], sprites[10], sprites[11], sprites[10])
        };

        var animatorController = SimpleTransitionAnimatorControllerCreator.CreateAnimatorController(definition);

        AssetDatabase.AddObjectToAsset(definition.DefaultAnimationClip, animatorController);
        AssetDatabase.AddObjectToAsset(definition.PinchAnimationClip, animatorController);
        AssetDatabase.AddObjectToAsset(definition.HitAnimationClip, animatorController);
        AssetDatabase.AddObjectToAsset(definition.WalkAnimationClip, animatorController);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(definition.DefaultAnimationClip));
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(definition.PinchAnimationClip));
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(definition.HitAnimationClip));
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(definition.WalkAnimationClip));
    }

    static AnimationClip CreateSpriteAnimationClip(bool isLoop, string name, float frameDuration, params Sprite[] sprites)
    {
        var spriteKeyframes = sprites
            .Select((sprite, index) => new SpriteKeyframeDefinition
            {
                Value = sprite,
                Time = frameDuration * index
            }).ToList();

        var definition = new SpriteAnimationClipDefinition
        {
            SpriteKeyframes = spriteKeyframes,
            WrapMode = isLoop ? WrapMode.Loop : WrapMode.Default,
            IsLoop = isLoop,
            FrameRate = 4F,
            Name = name,
        };

        return SpriteAnimationClipCreator.ConvertToAnimationClip(definition);
    }
}
