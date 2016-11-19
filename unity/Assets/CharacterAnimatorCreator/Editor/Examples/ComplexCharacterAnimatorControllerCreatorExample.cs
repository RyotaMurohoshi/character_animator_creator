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

    [MenuItem("Assets/CharacterAnimatorCreator/Create Cpmplex Transition AnimatorController")]
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

        string[] suffixArray = new[] { "", "_1", "_2", "_3" };
        string[] texturePaths = suffixArray.Select(suffix => string.Format("CharacterImages/{0}/${1}{2}", name, name, suffix)).ToArray();

        foreach (var texturePath in texturePaths)
        {
            var imagePath = "Assets/Resources/" + texturePath + ".png";
            SpriteDivider.Execute(imagePath, 3, 4);
        }

        List<List<Sprite>> listOfSpriteList = texturePaths
            .Select(texturePath => Resources.LoadAll<Sprite>(texturePath).ToList())
            .ToList();

        ComplexCharacterAnimatorControllerCreator.CreateAnimatorController(new ComplexCharacterAnimatorControllerDefinition
        {
            AnimationClipDictionary = CreateDictionary(listOfSpriteList),
            ResulutPath = string.Format("{0}/{1}.controller", AnimatorControllerPath, name)
        });
    }

    static Dictionary<CharacterState, AnimationClip> CreateDictionary(List<List<Sprite>> sprites)
    {
        return new Dictionary<CharacterState, AnimationClip>{
            {
                CharacterState.Default,
                CreateAnimationClip (true, "Default", 0.2F, sprites[1][0], sprites[1][1], sprites[1][2], sprites[1][1])
            },
            {
                CharacterState.Dead,
                CreateAnimationClip (false, "Dead", 0.2F, sprites[2][9], sprites[2][10],sprites[2][11])
            },
            {
                CharacterState.Pinch,
                CreateAnimationClip (true, "Pinch", 0.2F, sprites[1][6], sprites[1][7], sprites[1][8], sprites[1][7])
            },
            {
                CharacterState.Hit,
                CreateAnimationClip (false, "Hit", 0.1F, sprites[1][3], sprites[1][4], sprites[1][5])
            },
            {
                CharacterState.Attack,
                CreateAnimationClip (false, "Attack", 0.1F, sprites[3][0], sprites[3][1], sprites[3][2])
            },
            {
                CharacterState.Down,
                CreateAnimationClip (true, "Down", 0.2F, sprites[0][0], sprites[0][1], sprites[0][2], sprites[0][1])
            },
            {
                CharacterState.Left,
                CreateAnimationClip (true, "Left", 0.2F, sprites[0][3], sprites[0][4], sprites[0][5], sprites[0][3])
            },
            {
                CharacterState.Right,
                CreateAnimationClip (true, "Right", 0.2F, sprites[0][6], sprites[0][7], sprites[0][8], sprites[0][7])
            },
            {
                CharacterState.Up,
                CreateAnimationClip (true, "Up", 0.2F, sprites[0][9], sprites[0][10], sprites[0][11], sprites[0][10])
            },
            {
                CharacterState.Walk,
                CreateAnimationClip (true, "Walk", 0.2F, sprites[1][9], sprites[1][10], sprites[1][11], sprites[1][10])
            },
            {
                CharacterState.Win,
                CreateAnimationClip (true, "Win", 0.2F, sprites[2][0], sprites[2][1], sprites[2][2], sprites[2][1])
            },
        };
    }

    static AnimationClip CreateAnimationClip(bool isLoop, string name, float frameDuration, params Sprite[] sprites)
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
