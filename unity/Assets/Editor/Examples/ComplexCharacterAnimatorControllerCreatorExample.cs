using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections.Generic;

public static class ComplexCharacterAnimatorControllerCreatorExample
{
    static readonly string ResourcesFolderName;
    static readonly string AnimatorControllerFolderName;

    static readonly string AssetsPath;
    static readonly string ResourcesPath;
    static readonly string AnimatorControllerPath;

    static ComplexCharacterAnimatorControllerCreatorExample()
    {
        ResourcesFolderName = "Resources";
        AnimatorControllerFolderName = "AnimatorController";

        AssetsPath = "Assets";
        ResourcesPath = string.Format("{0}/{1}", AssetsPath, ResourcesFolderName);
        AnimatorControllerPath = string.Format("{0}/{1}", ResourcesPath, AnimatorControllerFolderName);
    }


    [MenuItem("Assets/CharacterAnimatorCreator/Execute")]
    static void TextureToAnimatiorController()
    {
        if (!Selection.objects.Any())
        {
            Debug.LogWarning("Please select floders of character images.");
            return;
        }

        foreach (string name in Selection.objects.Select(it => it.name))
        {
            TextureToAnimatiorController(name);
        }
    }

    static void TextureToAnimatiorController(string name)
    {
        CreateFolderIfNotExist(AssetsPath, ResourcesFolderName);
        CreateFolderIfNotExist(ResourcesPath, AnimatorControllerFolderName);

        var suffixArray = new[] { "", "_1", "_2", "_3" };
        var texturePaths = suffixArray.Select(suffix => string.Format("CharacterImages/{0}/${1}{2}", name, name, suffix));

        foreach (var texturePath in texturePaths)
        {
            string target = "Assets/Resources/" + texturePath + ".png";
            SpriteDivider.Execute(target, 3, 4);
        }

        var listOfSpriteList = texturePaths
            .Select(texurePath => Resources.LoadAll<Sprite>(texurePath).ToList())
            .ToList();

        Dictionary<CharacterState, SpriteAnimationClipDefinition> definitionDict = CreateCharacterAnimatorDefinition(listOfSpriteList);
        Dictionary<CharacterState, AnimationClip> animationClipDictionary = definitionDict
            .Select(entry => new { Key = entry.Key, Value = SpriteAnimationClipCreator.Create(entry.Value) })
            .ToDictionary(entry => entry.Key, entry => entry.Value);

        string animatorControllerPath = string.Format("{0}/{1}.controller", AnimatorControllerPath, name);
        ComplexCharacterAnimatorControllerCreator.CreateAnimatorController(animationClipDictionary, animatorControllerPath);
    }

    static Dictionary<CharacterState, SpriteAnimationClipDefinition> CreateCharacterAnimatorDefinition(List<List<Sprite>> sprites)
    {
        return new Dictionary<CharacterState, SpriteAnimationClipDefinition>{
            {
                CharacterState.Default,
                CreateSpriteAnimationClipDefinition (true, "Default", 0.2F, sprites[1][0], sprites[1][1], sprites[1][2], sprites[1][1])
            },
            {
                CharacterState.Dead,
                CreateSpriteAnimationClipDefinition (false, "Dead", 0.2F, sprites[2][9], sprites[2][10],sprites[2][11])
            },
            {
                CharacterState.Pinch,
                CreateSpriteAnimationClipDefinition (true, "Pinch", 0.2F, sprites[1][6], sprites[1][7], sprites[1][8], sprites[1][7])
            },
            {
                CharacterState.Hit,
                CreateSpriteAnimationClipDefinition (false, "Hit", 0.1F, sprites[1][3], sprites[1][4], sprites[1][5])
            },
            {
                CharacterState.Attack,
                CreateSpriteAnimationClipDefinition (false, "Attack", 0.1F, sprites[3][0], sprites[3][1], sprites[3][2])
            },
            {
                CharacterState.Down,
                CreateSpriteAnimationClipDefinition (true, "Down", 0.2F, sprites[0][0], sprites[0][1], sprites[0][2], sprites[0][1])
            },
            {
                CharacterState.Left,
                CreateSpriteAnimationClipDefinition (true, "Left", 0.2F, sprites[0][3], sprites[0][4], sprites[0][5], sprites[0][3])
            },
            {
                CharacterState.Right,
                CreateSpriteAnimationClipDefinition (true, "Right", 0.2F, sprites[0][6], sprites[0][7], sprites[0][8], sprites[0][7])
            },
            {
                CharacterState.Up,
                CreateSpriteAnimationClipDefinition (true, "Up", 0.2F, sprites[0][9], sprites[0][10], sprites[0][11], sprites[0][10])
            },
            {
                CharacterState.Walk,
                CreateSpriteAnimationClipDefinition (true, "Walk", 0.2F, sprites[1][9], sprites[1][10], sprites[1][11], sprites[1][10])
            },
            {
                CharacterState.Win,
                CreateSpriteAnimationClipDefinition (true, "Win", 0.2F, sprites[2][0], sprites[2][1], sprites[2][2], sprites[2][1])
            },
        };
    }

    static SpriteAnimationClipDefinition CreateSpriteAnimationClipDefinition(bool isLoop, string name, float frameDuration, params Sprite[] sprites)
    {
        List<SpriteKeyframeDefinition> spriteKeyframes = sprites
            .Select((sprite, index) => new SpriteKeyframeDefinition
            {
                Value = sprite,
                Time = frameDuration * index
            }).ToList();

        return new SpriteAnimationClipDefinition
        {
            SpriteKeyframes = spriteKeyframes,
            WrapMode = isLoop ? WrapMode.Loop : WrapMode.Default,
            IsLoop = isLoop,
            FrameRate = 4F,
            Name = name,
        };
    }

    static void CreateFolderIfNotExist(string parentFolder, string newFolderName)
    {
        string expectPath = string.Format("{0}/{1}", parentFolder, newFolderName);
        string guid = AssetDatabase.CreateFolder(parentFolder, newFolderName);
        string actualPath = AssetDatabase.GUIDToAssetPath(guid);
        bool isAlreadyExist = expectPath != actualPath;

        if (isAlreadyExist)
        {
            AssetDatabase.DeleteAsset(actualPath);
        }
    }
}
