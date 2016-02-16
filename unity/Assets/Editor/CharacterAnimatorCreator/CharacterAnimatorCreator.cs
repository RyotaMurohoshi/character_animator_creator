using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections.Generic;

public static class CharacterAnimatorCreator
{
    static readonly string ResourcesFolderName;
    static readonly string AnimationClipFolderName;
    static readonly string AnimatorControllerFolderName;
    static readonly string PrefabFolderName;

    static readonly string AssetsPath;
    static readonly string ResourcesPath;
    static readonly string AnimationClipPath;
    static readonly string AnimatorControllerPath;
    static readonly string PrefabPath;

    static CharacterAnimatorCreator()
    {
        ResourcesFolderName = "Resources";
        AnimationClipFolderName = "AnimationClip";
        AnimatorControllerFolderName = "AnimatorController";
        PrefabFolderName = "Prefab";

        AssetsPath = "Assets";
        ResourcesPath = string.Format("{0}/{1}", AssetsPath, ResourcesFolderName);
        AnimationClipPath = string.Format("{0}/{1}", ResourcesPath, AnimationClipFolderName);
        AnimatorControllerPath = string.Format("{0}/{1}", ResourcesPath, AnimatorControllerFolderName);
        PrefabPath = string.Format("{0}/{1}", ResourcesPath, PrefabFolderName);
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
        CreateFolderIfNotExist(ResourcesPath, AnimationClipFolderName);
        CreateFolderIfNotExist(ResourcesPath, AnimatorControllerFolderName);
        CreateFolderIfNotExist(ResourcesPath, PrefabFolderName);
        CreateFolderIfNotExist(AnimationClipPath, name);

        var suffixArray = new[] { "", "_1", "_2", "_3" };
        var texturePaths = suffixArray.Select(suffix => string.Format("CharacterImages/{0}/${1}{2}", name, name, suffix));

        foreach (var texturePath in texturePaths)
        {
            string target = "Assets/Resources/" + texturePath + ".png";
            SpriteDivider.DividSprite(target, 3, 4);
        }

        var listOfSpriteList = texturePaths
            .Select(texurePath => Resources.LoadAll<Sprite>(texurePath).ToList())
            .ToList();

        string animationClipsOutputFolderName = string.Format("{0}/{1}", AnimationClipPath, name);
        Dictionary<CharacterState, SpriteAnimationClipDefinition> definitionDict =
            CreateCharacterAnimatorDefinition(listOfSpriteList, animationClipsOutputFolderName);
        Dictionary<CharacterState, AnimationClip> animationClipDictionary = definitionDict
            .Select(entry => new { Key = entry.Key, Value = SpriteAnimationClipCreator.CreateAnimationClip(entry.Value) })
            .ToDictionary(entry => entry.Key, entry => entry.Value);

        string animatorControllerPath = string.Format("{0}/{1}.controller", AnimatorControllerPath, name);
        RuntimeAnimatorController animatorController =
            CharacterAnimatorControllerCreator.CreateAnimatorController(animationClipDictionary, animatorControllerPath);

        string prefabPath = string.Format("{0}/{1}.prefab", PrefabPath, name);
        CreatePrefab(animatorController, prefabPath);
    }

    static Dictionary<CharacterState, SpriteAnimationClipDefinition> CreateCharacterAnimatorDefinition(List<List<Sprite>> sprites, string outputDirectoryName)
    {
        return new Dictionary<CharacterState, SpriteAnimationClipDefinition>{
            {
                CharacterState.Default,
                CreateSpriteAnimationClipDefinition (true, string.Format("{0}/Default.anim", outputDirectoryName), 0.2F, sprites[1][0], sprites[1][1], sprites[1][2], sprites[1][1])
            },
            {
                CharacterState.Dead,
                CreateSpriteAnimationClipDefinition (false, string.Format("{0}/Dead.anim", outputDirectoryName), 0.2F, sprites[2][9], sprites[2][10],sprites[2][11])
            },
            {
                CharacterState.Pinch,
                CreateSpriteAnimationClipDefinition (true, string.Format("{0}/Pinch.anim", outputDirectoryName), 0.2F, sprites[1][6], sprites[1][7], sprites[1][8], sprites[1][7])
            },
            {
                CharacterState.Hit,
                CreateSpriteAnimationClipDefinition (false, string.Format("{0}/Hit.anim", outputDirectoryName), 0.1F, sprites[1][3], sprites[1][4], sprites[1][5])
            },
            {
                CharacterState.Attack,
                CreateSpriteAnimationClipDefinition (false, string.Format("{0}/Attack.anim", outputDirectoryName), 0.1F, sprites[3][0], sprites[3][1], sprites[3][2])
            },
            {
                CharacterState.Down,
                CreateSpriteAnimationClipDefinition (true, string.Format("{0}/Down.anim", outputDirectoryName), 0.2F, sprites[0][0], sprites[0][1], sprites[0][2], sprites[0][1])
            },
            {
                CharacterState.Left,
                CreateSpriteAnimationClipDefinition (true, string.Format("{0}/Left.anim", outputDirectoryName), 0.2F, sprites[0][3], sprites[0][4], sprites[0][5], sprites[0][3])
            },
            {
                CharacterState.Right,
                CreateSpriteAnimationClipDefinition (true, string.Format("{0}/Right.anim", outputDirectoryName), 0.2F, sprites[0][6], sprites[0][7], sprites[0][8], sprites[0][7])
            },
            {
                CharacterState.Up,
                CreateSpriteAnimationClipDefinition (true, string.Format("{0}/Up.anim", outputDirectoryName), 0.2F, sprites[0][9], sprites[0][10], sprites[0][11], sprites[0][10])
            },
        };
    }

    static SpriteAnimationClipDefinition CreateSpriteAnimationClipDefinition(bool isLoop, string resultPath, float frameDuration, params Sprite[] sprites)
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
            ResulutPath = resultPath,
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

    static GameObject CreatePrefab(RuntimeAnimatorController animatorController, string outputPath)
    {
        GameObject gameObject = EditorUtility.CreateGameObjectWithHideFlags(
            animatorController.name,
            HideFlags.HideInHierarchy,
            typeof(SpriteRenderer), typeof(Animator)
        );

        gameObject.GetComponent<Animator>().runtimeAnimatorController = animatorController;

        GameObject result = PrefabUtility.CreatePrefab(outputPath, gameObject, ReplacePrefabOptions.ReplaceNameBased);

        Editor.DestroyImmediate(gameObject);

        return result;
    }
}
